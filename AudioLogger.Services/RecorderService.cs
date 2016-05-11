using System;
using log4net;
using NAudio.Wave;

namespace AudioLogger.Services
{
    public class Device
    {
        public Device(WaveInCapabilities capabilities, int deviceId, string productName = null)
        {
            Capabilities = capabilities;
            DeviceId = deviceId;
            ProductName = productName ?? Capabilities.ProductName;
        }

        public Device() {}

        public WaveInCapabilities Capabilities { get; }
        public string ProductName { get; set; }
        public int DeviceId { get; set; }
    }

    public class WaveInDeviceFactory
    {
        public static IWaveIn GetWaveInDevice(Device device)
        {
            switch (device.DeviceId)
            {
                case int.MaxValue:
                    return new  WasapiLoopbackCapture();
                default:
                    return new WaveInEvent
                    {
                        DeviceNumber = (int)device.DeviceId,
                        WaveFormat = new WaveFormat(48000, 16, 2)
                    };
            }
        }
    }
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

        public void WaveFile(string filename)
        {
            if (_waveSource == null) throw new ArgumentException("wave source not set up");
            lock (_lock)
            {
                _waveFile?.Dispose();
                _waveFile = new WaveFileWriter(filename, _waveSource.WaveFormat);
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