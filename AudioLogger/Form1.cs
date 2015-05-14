using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AudioLogger.Services;
using Ini;
using NAudio.Wave;

namespace AudioLogger
{
    public partial class Form1 : Form
    {
        private readonly IniFile _config = new IniFile(Directory.GetCurrentDirectory() + "/config.ini");
        private string _filelenght;
        private string _filepathMp3;
        private string _filepathWav;
        private IFtpClientService _ftpClientService;
        private int _progress;
        private int _progressTotal;
        public WaveIn Device;
        public int DeviceId;

        public Form1(IFtpClientService ftpClientService)
        {
            if (ftpClientService == null) throw new ArgumentException("ftpClientService");
            _ftpClientService = ftpClientService;

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
            Invoke(new MethodInvoker(delegate { _filelenght = cb_lenght.Text; }));
            Invoke(new MethodInvoker(delegate { _filepathWav = cb_path_wav.Text; }));
            Invoke(new MethodInvoker(delegate { _filepathMp3 = cb_path_mp3.Text; }));

            Invoke(new MethodInvoker(delegate { _progressTotal = progressBar1.Maximum; }));
            Invoke(new MethodInvoker(delegate { _progress = progressBar1.Value; }));

            inzinierius.RunWorkerAsync();
        }

        public void btn_stop_Click(object sender, EventArgs e)
        {
            l_status.Invoke((MethodInvoker) delegate { l_status.Text = "Stopping at cycle end."; });
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
                var kasete = new Recorder(DeviceId, _filepathWav, _filepathMp3);

                var now = DateTime.Now;
                var fileLenghtrequested = Convert.ToInt32(_filelenght);
                var endofSpan = roundup(now, TimeSpan.FromMinutes(fileLenghtrequested));
                var span = endofSpan.Subtract(now);

                var sleeptime = 1000*Convert.ToInt32(span.TotalSeconds) + 1000;

                l_status.Invoke(
                    (MethodInvoker) delegate { l_status.Text = "Next file will be " + span.Minutes + " min. long."; });

                Debug.WriteLine("Sleep time: " + sleeptime);
                Debug.WriteLine("Creating file of " + span.Minutes + " min.");
                _progressTotal = Convert.ToInt32(sleeptime);
                var i = 0;

                while (i < Convert.ToInt32(sleeptime))
                {
                    i++;
                    _progress = i;
                    Thread.Sleep(1);
                    Debug.WriteLine(_progress);
                }

                kasete.disableRecorder();
            } while (!inzinierius.CancellationPending);

            e.Cancel = true;
        }

        private void inzinierius_RunWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cb_soundcard.Enabled = true;
            btn_stop.Enabled = false;
            btn_start.Enabled = true;
            cb_lenght.Enabled = true;
            cb_path_wav.Enabled = true;
            cb_path_mp3.Enabled = true;
            l_status.Text = "Stopped.";
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            _config.IniWriteValue("ftp", "host", tb_hostname.Text);
            _config.IniWriteValue("ftp", "targetDir", tb_directory.Text);
            _config.IniWriteValue("ftp", "user", tb_username.Text);
            _config.IniWriteValue("ftp", "pass", tb_password.Text);

            _config.IniWriteValue("file", "pathWav", cb_path_wav.Text);
            _config.IniWriteValue("file", "pathMp3", cb_path_mp3.Text);
            _config.IniWriteValue("file", "span", cb_lenght.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Maximum = _progressTotal;
            progressBar1.Value = _progress;
            progressBar1.Refresh();
        }
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