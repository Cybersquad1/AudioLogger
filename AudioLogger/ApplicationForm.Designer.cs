
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
            this.cb_temp_path = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.peak_R = new System.Windows.Forms.ProgressBar();
            this.peak_L = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
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
            this.tab1 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.tab2 = new System.Windows.Forms.TabPage();
            this.tab3 = new System.Windows.Forms.TabPage();
            this.bt_browseWinDir = new System.Windows.Forms.Button();
            this.l_uploadDir = new System.Windows.Forms.Label();
            this.dx_browseWinDir = new System.Windows.Forms.FolderBrowserDialog();
            this.bt_exit = new System.Windows.Forms.Button();
            this.bt_minimyze = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txt_version = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.settings.SuspendLayout();
            this.tab1.SuspendLayout();
            this.tab2.SuspendLayout();
            this.tab3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_soundcard
            // 
            this.cb_soundcard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_soundcard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_soundcard.FormattingEnabled = true;
            this.cb_soundcard.Location = new System.Drawing.Point(-49, -47);
            this.cb_soundcard.Name = "cb_soundcard";
            this.cb_soundcard.Size = new System.Drawing.Size(262, 21);
            this.cb_soundcard.TabIndex = 5;
            this.cb_soundcard.TabStop = false;
            this.cb_soundcard.SelectedValueChanged += new System.EventHandler(this.set_device);
            // 
            // btn_stop
            // 
            this.btn_stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_stop.ForeColor = System.Drawing.Color.Red;
            this.btn_stop.Location = new System.Drawing.Point(502, 119);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(39, 40);
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
            this.btn_start.Location = new System.Drawing.Point(447, 119);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(49, 40);
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
            this.cb_lenght.Location = new System.Drawing.Point(100, 38);
            this.cb_lenght.Name = "cb_lenght";
            this.cb_lenght.Size = new System.Drawing.Size(315, 21);
            this.cb_lenght.TabIndex = 12;
            // 
            // cb_temp_path
            // 
            this.cb_temp_path.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_temp_path.FormattingEnabled = true;
            this.cb_temp_path.Items.AddRange(new object[] {
            "E:\\_AudioLogger",
            "C:",
            "D:"});
            this.cb_temp_path.Location = new System.Drawing.Point(100, 11);
            this.cb_temp_path.Name = "cb_temp_path";
            this.cb_temp_path.Size = new System.Drawing.Size(315, 21);
            this.cb_temp_path.TabIndex = 13;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.peak_R);
            this.groupBox1.Controls.Add(this.cb_soundcard);
            this.groupBox1.Controls.Add(this.peak_L);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 49);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Audio Source";
            // 
            // peak_R
            // 
            this.peak_R.BackColor = System.Drawing.Color.WhiteSmoke;
            this.peak_R.ForeColor = System.Drawing.Color.DarkGreen;
            this.peak_R.Location = new System.Drawing.Point(274, 30);
            this.peak_R.Name = "peak_R";
            this.peak_R.Size = new System.Drawing.Size(148, 10);
            this.peak_R.Step = 5;
            this.peak_R.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.peak_R.TabIndex = 33;
            this.peak_R.Value = 2;
            // 
            // peak_L
            // 
            this.peak_L.BackColor = System.Drawing.Color.WhiteSmoke;
            this.peak_L.ForeColor = System.Drawing.Color.DarkGreen;
            this.peak_L.Location = new System.Drawing.Point(274, 18);
            this.peak_L.Name = "peak_L";
            this.peak_L.Size = new System.Drawing.Size(148, 10);
            this.peak_L.Step = 5;
            this.peak_L.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.peak_L.TabIndex = 32;
            this.peak_L.Value = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Time span (min):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Temp path:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 225);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(428, 21);
            this.progressBar1.Step = 5;
            this.progressBar1.TabIndex = 21;
            // 
            // tb_password
            // 
            this.tb_password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_password.Location = new System.Drawing.Point(66, 84);
            this.tb_password.MaxLength = 20;
            this.tb_password.Name = "tb_password";
            this.tb_password.PasswordChar = '*';
            this.tb_password.Size = new System.Drawing.Size(349, 20);
            this.tb_password.TabIndex = 7;
            // 
            // tb_username
            // 
            this.tb_username.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_username.Location = new System.Drawing.Point(66, 58);
            this.tb_username.MaxLength = 20;
            this.tb_username.Name = "tb_username";
            this.tb_username.Size = new System.Drawing.Size(349, 20);
            this.tb_username.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Password:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Username:";
            // 
            // tb_directory
            // 
            this.tb_directory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_directory.Location = new System.Drawing.Point(66, 32);
            this.tb_directory.MaxLength = 20;
            this.tb_directory.Name = "tb_directory";
            this.tb_directory.Size = new System.Drawing.Size(349, 20);
            this.tb_directory.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Directory:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Hostname:";
            // 
            // tb_hostname
            // 
            this.tb_hostname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_hostname.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tb_hostname.Location = new System.Drawing.Point(66, 6);
            this.tb_hostname.MaxLength = 100;
            this.tb_hostname.Name = "tb_hostname";
            this.tb_hostname.Size = new System.Drawing.Size(349, 20);
            this.tb_hostname.TabIndex = 0;
            // 
            // bt_Save
            // 
            this.bt_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_Save.Location = new System.Drawing.Point(447, 165);
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
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tb_fileUploadDir
            // 
            this.tb_fileUploadDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_fileUploadDir.ForeColor = System.Drawing.SystemColors.MenuText;
            this.tb_fileUploadDir.Location = new System.Drawing.Point(97, 6);
            this.tb_fileUploadDir.Name = "tb_fileUploadDir";
            this.tb_fileUploadDir.Size = new System.Drawing.Size(318, 20);
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
            "Windows directory"});
            this.cb_uploadType.Location = new System.Drawing.Point(100, 65);
            this.cb_uploadType.Name = "cb_uploadType";
            this.cb_uploadType.Size = new System.Drawing.Size(315, 21);
            this.cb_uploadType.TabIndex = 27;
            // 
            // settings
            // 
            this.settings.Controls.Add(this.tab1);
            this.settings.Controls.Add(this.tab2);
            this.settings.Controls.Add(this.tab3);
            this.settings.Location = new System.Drawing.Point(12, 75);
            this.settings.Name = "settings";
            this.settings.SelectedIndex = 0;
            this.settings.Size = new System.Drawing.Size(429, 146);
            this.settings.TabIndex = 29;
            // 
            // tab1
            // 
            this.tab1.BackColor = System.Drawing.Color.Gainsboro;
            this.tab1.Controls.Add(this.cb_temp_path);
            this.tab1.Controls.Add(this.label8);
            this.tab1.Controls.Add(this.label1);
            this.tab1.Controls.Add(this.cb_uploadType);
            this.tab1.Controls.Add(this.label3);
            this.tab1.Controls.Add(this.cb_lenght);
            this.tab1.Location = new System.Drawing.Point(4, 22);
            this.tab1.Name = "tab1";
            this.tab1.Padding = new System.Windows.Forms.Padding(3);
            this.tab1.Size = new System.Drawing.Size(421, 120);
            this.tab1.TabIndex = 2;
            this.tab1.Text = "General";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Upload target:";
            // 
            // tab2
            // 
            this.tab2.BackColor = System.Drawing.Color.Gainsboro;
            this.tab2.Controls.Add(this.tb_password);
            this.tab2.Controls.Add(this.tb_username);
            this.tab2.Controls.Add(this.tb_hostname);
            this.tab2.Controls.Add(this.label7);
            this.tab2.Controls.Add(this.label4);
            this.tab2.Controls.Add(this.label6);
            this.tab2.Controls.Add(this.label5);
            this.tab2.Controls.Add(this.tb_directory);
            this.tab2.Location = new System.Drawing.Point(4, 22);
            this.tab2.Name = "tab2";
            this.tab2.Padding = new System.Windows.Forms.Padding(3);
            this.tab2.Size = new System.Drawing.Size(421, 120);
            this.tab2.TabIndex = 0;
            this.tab2.Text = "FTP";
            // 
            // tab3
            // 
            this.tab3.BackColor = System.Drawing.Color.Gainsboro;
            this.tab3.Controls.Add(this.bt_browseWinDir);
            this.tab3.Controls.Add(this.l_uploadDir);
            this.tab3.Controls.Add(this.tb_fileUploadDir);
            this.tab3.Location = new System.Drawing.Point(4, 22);
            this.tab3.Name = "tab3";
            this.tab3.Padding = new System.Windows.Forms.Padding(3);
            this.tab3.Size = new System.Drawing.Size(421, 120);
            this.tab3.TabIndex = 1;
            this.tab3.Text = "WinDir";
            // 
            // bt_browseWinDir
            // 
            this.bt_browseWinDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bt_browseWinDir.Location = new System.Drawing.Point(340, 32);
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
            this.l_uploadDir.Size = new System.Drawing.Size(87, 13);
            this.l_uploadDir.TabIndex = 1;
            this.l_uploadDir.Text = "Upload directory:";
            // 
            // bt_exit
            // 
            this.bt_exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_exit.Location = new System.Drawing.Point(447, 223);
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
            this.bt_minimyze.Location = new System.Drawing.Point(447, 194);
            this.bt_minimyze.Name = "bt_minimyze";
            this.bt_minimyze.Size = new System.Drawing.Size(94, 23);
            this.bt_minimyze.TabIndex = 31;
            this.bt_minimyze.Text = "Minimyze";
            this.bt_minimyze.UseVisualStyleBackColor = true;
            this.bt_minimyze.Click += new System.EventHandler(this.bt_minimyze_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "AudioLogger";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(453, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(88, 73);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 32;
            this.pictureBox1.TabStop = false;
            // 
            // txt_version
            // 
            this.txt_version.AutoSize = true;
            this.txt_version.BackColor = System.Drawing.Color.Transparent;
            this.txt_version.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txt_version.Location = new System.Drawing.Point(450, 88);
            this.txt_version.Name = "txt_version";
            this.txt_version.Size = new System.Drawing.Size(91, 13);
            this.txt_version.TabIndex = 33;
            this.txt_version.Text = "AudioLogger v1.2";
            // 
            // ApplicationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(553, 257);
            this.Controls.Add(this.txt_version);
            this.Controls.Add(this.pictureBox1);
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
            this.Opacity = 0.96D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AudioLogger v1.2";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.settings.ResumeLayout(false);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.tab2.ResumeLayout(false);
            this.tab2.PerformLayout();
            this.tab3.ResumeLayout(false);
            this.tab3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.Button btn_start;
        private System.ComponentModel.BackgroundWorker inzinierius;
        public System.Windows.Forms.ComboBox cb_soundcard;
        public System.Windows.Forms.ComboBox cb_lenght;
        private System.Windows.Forms.ComboBox cb_temp_path;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
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
        private System.Windows.Forms.TextBox tb_fileUploadDir;
        public System.Windows.Forms.ComboBox cb_uploadType;
        private System.Windows.Forms.TabControl settings;
        private System.Windows.Forms.TabPage tab2;
        private System.Windows.Forms.TabPage tab3;
        private System.Windows.Forms.Button bt_browseWinDir;
        private System.Windows.Forms.Label l_uploadDir;
        private System.Windows.Forms.FolderBrowserDialog dx_browseWinDir;
        private System.Windows.Forms.TabPage tab1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button bt_exit;
        private System.Windows.Forms.Button bt_minimyze;
        private System.Windows.Forms.ProgressBar peak_L;
        private System.Windows.Forms.ProgressBar peak_R;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label txt_version;
    }
}

