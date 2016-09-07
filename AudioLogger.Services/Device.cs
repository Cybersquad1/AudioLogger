using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace AudioLogger.Services
{
    public class Device
    {
        public Device(string deviceId, string productName, bool loopback)
        {
            DeviceId = deviceId;
            ProductName = productName;
			isLoopback = loopback;
		}

        public Device() {}

        public string ProductName { get; set; }
        public string DeviceId { get; set; }
		public bool isLoopback { get; }
    }
}