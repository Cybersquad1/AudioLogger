using System;
using log4net;
using NAudio.Wave;

namespace AudioLogger.Services
{
    public class RecorderService : IRecorderService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (RecorderService));
        private readonly object _lock = new object();

        private WaveFileWriter _waveFile;
        private IWaveIn _waveSource;

        public void Setup(Device device)
        {
            _waveFile = null;
            _waveSource = WaveInDeviceFactory.GetWaveInDevice(device);
            _waveSource.DataAvailable += waveSource_DataAvailable;
            _waveSource.RecordingStopped += waveSource_RecordingStopped;
        }

        public void WaveFile(AudioLog audioLog)
        {
            if (_waveSource == null) throw new ArgumentException("wave source not set up");
            lock (_lock)
            {
                _waveFile?.Dispose();
                _waveFile = new WaveFileWriter(audioLog.GetWav(), _waveSource.WaveFormat);
            }
        }

        public void StartRecording()
        {
            try
            {
                _waveSource.StartRecording();
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }

        public void StopRecording()
        {
            _waveSource.StopRecording();
        }

        private void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            lock (_lock)
            {
                if (_waveFile != null)
                {
                    _waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                    _waveFile.Flush();
                }
            }
        }

        private void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            lock (_lock)
            {
                _waveSource.Dispose();
                _waveSource = null;
                _waveFile.Dispose();
                _waveFile = null;
            }
        }
    }
}