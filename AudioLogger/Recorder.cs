using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Ftp;
using NAudio.Lame;
using NAudio.Wave;

namespace AudioLogger
{
    public class Recorder
    {
        public string FilenameMp3;
        public string Fullpathmp3;
        public string Fullpathwav;
        public WaveFileWriter WaveFile;
        public WaveInEvent WaveSource;

        public Recorder(int deviceId, string pathWav, string pathMp3)
        {
            enableRecorder(deviceId, pathWav, pathMp3);
        }

        public void enableRecorder(int device, string pathWav, string pathMp3)
        {
            WaveSource = new WaveInEvent();
            WaveSource.DeviceNumber = device;
            WaveSource.WaveFormat = new WaveFormat(44100, 16, 2);
            WaveSource.DataAvailable += waveSource_DataAvailable;
            WaveSource.RecordingStopped += waveSource_RecordingStopped;
            var now = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            Fullpathwav = pathWav + @"\" + now + ".wav";
            Fullpathmp3 = pathMp3 + @"\" + now + ".mp3";
            FilenameMp3 = now + ".mp3";
            WaveFile = new WaveFileWriter(Fullpathwav, WaveSource.WaveFormat);
            try
            {
                WaveSource.StartRecording();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void disableRecorder()
        {
            WaveSource.StopRecording();
            var converter = new Thread(Wav2Mp3);
            converter.Start();
        }

        private void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (WaveFile != null)
            {
                WaveFile.Write(e.Buffer, 0, e.BytesRecorded);
                WaveFile.Flush();
            }
        }

        private void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            WaveSource.Dispose();
            WaveSource = null;
            WaveFile.Dispose();
            WaveFile = null;
        }

        public void Wav2Mp3()
        {
            Thread.Sleep(1000);
            var wavefilesource = new MemoryStream(File.ReadAllBytes(Fullpathwav));
            wavefilesource.Seek(0, SeekOrigin.Begin);
            using (var retMs = new MemoryStream())
            using (var rdr = new WaveFileReader(wavefilesource))
            using (var wtr = new LameMP3FileWriter(Fullpathmp3, rdr.WaveFormat, LAMEPreset.VBR_90))
            {
                rdr.CopyTo(wtr);
            }
            var serveris = new FtpHandler();
            serveris.Upload(Fullpathmp3, FilenameMp3);
        }
    }
}