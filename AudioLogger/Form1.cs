using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using System.Diagnostics;
using Ini;

namespace AudioLogger
{
    public partial class Form1 : Form
    {
        public WaveIn device;
        public int deviceId;
        string filelenght;
        string filepathWav;
        string filepathMp3;
        int progressTotal;
        int progress;
        string filetype;
        bool keepMp3;
        bool keepWav;

        IniFile config = new IniFile(System.IO.Directory.GetCurrentDirectory() + "/config.ini");

        public Form1()
        {
            InitializeComponent();

            int waveInDevices = WaveIn.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                ComboboxItem item = new ComboboxItem();
                item.Text = deviceInfo.ProductName + " [" + deviceInfo.Channels + "]";
                item.Value = waveInDevice;
                cb_soundcard.Items.Add(item);
            }
            cb_soundcard.SelectedItem = cb_soundcard.SelectedIndex = 0;
            cb_lenght.SelectedItem = cb_lenght.SelectedIndex = 0;
            cb_path_wav.SelectedItem = cb_path_wav.SelectedIndex = 0;
            cb_path_mp3.SelectedItem = cb_path_mp3.SelectedIndex = 0;

            tb_hostname.Text = config.IniReadValue("ftp", "host");
            tb_directory.Text = config.IniReadValue("ftp", "targetDir");
            tb_username.Text = config.IniReadValue("ftp","user");
            tb_password.Text = config.IniReadValue("ftp","pass");
        }

        public void set_device(object sender, System.EventArgs e)
        {
            ComboboxItem selectedDevice = (ComboboxItem)cb_soundcard.SelectedItem;
            deviceId = Convert.ToInt32(selectedDevice.Value);
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
                cb_uploadFtp.Enabled = false;
                cb_keepMp3.Enabled = false;
                cb_keepWav.Enabled = false;

                this.Invoke(new MethodInvoker(delegate() { filelenght = cb_lenght.Text; }));
                this.Invoke(new MethodInvoker(delegate() { filepathWav = cb_path_wav.Text; }));
                this.Invoke(new MethodInvoker(delegate() { filepathMp3 = cb_path_mp3.Text; }));
                this.Invoke(new MethodInvoker(delegate() { progressTotal = progressBar1.Maximum; }));
                this.Invoke(new MethodInvoker(delegate() { progress = progressBar1.Value; }));
                this.Invoke(new MethodInvoker(delegate() { keepMp3 = cb_keepMp3.Checked; }));
                this.Invoke(new MethodInvoker(delegate() { keepWav = cb_keepWav.Checked; }));


                if (r_mp3.Checked)
                {
                    filetype = "mp3";
                }
                else
                {
                    filetype = "wav";
                }
                if (!cb_uploadFtp.Checked) {
                    filetype = null;
                }
                inzinierius.RunWorkerAsync();
        }

        public void btn_stop_Click(object sender, EventArgs e)
        {
            inzinierius.CancelAsync();
        }

        DateTime roundup(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }

        private void inzinierius_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                Recorder kasete = new Recorder(deviceId, filepathWav, filepathMp3, filetype, keepMp3, keepWav);
                DateTime now = DateTime.Now;
                int fileLenghtrequested = Convert.ToInt32(filelenght);
                DateTime endofSpan = roundup(now, TimeSpan.FromMinutes(fileLenghtrequested));
                TimeSpan span = endofSpan.Subtract(now);
                int sleeptime = 1000 * Convert.ToInt32(span.TotalSeconds) + 1000;
                progressTotal = Convert.ToInt32(sleeptime);
                int i = 0;

                while (i < Convert.ToInt32(sleeptime))
                {   
                    i++;
                    progress = i;
                    Thread.Sleep(1);
                    if (this.inzinierius.CancellationPending) { i = sleeptime; }
                }

                kasete.disableRecorder();
            } while (!this.inzinierius.CancellationPending);

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
            r_mp3.Enabled = true;
            r_wav.Enabled = true;
            cb_uploadFtp.Enabled = true;
            cb_keepMp3.Enabled = true;
            cb_keepWav.Enabled = true;
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            config.IniWriteValue("ftp", "host", tb_hostname.Text);
            config.IniWriteValue("ftp", "targetDir", tb_directory.Text);
            config.IniWriteValue("ftp", "user", tb_username.Text);
            config.IniWriteValue("ftp", "pass", tb_password.Text);          
            config.IniWriteValue("file", "pathWav", cb_path_wav.Text);
            config.IniWriteValue("file", "pathMp3", cb_path_mp3.Text);
            config.IniWriteValue("file", "span", cb_lenght.Text);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Maximum = progressTotal;
            progressBar1.Value = progress;
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