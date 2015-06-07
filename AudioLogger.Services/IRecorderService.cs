namespace AudioLogger.Services
{
    public interface IRecorderService
    {
        void StartRecording(int device, string pathWav);
        void StopRecording();
    }
}