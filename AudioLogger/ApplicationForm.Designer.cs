namespace AudioLogger.Application
{
    partial class ApplicationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationForm));
            this.cb_soundcard = new System.Windows.Forms.ComboBox();
            this.btn_stop = new System.Windows.Forms.Button();
            this.btn_start = new System.Windows.Forms.Button();
            this.inzinierius = new System.ComponentModel.BackgroundWorker();
            this.cb_lenght = new System.Windows.Forms.ComboBox();
            this.cb_path_wav = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_keepMp3 = new System.Windows.Forms.CheckBox();
            this.cb_keepWav = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_path_mp3 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.tb_username = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_directory = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_hostname = new System.Windows.Forms.TextBox();
            this.bt_Save = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tb_fileUploadDir = new System.Windows.Forms.TextBox();
            this.cb_uploadType = new System.Windows.Forms.ComboBox();
            this.settings = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.bt_browseWinDir = new System.Windows.Forms.Button();
            this.l_uploadDir = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cb_uploadFormat = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dx_browseWinDir = new System.Windows.Forms.FolderBrowserDialog();
            this.bt_exit = new System.Windows.Forms.Button();
            this.bt_minimyze = new System.Windows.Forms.Button();
            this.peak_L = new System.Windows.Forms.ProgressBar();
            this.peak_R = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.settings.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_soundcard
            // 
            this.cb_soundcard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_soundcard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_soundcard.FormattingEnabled = true;
            this.cb_soundcard.Location = new System.Drawing.Point(6, 19);
            this.cb_soundcard.Name = "cb_soundcard";
            this.cb_soundcard.Size = new System.Drawing.Size(327, 21);
            this.cb_soundcard.TabIndex = 5;
            this.cb_soundcard.TabStop = false;
            this.cb_soundcard.SelectedValueChanged += new System.EventHandler(this.set_device);
            // 
            // btn_stop
            // 
            this.btn_stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_stop.ForeColor = System.Drawing.Color.Red;
            this.btn_stop.Location = new System.Drawing.Point(447, 46);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(94, 23);
            this.btn_stop.TabIndex = 8;
            this.btn_stop.TabStop = false;
            this.btn_stop.Text = "Stop";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // btn_start
            // 
            this.btn_start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_start.ForeColor = System.Drawing.Color.Green;
            this.btn_start.Location = new System.Drawing.Point(447, 17);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(94, 23);
            this.btn_start.TabIndex = 9;
            this.btn_start.TabStop = false;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // inzinierius
            // 
            this.inzinierius.WorkerReportsProgress = true;
            this.inzinierius.WorkerSupportsCancellation = true;
            this.inzinierius.DoWork += new System.ComponentModel.DoWorkEventHandler(this.inzinierius_DoWork);
            this.inzinierius.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.inzinierius_RunWorkCompleted);
            // 
            // cb_lenght
            // 
            this.cb_lenght.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_lenght.FormattingEnabled = true;
            this.cb_lenght.Items.AddRange(new object[] {
            "15",
            "30",
            "60"});
            this.cb_lenght.Location = new System.Drawing.Point(179, 63);
            this.cb_lenght.Name = "cb_lenght";
            this.cb_lenght.Size = new System.Drawing.Size(71, 21);
            this.cb_lenght.TabIndex = 12;
            // 
            // cb_path_wav
            // 
            this.cb_path_wav.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_path_wav.FormattingEnabled = true;
            this.cb_path_wav.Items.AddRange(new object[] {
            "E:\\_AudioLogger",
            "C:",
            "D:"});
            this.cb_path_wav.Location = new System.Drawing.Point(100, 11);
            this.cb_path_wav.Name = "cb_path_wav";
            this.cb_path_wav.Size = new System.Drawing.Size(150, 21);
            this.cb_path_wav.TabIndex = 13;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.peak_R);
            this.groupBox1.Controls.Add(this.cb_soundcard);
            this.groupBox1.Controls.Add(this.peak_L);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 57);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Audio Source";
            // 
            // cb_keepMp3
            // 
            this.cb_keepMp3.AutoSize = true;
            this.cb_keepMp3.Location = new System.Drawing.Point(256, 40);
            this.cb_keepMp3.Name = "cb_keepMp3";
            this.cb_keepMp3.Size = new System.Drawing.Size(118, 17);
            this.cb_keepMp3.TabIndex = 23;
            this.cb_keepMp3.Text = "Keep local .mp3 file";
            this.cb_keepMp3.UseVisualStyleBackColor = true;
            // 
            // cb_keepWav
            // 
            this.cb_keepWav.AutoSize = true;
            this.cb_keepWav.Location = new System.Drawing.Point(256, 14);
            this.cb_keepWav.Name = "cb_keepWav";
            this.cb_keepWav.Size = new System.Drawing.Size(118, 17);
            this.cb_keepWav.TabIndex = 22;
            this.cb_keepWav.Text = "Keep local .wav file";
            this.cb_keepWav.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Time span (min):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = ".mp3 path:";
            // 
            // cb_path_mp3
            // 
            this.cb_path_mp3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_path_mp3.FormattingEnabled = true;
            this.cb_path_mp3.Items.AddRange(new object[] {
            "E:\\_AudioLogger",
            "C:",
            "D:"});
            this.cb_path_mp3.Location = new System.Drawing.Point(100, 38);
            this.cb_path_mp3.Name = "cb_path_mp3";
            this.cb_path_mp3.Size = new System.Drawing.Size(150, 21);
            this.cb_path_mp3.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = ".wav path:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 225);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(528, 21);
            this.progressBar1.Step = 5;
            this.progressBar1.TabIndex = 21;
            // 
            // tb_password
            // 
            this.tb_password.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_password.Location = new System.Drawing.Point(66, 63);
            this.tb_password.MaxLength = 20;
            this.tb_password.Name = "tb_password";
            this.tb_password.PasswordChar = '*';
            this.tb_password.Size = new System.Drawing.Size(313, 13);
            this.tb_password.TabIndex = 7;
            // 
            // tb_username
            // 
            this.tb_username.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_username.Location = new System.Drawing.Point(66, 44);
            this.tb_username.MaxLength = 20;
            this.tb_username.Name = "tb_username";
            this.tb_username.Size = new System.Drawing.Size(313, 13);
            this.tb_username.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Password:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Username:";
            // 
            // tb_directory
            // 
            this.tb_directory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_directory.Location = new System.Drawing.Point(66, 25);
            this.tb_directory.MaxLength = 20;
            this.tb_directory.Name = "tb_directory";
            this.tb_directory.Size = new System.Drawing.Size(313, 13);
            this.tb_directory.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Directory:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Hostname:";
            // 
            // tb_hostname
            // 
            this.tb_hostname.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_hostname.Location = new System.Drawing.Point(66, 6);
            this.tb_hostname.MaxLength = 100;
            this.tb_hostname.Name = "tb_hostname";
            this.tb_hostname.Size = new System.Drawing.Size(313, 13);
            this.tb_hostname.TabIndex = 0;
            // 
            // bt_Save
            // 
            this.bt_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_Save.Location = new System.Drawing.Point(447, 145);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(94, 23);
            this.bt_Save.TabIndex = 18;
            this.bt_Save.TabStop = false;
            this.bt_Save.Text = "Save";
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tb_fileUploadDir
            // 
            this.tb_fileUploadDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_fileUploadDir.ForeColor = System.Drawing.SystemColors.MenuText;
            this.tb_fileUploadDir.Location = new System.Drawing.Point(97, 6);
            this.tb_fileUploadDir.Name = "tb_fileUploadDir";
            this.tb_fileUploadDir.Size = new System.Drawing.Size(191, 20);
            this.tb_fileUploadDir.TabIndex = 0;
            this.tb_fileUploadDir.Text = "C:\\";
            // 
            // cb_uploadType
            // 
            this.cb_uploadType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_uploadType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_uploadType.FormattingEnabled = true;
            this.cb_uploadType.Items.AddRange(new object[] {
            "FTP",
            "Directory"});
            this.cb_uploadType.Location = new System.Drawing.Point(105, 90);
            this.cb_uploadType.Name = "cb_uploadType";
            this.cb_uploadType.Size = new System.Drawing.Size(145, 21);
            this.cb_uploadType.TabIndex = 27;
            // 
            // settings
            // 
            this.settings.Controls.Add(this.tabPage1);
            this.settings.Controls.Add(this.tabPage2);
            this.settings.Controls.Add(this.tabPage3);
            this.settings.Location = new System.Drawing.Point(12, 75);
            this.settings.Name = "settings";
            this.settings.SelectedIndex = 0;
            this.settings.Size = new System.Drawing.Size(429, 146);
            this.settings.TabIndex = 29;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage1.Controls.Add(this.tb_password);
            this.tabPage1.Controls.Add(this.tb_username);
            this.tabPage1.Controls.Add(this.tb_hostname);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.tb_directory);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(421, 120);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "FTP settings";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage2.Controls.Add(this.bt_browseWinDir);
            this.tabPage2.Controls.Add(this.l_uploadDir);
            this.tabPage2.Controls.Add(this.tb_fileUploadDir);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(421, 120);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "WinDir settings";
            // 
            // bt_browseWinDir
            // 
            this.bt_browseWinDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bt_browseWinDir.Location = new System.Drawing.Point(294, 5);
            this.bt_browseWinDir.Name = "bt_browseWinDir";
            this.bt_browseWinDir.Size = new System.Drawing.Size(75, 21);
            this.bt_browseWinDir.TabIndex = 2;
            this.bt_browseWinDir.Text = "Browse";
            this.bt_browseWinDir.UseVisualStyleBackColor = true;
            this.bt_browseWinDir.Click += new System.EventHandler(this.bt_browseWinDir_Click);
            // 
            // l_uploadDir
            // 
            this.l_uploadDir.AutoSize = true;
            this.l_uploadDir.Location = new System.Drawing.Point(7, 9);
            this.l_uploadDir.Name = "l_uploadDir";
            this.l_uploadDir.Size = new System.Drawing.Size(84, 13);
            this.l_uploadDir.TabIndex = 1;
            this.l_uploadDir.Text = "Upload directory";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage3.Controls.Add(this.cb_uploadFormat);
            this.tabPage3.Controls.Add(this.cb_path_wav);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.cb_uploadType);
            this.tabPage3.Controls.Add(this.cb_path_mp3);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.cb_lenght);
            this.tabPage3.Controls.Add(this.cb_keepMp3);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.cb_keepWav);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(421, 120);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "File settings";
            // 
            // cb_uploadFormat
            // 
            this.cb_uploadFormat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_uploadFormat.FormattingEnabled = true;
            this.cb_uploadFormat.Items.AddRange(new object[] {
            "wav",
            "mp3"});
            this.cb_uploadFormat.Location = new System.Drawing.Point(256, 90);
            this.cb_uploadFormat.Name = "cb_uploadFormat";
            this.cb_uploadFormat.Size = new System.Drawing.Size(117, 21);
            this.cb_uploadFormat.TabIndex = 29;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Upload target:";
            // 
            // bt_exit
            // 
            this.bt_exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_exit.Location = new System.Drawing.Point(447, 198);
            this.bt_exit.Name = "bt_exit";
            this.bt_exit.Size = new System.Drawing.Size(94, 23);
            this.bt_exit.TabIndex = 30;
            this.bt_exit.Text = "Exit";
            this.bt_exit.UseVisualStyleBackColor = true;
            this.bt_exit.Click += new System.EventHandler(this.bt_exit_Click);
            // 
            // bt_minimyze
            // 
            this.bt_minimyze.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_minimyze.Location = new System.Drawing.Point(447, 172);
            this.bt_minimyze.Name = "bt_minimyze";
            this.bt_minimyze.Size = new System.Drawing.Size(94, 23);
            this.bt_minimyze.TabIndex = 31;
            this.bt_minimyze.Text = "Minimyze";
            this.bt_minimyze.UseVisualStyleBackColor = true;
            // 
            // peak_L
            // 
            this.peak_L.BackColor = System.Drawing.Color.WhiteSmoke;
            this.peak_L.ForeColor = System.Drawing.Color.DarkGreen;
            this.peak_L.Location = new System.Drawing.Point(338, 18);
            this.peak_L.Name = "peak_L";
            this.peak_L.Size = new System.Drawing.Size(84, 10);
            this.peak_L.Step = 5;
            this.peak_L.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.peak_L.TabIndex = 32;
            this.peak_L.Value = 2;
            // 
            // peak_R
            // 
            this.peak_R.BackColor = System.Drawing.Color.WhiteSmoke;
            this.peak_R.ForeColor = System.Drawing.Color.DarkGreen;
            this.peak_R.Location = new System.Drawing.Point(338, 30);
            this.peak_R.Name = "peak_R";
            this.peak_R.Size = new System.Drawing.Size(84, 10);
            this.peak_R.Step = 5;
            this.peak_R.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.peak_R.TabIndex = 33;
            this.peak_R.Value = 2;
            // 
            // ApplicationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(553, 253);
            this.Controls.Add(this.bt_minimyze);
            this.Controls.Add(this.bt_exit);
            this.Controls.Add(this.settings);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.btn_stop);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ApplicationForm";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AudioLogger v1.2";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.settings.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.Button btn_start;
        private System.ComponentModel.BackgroundWorker inzinierius;
        public System.Windows.Forms.ComboBox cb_soundcard;
        public System.Windows.Forms.ComboBox cb_lenght;
        private System.Windows.Forms.ComboBox cb_path_wav;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cb_path_mp3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_password;
        private System.Windows.Forms.TextBox tb_username;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_directory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_hostname;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox cb_keepMp3;
        private System.Windows.Forms.CheckBox cb_keepWav;
        private System.Windows.Forms.TextBox tb_fileUploadDir;
        public System.Windows.Forms.ComboBox cb_uploadType;
        private System.Windows.Forms.TabControl settings;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button bt_browseWinDir;
        private System.Windows.Forms.Label l_uploadDir;
        private System.Windows.Forms.FolderBrowserDialog dx_browseWinDir;
        private System.Windows.Forms.TabPage tabPage3;
        public System.Windows.Forms.ComboBox cb_uploadFormat;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button bt_exit;
        private System.Windows.Forms.Button bt_minimyze;
        private System.Windows.Forms.ProgressBar peak_L;
        private System.Windows.Forms.ProgressBar peak_R;
    }
}

