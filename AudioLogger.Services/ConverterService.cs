using System;
using System.IO;
using System.Threading;
using log4net;
using NAudio.Lame;
using NAudio.Wave;

namespace AudioLogger.Services
{
    public class ConverterService : IConverterService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (ConverterService));
        private static readonly object ThreadLock = new object();
        private string _fullpathmp3;
        private string _fullpathwav;
        private Thread _task;

        public ConverterService()
        {
            TaskFinished = true;
        }

        private bool TaskFinished { get; set; }

        public void AsyncConvert(string fullPathWav, string fillPathMp3)
        {
            lock (ThreadLock)
            {
                TaskFinished = false;
            }
            _fullpathmp3 = fillPathMp3;
            _fullpathwav = fullPathWav;
            _task = new Thread(Wav2Mp3);
        }

        public void Wait()
        {
            _task.Join();
        }

        private void Wav2Mp3()
        {
            try
            {
                Thread.Sleep(1000);
                var wavefilesource = new MemoryStream(File.ReadAllBytes(_fullpathwav));
                wavefilesource.Seek(0, SeekOrigin.Begin);
                using (var rdr = new WaveFileReader(wavefilesource))
                using (var wtr = new LameMP3FileWriter(_fullpathmp3, rdr.WaveFormat, LAMEPreset.ABR_128))
                {
                    rdr.CopyTo(wtr);
                }
                lock (ThreadLock)
                {
                    TaskFinished = true;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw e;
            }
        }
    }
}