using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AudioLogger.Services;
using NAudio.CoreAudioApi;
using Ini;
using log4net;
using Microsoft.Practices.ObjectBuilder2;
using NAudio.Wave;

namespace AudioLogger.Application
{
    public partial class ApplicationForm : Form
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (ApplicationForm));
        private static readonly Configuration ApplicationConfiguration = Configuration.Default;
        private readonly IniFile _config = new IniFile(
            Extentions.GetProgramFolder() + 
            ApplicationConfiguration.IniFilename);

        private static Parameters AppParameters { get; set; }

        private string _filelenght;
        private int _progress;
        private int _progressTotal;
        private readonly Dictionary<int, Device> _devices;
        private Device _activeDevice;

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

            AppParameters = new Parameters {TemporaryFolder = ApplicationConfiguration.TemporaryFolder.GetProgramDataSubFolder()};

            InitializeComponent();

            _devices = new Dictionary<int, Device>();
            var deviceCount = WaveIn.DeviceCount;
            for (var deviceId = 0; deviceId < deviceCount; deviceId++)
                _devices.Add(deviceId, new Device(WaveIn.GetCapabilities(deviceId), deviceId));
            _devices.Add(deviceCount + 1, new Device {ProductName = "Windows mixed output",  DeviceId = int.MaxValue});
            _devices.ForEach(device => cb_soundcard.Items.Add(new ComboboxItem {Text = device.Value.ProductName, Value = device.Key}));

            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            cb_soundcard.SelectedItem = cb_soundcard.SelectedIndex = 0;
            tb_length.Text = _config.IniReadValue("general", "length");
            tb_keepFilesForDays.Text = _config.IniReadValue("general", "retention_rate");

            tb_hostname.Text = _config.IniReadValue("ftp", "host");
            tb_directory.Text = _config.IniReadValue("ftp", "targetDir");
            tb_username.Text = _config.IniReadValue("ftp", "user");

            tb_password.Text = _encryptionService.Decrypt(_config.IniReadValue("ftp", "pass"));

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
            if (!_devices.TryGetValue(selectedDevice.Value, out _activeDevice))
                throw new Exception("An error occured during device selection"); 
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

            InitializeProperties();
            if (!ValidateFields()) return;

            if (CheckAndWarnForFileDeletion()) return;

            inzinierius.RunWorkerAsync();
        }

        private bool CheckAndWarnForFileDeletion()
        {
            IUploadService service = UploadServiceFactory.CreateUploadService(AppParameters);
            var deletableFiles = service.GetFilesOlderThan(GetEndOfLifeDate()).ToArray();
            if (deletableFiles.Count() > ApplicationConfiguration.WarningTrigger)
            {
                var result =
                    MessageBox.Show(
                        $"{deletableFiles.Length} files of recordings will be deleted when this cycle is finished. Are you sure you want to do this?",
                        "Warning", MessageBoxButtons.YesNo);

                if (CancelIfUnsatisfied(
                    result.Equals(DialogResult.No),
                    "Check the 'keep for' field, you may have entered an incorrect value."))
                    return true;
            }
            return false;
        }

        private void InitializeProperties()
        {
            Invoke(new MethodInvoker(delegate { _filelenght = tb_length.Text; }));
            Invoke(new MethodInvoker(delegate { _progressTotal = progressBar1.Maximum; }));
            Invoke(new MethodInvoker(delegate { _progress = progressBar1.Value; }));
            Invoke(new MethodInvoker(delegate { AppParameters.WindowsDirectoryUploadTarget = tb_fileUploadDir.Text; }));
            Invoke(new MethodInvoker(delegate { AppParameters.UploadType = cb_uploadType.Text; }));
            Invoke(new MethodInvoker(delegate { AppParameters.FtpHost = tb_hostname.Text; }));
            Invoke(new MethodInvoker(delegate { AppParameters.FtpUsername = tb_username.Text; }));
            Invoke(new MethodInvoker(delegate { AppParameters.FtpPassword = tb_password.Text; }));
            Invoke(
                new MethodInvoker(
                    delegate { AppParameters.FileNameFromDateFormat = ApplicationConfiguration.AudioFilenameFormat; }));
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
        }

        private bool ValidateFields()
        {
            if (CancelIfUnsatisfied(AppParameters.RetentionRateInDays < 0,
                "Please enter a valid number to keep for field")) return false;

            if (CancelIfUnsatisfied(AppParameters.RecordingDurationInMinutes == 0,
                "Please enter a valid number to time span field")) return false;

            if (AppParameters.UploadType.Equals("FTP"))
            {
                if (CancelIfUnsatisfied(!new FtpUploadService(AppParameters).TestConnection(),
                    "FTP connection check failed. Server details incorrect?")) return false;
            }
            else if (AppParameters.UploadType.Equals("Windows directory"))
            {
                if (CancelIfUnsatisfied(!Directory.Exists(AppParameters.WindowsDirectoryUploadTarget),
                    "The specified windows directory for uploading does not exist!")) return false;
            }

            return true;
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

        private bool CancelIfUnsatisfied(bool condition, string message)
        {
            if (condition)
            {
                MessageBox.Show(message);
                btn_stop_Click(this, null);
            }
            return condition;
        }

        private DateTime roundup(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1)/d.Ticks)*d.Ticks);
        }

        private void inzinierius_DoWork(object sender, DoWorkEventArgs e)
        {
            _recorderService.Setup(_activeDevice);
            _recorderService.StartRecording();
            do
            {
                var audioLog = new AudioLog(AppParameters.TemporaryFolder, DateTime.Now, AudioLogFormat.Wav);

                _recorderService.WaveFile(audioLog);
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

                var asyncConvert = new Thread(() => AsyncConvertAndUpload(audioLog));
                asyncConvert.Start();
            } while (!inzinierius.CancellationPending);
            _recorderService.StopRecording();
            e.Cancel = true;
        }

        private void AsyncConvertAndUpload(AudioLog audioLog)
        {
            Thread.Sleep(1000);

            _converterService.AsyncConvert(audioLog.GetWav(), audioLog.GetMp3());
            _converterService.Wait();
            try
            {
                UploadConvertedFile(audioLog);

                File.Delete(audioLog.GetWav());
                File.Delete(audioLog.GetMp3());
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        private void UploadConvertedFile(AudioLog audioLog)
        {
            var uploadService = UploadServiceFactory.CreateUploadService(AppParameters);

            // Retry cycle
            var retryCount = 3;
            for (var i = 0; i < retryCount; i++)
            {
                if (uploadService.TryUploadFile(audioLog))
                {
                    break;
                }
                Logger.Error($"Upload attempt {i + 1} failed");
                if (i >= retryCount)
                {
                    Logger.Error("Failed to upload file");
                }
                Thread.Sleep(2000);
            }

            FileManager.TryDeleteOldFiles(uploadService, GetEndOfLifeDate());
        }

        private DateTime GetEndOfLifeDate()
        {
            return DateTime.Now.Subtract(TimeSpan.FromDays(AppParameters.RetentionRateInDays));
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

            var device = de.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);
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
            Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
        }
    }
}