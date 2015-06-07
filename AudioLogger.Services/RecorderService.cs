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

        public void StartRecording(int device, string pathWav)
        {
            _waveSource = new WaveInEvent
            {
                DeviceNumber = device,
                WaveFormat = new WaveFormat(44100, 16, 2)
            };
            _waveSource.DataAvailable += waveSource_DataAvailable;
            _waveSource.RecordingStopped += waveSource_RecordingStopped;
            _waveFile = new WaveFileWriter(pathWav, _waveSource.WaveFormat);
            try
            {
                _waveSource.StartRecording();
            }
            catch (Exception e)
            {
                Logger.Warn(e.Message);
            }
        }

        public void StopRecording()
        {
            _waveSource.StopRecording();
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