using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AudioLogger.Services;
using Ini;
using log4net;
using NAudio.Wave;

namespace AudioLogger.Application
{
    public partial class ApplicationForm : Form
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (ApplicationForm));
        private readonly IniFile _config = new IniFile(Directory.GetCurrentDirectory() + "/config.ini");
        
        private string _filelenght;
        private string _filepathMp3;
        private string _filepathWav;
        private int _progress;
        private int _progressTotal;
        public WaveIn Device;
        public int DeviceId;

        // These were used in the pre-merge Recorder, for now they're unused
        private string _uploadDirectory;
        private bool _keepMp3;
        private bool _keepWav;
        private string _uploadType;
        private string _filetype;


        private readonly IConverterService _converterService;
        private readonly IFtpClientService _ftpClientService;
        private readonly IRecorderService _recorderService;

        public ApplicationForm(IFtpClientService ftpClientService,
            IRecorderService recorderService,
            IConverterService converterService)
        {
            if (ftpClientService == null) throw new ArgumentException("ftpClientService");
            if (recorderService == null) throw new ArgumentException("recorderService");
            if (converterService == null) throw new ArgumentException("converterService");

            _ftpClientService = ftpClientService;
            _recorderService = recorderService;
            _converterService = converterService;

            InitializeComponent();

            var waveInDevices = WaveIn.DeviceCount;
            for (var waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                var deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                var item = new ComboboxItem();
                item.Text = deviceInfo.ProductName + " [" + deviceInfo.Channels + "]";
                item.Value = waveInDevice;
                cb_soundcard.Items.Add(item);
            }
            cb_soundcard.SelectedItem = cb_soundcard.SelectedIndex = 0;
            cb_lenght.SelectedItem = cb_lenght.SelectedIndex = 0;
            cb_path_wav.SelectedItem = cb_path_wav.SelectedIndex = 0;
            cb_path_mp3.SelectedItem = cb_path_mp3.SelectedIndex = 0;

            tb_hostname.Text = _config.IniReadValue("ftp", "host");
            tb_directory.Text = _config.IniReadValue("ftp", "targetDir");
            tb_username.Text = _config.IniReadValue("ftp", "user");
            tb_password.Text = _config.IniReadValue("ftp", "pass");

            if (_config.IniReadValue("file", "keepWav") == "True")
            {
                cb_keepWav.Checked = true;
            }
            else
            {
                cb_keepWav.Checked = false;
            }

            if (_config.IniReadValue("file", "keepMp3") == "True")
            {
                cb_keepMp3.Checked = true;
            }
            else
            {
                cb_keepMp3.Checked = false;
            }

            if (_config.IniReadValue("upload", "format") == "wav")
            {
                r_wav.Checked = true;
            }

            cb_uploadType.Text = _config.IniReadValue("upload", "type");
            tb_fileUploadDir.Text = _config.IniReadValue("directory", "path");
        }

        public void set_device(object sender, EventArgs e)
        {
            var selectedDevice = (ComboboxItem) cb_soundcard.SelectedItem;
            DeviceId = Convert.ToInt32(selectedDevice.Value);
        }

        public void btn_start_Click(object sender, EventArgs e)
        {
            cb_soundcard.Enabled = false;
            btn_stop.Enabled = true;
            btn_start.Enabled = false;
            cb_lenght.Enabled = false;
            cb_path_wav.Enabled = false;
            cb_path_mp3.Enabled = false;
            r_mp3.Enabled = false;
            r_wav.Enabled = false;
            cb_uploadType.Enabled = false;
            cb_keepMp3.Enabled = false;
            cb_keepWav.Enabled = false;
            tb_fileUploadDir.Enabled = false;

            Invoke(new MethodInvoker(delegate { _filelenght = cb_lenght.Text; }));
            Invoke(new MethodInvoker(delegate { _filepathWav = cb_path_wav.Text; }));
            Invoke(new MethodInvoker(delegate { _filepathMp3 = cb_path_mp3.Text; }));
            Invoke(new MethodInvoker(delegate { _progressTotal = progressBar1.Maximum; }));
            Invoke(new MethodInvoker(delegate { _progress = progressBar1.Value; }));
            Invoke(new MethodInvoker(delegate { _keepMp3 = cb_keepMp3.Checked; }));
            Invoke(new MethodInvoker(delegate { _keepWav = cb_keepWav.Checked; }));
            Invoke(new MethodInvoker(delegate { _uploadType = cb_uploadType.Text; }));
            Invoke(new MethodInvoker(delegate { _uploadDirectory = tb_fileUploadDir.Text; }));


            if (r_mp3.Checked)
            {
                _filetype = "mp3";
            }
            else
            {
                _filetype = "wav";
            }

            inzinierius.RunWorkerAsync();
        }

        public void btn_stop_Click(object sender, EventArgs e)
        {
            inzinierius.CancelAsync();
        }

        private DateTime roundup(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1)/d.Ticks)*d.Ticks);
        }

        private void inzinierius_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                var fullBasePathNoExt = GenerateFilenameFromCurrentDate();
                var fullpathWav = fullBasePathNoExt + ".wav";
                var fullpathMp3 = fullBasePathNoExt + ".mp3";
                _recorderService.StartRecording(DeviceId, fullpathWav);
                var now = DateTime.Now;
                var fileLenghtrequested = Convert.ToInt32(_filelenght);
                var endofSpan = roundup(now, TimeSpan.FromMinutes(fileLenghtrequested));
                var span = endofSpan.Subtract(now);
                var sleeptime = 1000*Convert.ToInt32(span.TotalSeconds) + 1000;
                _progressTotal = Convert.ToInt32(sleeptime);
                var i = 0;

                while (i < Convert.ToInt32(sleeptime))
                {
                    i++;
                    _progress = i;
                    Thread.Sleep(1);
                    if (inzinierius.CancellationPending)
                    {
                        i = sleeptime;
                    }
                }

                _recorderService.StopRecording();
                var asyncConvert = new Thread(() => AsyncConvertAndUpload(fullpathWav, fullpathMp3));
                asyncConvert.Start();
            } while (!inzinierius.CancellationPending);

            e.Cancel = true;
        }

        private string GenerateFilenameFromCurrentDate()
        {
            return String.Format("{0}\\{1}", _filepathWav,
                DateTime.Now.ToString(Configuration.Default.AudioFilenameFormat));
        }

        private void AsyncConvertAndUpload(string fullpathWav, string fullpathMp3)
        {
            _converterService.AsyncConvert(fullpathWav, fullpathMp3);
            _converterService.Wait();

            // Retry cycle
            var retryCount = 3;
            for (var i = 0; i < retryCount; i++)
            {
                if (_ftpClientService.TryUploadFile(_filepathMp3, fullpathMp3.Split('\\').Last()))
                {
                    break;
                }
                Logger.Warn(string.Format("Attempt {0} failed", i + 1));
                if (i < retryCount)
                {
                    Logger.Error("Failed to upload file");
                }
                Thread.Sleep(2000);
            }
        }

        private void inzinierius_RunWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cb_soundcard.Enabled = true;
            btn_stop.Enabled = false;
            btn_start.Enabled = true;
            cb_lenght.Enabled = true;
            cb_path_wav.Enabled = true;
            cb_path_mp3.Enabled = true;
            r_mp3.Enabled = true;
            r_wav.Enabled = true;
            cb_uploadType.Enabled = true;
            cb_keepMp3.Enabled = true;
            cb_keepWav.Enabled = true;
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            _config.IniWriteValue("upload", "type", cb_uploadType.Text);

            if (r_wav.Checked)
            {
                _config.IniWriteValue("upload", "format", "wav");
            }
            else
            {
                _config.IniWriteValue("upload", "format", "mp3");
            }

            _config.IniWriteValue("ftp", "host", tb_hostname.Text);
            _config.IniWriteValue("ftp", "targetDir", tb_directory.Text);
            _config.IniWriteValue("ftp", "user", tb_username.Text);
            _config.IniWriteValue("ftp", "pass", tb_password.Text);

            _config.IniWriteValue("file", "pathWav", cb_path_wav.Text);
            _config.IniWriteValue("file", "pathMp3", cb_path_mp3.Text);
            _config.IniWriteValue("file", "span", cb_lenght.Text);
            _config.IniWriteValue("file", "keepWav", cb_keepWav.Checked.ToString());
            _config.IniWriteValue("file", "keepMp3", cb_keepMp3.Checked.ToString());

            _config.IniWriteValue("directory", "path", tb_fileUploadDir.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Maximum = _progressTotal;
            progressBar1.Value = _progress;
            progressBar1.Refresh();
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}