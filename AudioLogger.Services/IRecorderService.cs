namespace AudioLogger.Services
{
    public interface IRecorderService
    {
        string FilenameMp3 { get; set; }
        string Fullpathmp3 { get; set; }
        string Fullpathwav { get; set; }
        void StartRecording(int device, string pathWav, string pathMp3);
        void StopRecording();
    }
}