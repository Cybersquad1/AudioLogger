AudioLogger for rotational audio capturing.

1. Continuously record audio  to files based on provided span time (15 min, 30 min, 60 min, oth.);
2. First file will be to first alignment point and all others will start at XX:00, XX:15, XX:30, XX:45 etc. if span set to 15 min.;
3. Files will be saved to .wav and converted to .mp3;
4. .mp3 files will be uploaded to defined server;
5. Settings saved to config.ini file;
