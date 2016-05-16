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
}