using NAudio.Wave;

namespace AudioLogger.Services
{
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
}