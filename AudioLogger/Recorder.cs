using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using NAudio.Lame;
using System.IO;
using Ini;
using Ftp;

//just to change file for GIT commit.

namespace AudioLogger
{
    public class Recorder
    {
       public WaveInEvent waveSource = null;
       public WaveFileWriter waveFile = null;
       public string fullpathwav = null;
       public string fullpathmp3 = null;
       public string filenameMp3 = null;

        public Recorder(int deviceId, string pathWav, string pathMp3)
           {
                enableRecorder(deviceId, pathWav, pathMp3);
           }

        public void enableRecorder(int device, string pathWav, string pathMp3)
        {
            waveSource = new WaveInEvent();
            waveSource.DeviceNumber = device;
            waveSource.WaveFormat = new WaveFormat(44100, 16, 2);
            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);
            string now = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            fullpathwav = pathWav + @"\" + now + ".wav";
            fullpathmp3 = pathMp3 + @"\" + now + ".mp3";
            filenameMp3 = now + ".mp3";
            waveFile = new WaveFileWriter(fullpathwav, waveSource.WaveFormat);
            try
            {
                waveSource.StartRecording();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            } 
        }

        public void disableRecorder()
        {
            waveSource.StopRecording();
            Thread converter = new Thread(Wav2Mp3);
            converter.Start();
        }

        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
                    {
                        waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                        waveFile.Flush();
                    }
        }

        void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            waveSource.Dispose();
            waveSource = null;
            waveFile.Dispose();
            waveFile = null;
        }

        public void Wav2Mp3()
        {
            Thread.Sleep(1000);
            MemoryStream wavefilesource = new MemoryStream(File.ReadAllBytes(fullpathwav));
            wavefilesource.Seek(0, SeekOrigin.Begin);
            using (var retMs = new MemoryStream())
            using (var rdr = new WaveFileReader(wavefilesource))
            using (var wtr = new LameMP3FileWriter(fullpathmp3, rdr.WaveFormat, LAMEPreset.VBR_90))
            {
                rdr.CopyTo(wtr);
            }
            FtpHandler serveris = new FtpHandler();
            serveris.Upload(fullpathmp3, filenameMp3);
        }
    }
}
