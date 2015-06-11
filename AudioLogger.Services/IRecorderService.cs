namespace AudioLogger.Services
{
    public interface IRecorderService
    {
        void Setup(int device);
        void WaveFile(string filename);
        void StartRecording();
        void StopRecording();
    }
}