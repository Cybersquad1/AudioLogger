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

namespace AudioLogger.Application
{
    public partial class ApplicationForm : Form
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (ApplicationForm));
        private readonly IniFile _config = new IniFile(Configuration.Default.ProgramDataFolder + "/config.ini");

        private static Parameters AppParameters { get; set; }

        private string _filelenght;
        private int _progress;
        private int _progressTotal;
        public WaveIn Device;
        private int _deviceId;
        public string DeviceName;

        private readonly IConverterService _converterService;
        private readonly IRecorderService _recorderService;
        private readonly IEncryptionService _encryptionService;

        public ApplicationForm(IRecorderService recorderService,
            IConverterService converterService,
            IEncryptionService encryptionService)
        {
            if (recorderService == null) throw new ArgumentException("recorderService");
            if (converterService == null) throw new ArgumentException("converterService");
            if (encryptionService == null) throw new ArgumentException("encryptionService");

            _recorderService = recorderService;
            _converterService = converterService;
            _encryptionService = encryptionService;

            AppParameters = new Parameters {TemporaryFolder = Configuration.Default.TemporaryFolder};

            InitializeComponent();

            var waveInDevices = WaveIn.DeviceCount;
            for (var waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                var deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                var item = new ComboboxItem
                {
                    Text = deviceInfo.ProductName,
                    Value = waveInDevice
                };
                cb_soundcard.Items.Add(item);
            }
            cb_soundcard.SelectedItem = cb_soundcard.SelectedIndex = 0;
            tb_length.Text = _config.IniReadValue("general", "length");
            tb_keepFilesForDays.Text = _config.IniReadValue("general", "retention_rate");

            tb_hostname.Text = _config.IniReadValue("ftp", "host");
            tb_directory.Text = _config.IniReadValue("ftp", "targetDir");
            tb_username.Text = _config.IniReadValue("ftp", "user");

            tb_password.Text = _encryptionService.Decrypt(_config.IniReadValue("ftp", "pass")).Trim((char)0x10); 

            cb_uploadType.Text = _config.IniReadValue("upload", "type");
            tb_fileUploadDir.Text = _config.IniReadValue("directory", "path");
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WmNchittest)
                m.Result = (IntPtr) (HtCaption);
        }

        private const int WmNchittest = 0x84;
        private const int HtCaption = 0x2;


        public void set_device(object sender, EventArgs e)
        {
            var selectedDevice = (ComboboxItem) cb_soundcard.SelectedItem;
            _deviceId = Convert.ToInt32(selectedDevice.Value);
            DeviceName = selectedDevice.Text;
        }

        public void btn_start_Click(object sender, EventArgs e)
        {
            cb_soundcard.Enabled = false;
            btn_stop.Enabled = true;
            btn_start.Enabled = false;
            tb_length.Enabled = false;
            tb_keepFilesForDays.Enabled = false;
            cb_uploadType.Enabled = false;
            tb_fileUploadDir.Enabled = false;

            // Parameter setting and validation
            Invoke(new MethodInvoker(delegate { _filelenght = tb_length.Text; }));
            Invoke(new MethodInvoker(delegate { _progressTotal = progressBar1.Maximum; }));
            Invoke(new MethodInvoker(delegate { _progress = progressBar1.Value; }));
            Invoke(new MethodInvoker(delegate { AppParameters.WindowsDirectoryUploadTarget = tb_fileUploadDir.Text; }));
            Invoke(new MethodInvoker(delegate { AppParameters.UploadType = cb_uploadType.Text; }));
            Invoke(new MethodInvoker(delegate { AppParameters.FtpHost = tb_hostname.Text; }));
            Invoke(new MethodInvoker(delegate { AppParameters.FtpUsername = tb_username.Text; }));
            Invoke(new MethodInvoker(delegate { AppParameters.FtpPassword = tb_password.Text; }));
            Invoke(new MethodInvoker(delegate { AppParameters.FtpTargetDirectory = tb_directory.Text; }));
            Invoke(new MethodInvoker(delegate
            {
                int result;
                AppParameters.RecordingDurationInMinutes = Int32.TryParse(tb_length.Text, out result) ? result : 0;
            }));
            Invoke(new MethodInvoker(delegate
            {
                int result;
                AppParameters.RetentionRateInDays = Int32.TryParse(tb_keepFilesForDays.Text, out result) ? result : 0;
            }));

            if (AppParameters.RetentionRateInDays == 0)
            {
                MessageBox.Show("Please enter a valid number to time span field", "Error");
                this.btn_stop_Click(this, e);
                return;
            }

            if (AppParameters.RecordingDurationInMinutes == 0)
            {
                MessageBox.Show("Please enter a valid number to time span field", "Error");
                this.btn_stop_Click(this, e);
                return;
            }

            if (AppParameters.UploadType.Equals("FTP"))
            {
                if (!new FtpUploadService(AppParameters).TestConnection())
                {
                    MessageBox.Show("FTP connection check failed. Server details incorrect?");
                    this.btn_stop_Click(this, e);
                    return;
                }
            }
            else if (AppParameters.UploadType.Equals("Windows directory"))
            {
                if (!Directory.Exists(AppParameters.WindowsDirectoryUploadTarget))
                {
                    MessageBox.Show("The specified windows directory for uploading does not exist!");
                    this.btn_stop_Click(this, e);
                    return;
                }
            }

            inzinierius.RunWorkerAsync();
        }

        public void btn_stop_Click(object sender, EventArgs e)
        {
            cb_soundcard.Enabled = true;
            btn_stop.Enabled = false;
            btn_start.Enabled = true;
            tb_length.Enabled = true;
            tb_keepFilesForDays.Enabled = true;
            cb_uploadType.Enabled = true;
            tb_fileUploadDir.Enabled = true;

            inzinierius.CancelAsync();
        }

        private DateTime roundup(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1)/d.Ticks)*d.Ticks);
        }

        private void inzinierius_DoWork(object sender, DoWorkEventArgs e)
        {
            _recorderService.Setup(_deviceId);
            _recorderService.StartRecording();
            do
            {
                var fullBasePathNoExt = GenerateFilenameFromCurrentDate();
                var fullpathWav = fullBasePathNoExt + ".wav";
                var fullpathMp3 = fullBasePathNoExt + ".mp3";
                _recorderService.WaveFile(fullpathWav);
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

                var asyncConvert = new Thread(() => AsyncConvertAndUpload(fullpathWav, fullpathMp3));
                asyncConvert.Start();
            } while (!inzinierius.CancellationPending);
            _recorderService.StopRecording();
            e.Cancel = true;
        }

        private string GenerateFilenameFromCurrentDate()
        {
            return String.Format("{0}\\{1}", AppParameters.TemporaryFolder,
                DateTime.Now.ToString(Configuration.Default.AudioFilenameFormat));
        }

        private void AsyncConvertAndUpload(string fullpathWav, string fullpathMp3)
        {
            Thread.Sleep(1000);
            try
            {
                _converterService.AsyncConvert(fullpathWav, fullpathMp3);
                _converterService.Wait();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }

            UploadConvertedFile(fullpathMp3);

            File.Delete(fullpathWav);
            File.Delete(fullpathMp3);
        }

        private void UploadConvertedFile(string fullpathMp3)
        {
            IUploadService uploadService = null;
            if (AppParameters.UploadType.Equals("FTP"))
            {
                uploadService = new FtpUploadService(AppParameters);
            }
            if (AppParameters.UploadType.Equals("Windows directory"))
            {
                uploadService = new WindowsDirectoryUploadService(AppParameters);
            }
            if (uploadService == null) throw new ArgumentException("Failed to construct upload service, the type selection must be broken.");

            // Retry cycle
            var retryCount = 3;
            for (var i = 0; i < retryCount; i++)
            {
                if (uploadService.TryUploadFile(fullpathMp3))
                {
                    break;
                }
                Logger.Error(string.Format("Upload attempt {0} failed", i + 1));
                if (i >= retryCount)
                {
                    Logger.Error("Failed to upload file");
                }
                Thread.Sleep(2000);
            }

            try
            {
                uploadService.RemoveFilesOlderThan(new DateTime(DateTime.Now.Day - AppParameters.RetentionRateInDays));
            }
            catch (Exception ex)
            {
                Logger.Warn("Could not remove old files");
                Logger.Warn(ex.Message);
            }
        }


        private void inzinierius_RunWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // ? 
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            _config.IniWriteValue("upload", "type", cb_uploadType.Text);
            _config.IniWriteValue("ftp", "host", tb_hostname.Text);
            _config.IniWriteValue("ftp", "targetDir", tb_directory.Text);
            _config.IniWriteValue("ftp", "user", tb_username.Text);
            _config.IniWriteValue("ftp", "pass", _encryptionService.Encrypt(tb_password.Text));

            _config.IniWriteValue("general", "length", tb_length.Text);
            _config.IniWriteValue("general", "retention_rate", tb_keepFilesForDays.Text);

            _config.IniWriteValue("directory", "path", tb_fileUploadDir.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Maximum = _progressTotal;
            progressBar1.Value = _progress;
            progressBar1.Refresh();

            MMDeviceEnumerator de = new MMDeviceEnumerator();

            var device = (MMDevice) de.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);
            //<-- veikia Default input device

            //var device = (MMDevice)de.GetDevice(DeviceName); // <-- niaveikia, crashina, jei nori pasirinkt device is comboBoxo
            // greiciausiai nes neatitinka DeviceName gautas is WaveIn ir MMDevice ID.

            peak_L.Value = (int) (device.AudioMeterInformation.MasterPeakValue*50 + 0.25);
            peak_R.Value = (int) (device.AudioMeterInformation.MasterPeakValue*50 + 0.25);
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