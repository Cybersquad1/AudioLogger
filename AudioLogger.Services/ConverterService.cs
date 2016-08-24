using System;
using System.Diagnostics;
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
        private string _fullpathmp3;
        private string _fullpathwav;
        private Thread _task;

        public void AsyncConvert(string fullPathWav, string fullPathMp3)
        {
            _fullpathmp3 = fullPathMp3;
            _fullpathwav = fullPathWav;
            _task = new Thread(Wav2Mp3);
            _task.Start();
        }

        public void Wait()
        {
            if (_task.IsAlive)
                _task.Join();
        }

        private void Wav2Mp3()
        {
            try
            {
                Thread.Sleep(1000);
                using (var waveFileStream = File.Open(_fullpathwav, FileMode.Open))
                using (var bufferedWaveFileStream = new BufferedStream(waveFileStream))
                using (var rdr = new WaveFileReader(bufferedWaveFileStream))
                {
                    using (var wtr = new LameMP3FileWriter(_fullpathmp3, rdr.WaveFormat, LAMEPreset.VBR_90))
                    {
                        rdr.CopyTo(wtr);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }
    }
}