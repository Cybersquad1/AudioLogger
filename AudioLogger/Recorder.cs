﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using NAudio.Lame;
using System.IO;
using Ini;
using Ftp;

namespace AudioLogger
{
    public class Recorder
    {
       public WaveInEvent waveSource = null;
       public WaveFileWriter waveFile = null;
       public string fullpathwav = null;
       public string fullpathmp3 = null;
       public string filenameMp3 = null;
       public string filenameWav = null;
       public string uploadtype = null;

        public Recorder(int deviceId, string pathWav, string pathMp3, string filetype)
           {
               uploadtype = filetype;
                enableRecorder(deviceId, pathWav, pathMp3, filetype);
           }

        public void enableRecorder(int device, string pathWav, string pathMp3, string filetype)
        {
            waveSource = new WaveInEvent();
            waveSource.DeviceNumber = device;
            waveSource.WaveFormat = new WaveFormat(44100, 16, 2);
            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);
            string now = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            fullpathwav = pathWav + @"\" + now + ".wav";
            fullpathmp3 = pathMp3 + @"\" + now + ".mp3";
            filenameWav = now + ".wav";
            filenameMp3 = now + ".mp3";
            
            waveFile = new WaveFileWriter(fullpathwav, waveSource.WaveFormat);
            try
            {
                waveSource.StartRecording();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            } 
        }

        public void disableRecorder()
        {
            waveSource.StopRecording();
            Thread converter = new Thread(Wav2Mp3);
            converter.Start();
        }

        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
                    {
                        waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                        waveFile.Flush();
                    }
        }

        void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            waveSource.Dispose();
            waveSource = null;
            waveFile.Dispose();
            waveFile = null;
        }

        public void Wav2Mp3()
        {
            Thread.Sleep(1000);
            MemoryStream wavefilesource = new MemoryStream(File.ReadAllBytes(fullpathwav));
            wavefilesource.Seek(0, SeekOrigin.Begin);
            using (var retMs = new MemoryStream())
            using (var rdr = new WaveFileReader(wavefilesource))
            using (var wtr = new LameMP3FileWriter(fullpathmp3, rdr.WaveFormat, LAMEPreset.VBR_90))
            {
                rdr.CopyTo(wtr);
            }

           
            if (uploadtype == "wav")
            {
                FtpHandler serveris = new FtpHandler();
                serveris.Upload(fullpathwav, filenameWav);
            }
            else if (uploadtype == "mp3")
            {
                FtpHandler serveris = new FtpHandler();
                serveris.Upload(fullpathmp3, filenameMp3);
            }
            else
            {
                Debug.WriteLine("No FTP upload");
            }
            
        }
    }
}
