using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AudioLogger.Services;
using NAudio.CoreAudioApi;
using Ini;
using log4net;
using NAudio.Wave;
using System.Runtime.InteropServices;
using System.Diagnostics;



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
        public string DeviceName;

        // These were used in the pre-merge Recorder, for now they're unused
        private string _uploadDirectory;
        private string _uploadType;

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
                item.Text = deviceInfo.ProductName;
                item.Value = waveInDevice;
                cb_soundcard.Items.Add(item);
            }
            cb_soundcard.SelectedItem = cb_soundcard.SelectedIndex = 0;
            cb_lenght.SelectedItem = cb_lenght.SelectedIndex = 0;
            cb_temp_path.SelectedItem = cb_temp_path.SelectedIndex = 0;

            tb_hostname.Text = _config.IniReadValue("ftp", "host");
            tb_directory.Text = _config.IniReadValue("ftp", "targetDir");
            tb_username.Text = _config.IniReadValue("ftp", "user");
            tb_password.Text = _config.IniReadValue("ftp", "pass");

            cb_uploadType.Text = _config.IniReadValue("upload", "type");
            tb_fileUploadDir.Text = _config.IniReadValue("directory", "path");
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;


        public void set_device(object sender, EventArgs e)
        {
            var selectedDevice = (ComboboxItem) cb_soundcard.SelectedItem;
            DeviceId = Convert.ToInt32(selectedDevice.Value);
            DeviceName = selectedDevice.Text;
        }

        public void btn_start_Click(object sender, EventArgs e)
        {
            cb_soundcard.Enabled = false;
            btn_stop.Enabled = true;
            btn_start.Enabled = false;
            cb_lenght.Enabled = false;
            cb_temp_path.Enabled = false;
            cb_uploadType.Enabled = false;
            tb_fileUploadDir.Enabled = false;

            Invoke(new MethodInvoker(delegate { _filelenght = cb_lenght.Text; }));
            Invoke(new MethodInvoker(delegate { _filepathWav = cb_temp_path.Text; }));
            Invoke(new MethodInvoker(delegate { _filepathMp3 = cb_temp_path.Text; }));
            Invoke(new MethodInvoker(delegate { _progressTotal = progressBar1.Maximum; }));
            Invoke(new MethodInvoker(delegate { _progress = progressBar1.Value; }));
            Invoke(new MethodInvoker(delegate { _uploadType = cb_uploadType.Text; }));
            Invoke(new MethodInvoker(delegate { _uploadDirectory = tb_fileUploadDir.Text; }));

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
                try
                {
                    _recorderService.StartRecording(DeviceId, _filepathWav, _filepathMp3);
                }
                catch(Exception ex)
                {
                    Logger.Error(ex.Message);
                    throw ex;
                }
                
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
                new Thread(AsyncConvertAndUpload);
            } while (!inzinierius.CancellationPending);

            e.Cancel = true;
        }

        private void AsyncConvertAndUpload()
        {
            _converterService.AsyncConvert(_filepathWav + _recorderService.FilenameWav,
                _filepathMp3 + _recorderService.FilenameMp3);
            _converterService.Wait();

            // Retry cycle
            var retryCount = 3;
            for (var i = 0; i < retryCount; i++)
            {
                if (_ftpClientService.TryUploadFile(_filepathMp3, _recorderService.FilenameMp3))
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
            cb_temp_path.Enabled = true;
            cb_uploadType.Enabled = true;
            tb_fileUploadDir.Enabled = true;
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            _config.IniWriteValue("upload", "type", cb_uploadType.Text);
            _config.IniWriteValue("ftp", "host", tb_hostname.Text);
            _config.IniWriteValue("ftp", "targetDir", tb_directory.Text);
            _config.IniWriteValue("ftp", "user", tb_username.Text);
            _config.IniWriteValue("ftp", "pass", tb_password.Text);

            _config.IniWriteValue("file", "pathWav", cb_temp_path.Text);
            _config.IniWriteValue("file", "pathMp3", cb_temp_path.Text);
            _config.IniWriteValue("file", "span", cb_lenght.Text);

            _config.IniWriteValue("directory", "path", tb_fileUploadDir.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Maximum = _progressTotal;
            progressBar1.Value = _progress;
            progressBar1.Refresh();

            MMDeviceEnumerator de = new MMDeviceEnumerator();
            
            var device = (MMDevice)de.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia); //<-- veikia Default input device

            //var device = (MMDevice)de.GetDevice(DeviceName); // <-- niaveikia, crashina, jei nori pasirinkt device is comboBoxo
            // greiciausiai nes neatitinka DeviceName gautas is WaveIn ir MMDevice ID.

            peak_L.Value = (int)(device.AudioMeterInformation.MasterPeakValue * 50 + 0.25);
            peak_R.Value = (int)(device.AudioMeterInformation.MasterPeakValue * 50 + 0.25);
            peak_L.Refresh();
            peak_R.Refresh();
            
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

        private void bt_browseWinDir_Click(object sender, EventArgs e)
        {
            DialogResult result = dx_browseWinDir.ShowDialog();
            if (result == DialogResult.OK)
            {
                tb_fileUploadDir.Text = dx_browseWinDir.SelectedPath;
            }
        }

        private void bt_exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bt_minimyze_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

    }
}