# Audio Logger v 1.2
AudioLogger for rotational audio capturing.

1. Continuously record audio files of defined size (15 min, 30 min, 60 min, oth.);
2. First file is recorded until first alignment point;
3. All following files will start at AA:00, AA:15, AA:30, AA:45 (if span time set to 15 min.);
3. Audio is written to .wav and, once completed, converted to .mp3;
4. After converting process .mp3 files will be uploaded to defined FTP server or Windows directory;
5. All settings can be saved (to config.ini file);


Ignas, 2015-06-02
