namespace AudioLogger.Services
{
    public interface IConverterService
    {
        void AsyncConvert(string fullPathWav, string fillPathMp3);
        void Wait();
    }
}