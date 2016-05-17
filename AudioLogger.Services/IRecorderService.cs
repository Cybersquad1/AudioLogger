namespace AudioLogger.Services
{
    public interface IRecorderService
    {
        void Setup(Device device);
        void WaveFile(AudioLog audioLog);
        void StartRecording();
        void StopRecording();
    }
}