using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace AudioLogger.Services
{
    public class WaveInDeviceFactory
    {
        public static IWaveIn GetWaveInDevice(Device device)
        {

			if(device.isLoopback) {
				return new WasapiLoopbackCapture();
			}

			MMDeviceEnumerator devices = new MMDeviceEnumerator();
			MMDevice inputDevice = devices.GetDevice(device.DeviceId);

			WaveFormat waveFormat = new WaveFormat(48000, 16, 2);
			// Make sure device AudioClient supports setting WaveFormat
			if(inputDevice.AudioClient.IsFormatSupported(AudioClientShareMode.Shared, waveFormat)) {
				return new WasapiCapture(inputDevice)
				{
					WaveFormat = waveFormat
				};
			} else {
				return new WasapiCapture(inputDevice);
			}

		}
    }
}