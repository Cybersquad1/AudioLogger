namespace AudioLogger.Services
{
    public interface IRecorderService
    {
        void Setup(Device device);
        void WaveFile(string filename);
        void StartRecording();
        void StopRecording();
    }
}