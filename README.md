# Audio Logger v 1.01
AudioLogger for rotational audio capturing.

1. Continuously record audio files of defined size (15 min, 30 min, 60 min, oth.);
2. First file is recorded to first alignment point;
3. All following files will start at *:00/*:15/*:30/*:45 if span = 15 min.;
3. Audio written to .wav and, once completed, converted to .mp3;
4. .mp3 files will be uploaded to defined FTP server;
5. Settings saved to config.ini file;
