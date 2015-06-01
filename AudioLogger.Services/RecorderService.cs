using System;
using log4net;
using NAudio.Wave;

namespace AudioLogger.Services
{
    public class RecorderService : IRecorderService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (RecorderService));
        private WaveFileWriter _waveFile;
        private WaveInEvent _waveSource;
        public string FilenameWav { get; set; }
        public string FilenameMp3 { get; set; }
        public string Fullpathmp3 { get; set; }
        public string Fullpathwav { get; set; }

        public void StartRecording(int device, string pathWav, string pathMp3)
        {
            _waveSource = new WaveInEvent();
            _waveSource.DeviceNumber = device;
            _waveSource.WaveFormat = new WaveFormat(44100, 16, 2);
            _waveSource.DataAvailable += waveSource_DataAvailable;
            _waveSource.RecordingStopped += waveSource_RecordingStopped;
            var now = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            Fullpathwav = pathWav + @"\" + now + ".wav";
            Fullpathmp3 = pathMp3 + @"\" + now + ".mp3";
            FilenameMp3 = now + ".mp3";
            FilenameWav = now + ".wav";
            _waveFile = new WaveFileWriter(Fullpathwav, _waveSource.WaveFormat);
            try
            {
                _waveSource.StartRecording();
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw e;
            }
        }

        public void StopRecording()
        {
            _waveSource.StopRecording();
            _waveSource = null;
        }

        private void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (_waveFile != null)
            {
                _waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                _waveFile.Flush();
            }
        }

        private void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            _waveSource.Dispose();
            _waveSource = null;
            _waveFile.Dispose();
            _waveFile = null;
        }
    }
}