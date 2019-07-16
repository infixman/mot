namespace MOT
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btn_PickPath = new System.Windows.Forms.Button();
            this.txt_LpmsPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_Log = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chk_StartAfterRun = new System.Windows.Forms.CheckBox();
            this.chk_PrintClock = new System.Windows.Forms.CheckBox();
            this.chk_PrintWaterMark = new System.Windows.Forms.CheckBox();
            this.chk_MotOldFile = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_PickPic2 = new System.Windows.Forms.Button();
            this.txt_PicPath2 = new System.Windows.Forms.TextBox();
            this.btn_Start = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_PickPic = new System.Windows.Forms.Button();
            this.txt_PicPath = new System.Windows.Forms.TextBox();
            this.notify_icon = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_PickPath
            // 
            this.btn_PickPath.Location = new System.Drawing.Point(625, 5);
            this.btn_PickPath.Name = "btn_PickPath";
            this.btn_PickPath.Size = new System.Drawing.Size(75, 23);
            this.btn_PickPath.TabIndex = 10;
            this.btn_PickPath.Text = "選擇";
            this.btn_PickPath.UseVisualStyleBackColor = true;
            this.btn_PickPath.Click += new System.EventHandler(this.btn_PickPath_Click);
            // 
            // txt_LpmsPath
            // 
            this.txt_LpmsPath.Location = new System.Drawing.Point(109, 7);
            this.txt_LpmsPath.Name = "txt_LpmsPath";
            this.txt_LpmsPath.Size = new System.Drawing.Size(510, 20);
            this.txt_LpmsPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "LPMs目錄";
            // 
            // txt_Log
            // 
            this.txt_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Log.Location = new System.Drawing.Point(0, 0);
            this.txt_Log.Name = "txt_Log";
            this.txt_Log.ReadOnly = true;
            this.txt_Log.Size = new System.Drawing.Size(1000, 607);
            this.txt_Log.TabIndex = 100;
            this.txt_Log.TabStop = false;
            this.txt_Log.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chk_StartAfterRun);
            this.splitContainer1.Panel1.Controls.Add(this.chk_PrintClock);
            this.splitContainer1.Panel1.Controls.Add(this.chk_PrintWaterMark);
            this.splitContainer1.Panel1.Controls.Add(this.chk_MotOldFile);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.btn_PickPic2);
            this.splitContainer1.Panel1.Controls.Add(this.txt_PicPath2);
            this.splitContainer1.Panel1.Controls.Add(this.btn_Start);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.btn_PickPic);
            this.splitContainer1.Panel1.Controls.Add(this.txt_PicPath);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.btn_PickPath);
            this.splitContainer1.Panel1.Controls.Add(this.txt_LpmsPath);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txt_Log);
            this.splitContainer1.Size = new System.Drawing.Size(1000, 695);
            this.splitContainer1.SplitterDistance = 84;
            this.splitContainer1.TabIndex = 4;
            // 
            // chk_StartAfterRun
            // 
            this.chk_StartAfterRun.AutoSize = true;
            this.chk_StartAfterRun.Location = new System.Drawing.Point(888, 8);
            this.chk_StartAfterRun.Name = "chk_StartAfterRun";
            this.chk_StartAfterRun.Size = new System.Drawing.Size(110, 17);
            this.chk_StartAfterRun.TabIndex = 90;
            this.chk_StartAfterRun.Text = "執行後自動開始";
            this.chk_StartAfterRun.UseVisualStyleBackColor = true;
            this.chk_StartAfterRun.CheckedChanged += new System.EventHandler(this.chk_StartAfterRun_CheckedChanged);
            // 
            // chk_PrintClock
            // 
            this.chk_PrintClock.AutoSize = true;
            this.chk_PrintClock.Location = new System.Drawing.Point(706, 60);
            this.chk_PrintClock.Name = "chk_PrintClock";
            this.chk_PrintClock.Size = new System.Drawing.Size(176, 17);
            this.chk_PrintClock.TabIndex = 80;
            this.chk_PrintClock.Text = "打上左下時鐘(模擬登出畫面)";
            this.chk_PrintClock.UseVisualStyleBackColor = true;
            this.chk_PrintClock.CheckedChanged += new System.EventHandler(this.chk_PrintClock_CheckedChanged);
            // 
            // chk_PrintWaterMark
            // 
            this.chk_PrintWaterMark.AutoSize = true;
            this.chk_PrintWaterMark.Location = new System.Drawing.Point(706, 34);
            this.chk_PrintWaterMark.Name = "chk_PrintWaterMark";
            this.chk_PrintWaterMark.Size = new System.Drawing.Size(110, 17);
            this.chk_PrintWaterMark.TabIndex = 70;
            this.chk_PrintWaterMark.Text = "打上右下浮水印";
            this.chk_PrintWaterMark.UseVisualStyleBackColor = true;
            this.chk_PrintWaterMark.CheckedChanged += new System.EventHandler(this.chk_PrintWaterMark_CheckedChanged);
            // 
            // chk_MotOldFile
            // 
            this.chk_MotOldFile.AutoSize = true;
            this.chk_MotOldFile.Location = new System.Drawing.Point(706, 9);
            this.chk_MotOldFile.Name = "chk_MotOldFile";
            this.chk_MotOldFile.Size = new System.Drawing.Size(98, 17);
            this.chk_MotOldFile.TabIndex = 60;
            this.chk_MotOldFile.Text = "母湯現有檔案";
            this.chk_MotOldFile.UseVisualStyleBackColor = true;
            this.chk_MotOldFile.CheckedChanged += new System.EventHandler(this.chk_MotOldFile_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "螢幕2圖片來源";
            // 
            // btn_PickPic2
            // 
            this.btn_PickPic2.Location = new System.Drawing.Point(625, 57);
            this.btn_PickPic2.Name = "btn_PickPic2";
            this.btn_PickPic2.Size = new System.Drawing.Size(75, 23);
            this.btn_PickPic2.TabIndex = 50;
            this.btn_PickPic2.Text = "選擇";
            this.btn_PickPic2.UseVisualStyleBackColor = true;
            this.btn_PickPic2.Click += new System.EventHandler(this.btn_PickPic2_Click);
            // 
            // txt_PicPath2
            // 
            this.txt_PicPath2.Location = new System.Drawing.Point(109, 59);
            this.txt_PicPath2.Name = "txt_PicPath2";
            this.txt_PicPath2.Size = new System.Drawing.Size(510, 20);
            this.txt_PicPath2.TabIndex = 40;
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(888, 57);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(100, 23);
            this.btn_Start.TabIndex = 100;
            this.btn_Start.Text = "開始";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "螢幕1圖片來源";
            // 
            // btn_PickPic
            // 
            this.btn_PickPic.Location = new System.Drawing.Point(625, 31);
            this.btn_PickPic.Name = "btn_PickPic";
            this.btn_PickPic.Size = new System.Drawing.Size(75, 23);
            this.btn_PickPic.TabIndex = 30;
            this.btn_PickPic.Text = "選擇";
            this.btn_PickPic.UseVisualStyleBackColor = true;
            this.btn_PickPic.Click += new System.EventHandler(this.btn_PickPic_Click);
            // 
            // txt_PicPath
            // 
            this.txt_PicPath.Location = new System.Drawing.Point(109, 33);
            this.txt_PicPath.Name = "txt_PicPath";
            this.txt_PicPath.Size = new System.Drawing.Size(510, 20);
            this.txt_PicPath.TabIndex = 20;
            // 
            // notify_icon
            // 
            this.notify_icon.Icon = ((System.Drawing.Icon)(resources.GetObject("notify_icon.Icon")));
            this.notify_icon.Text = "MOT";
            this.notify_icon.Visible = true;
            this.notify_icon.BalloonTipClicked += new System.EventHandler(this.notify_icon_BalloonTipClicked);
            this.notify_icon.DoubleClick += new System.EventHandler(this.notify_icon_DoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 695);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MOT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_PickPath;
        private System.Windows.Forms.TextBox txt_LpmsPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox txt_Log;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_PickPic;
        private System.Windows.Forms.TextBox txt_PicPath;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_PickPic2;
        private System.Windows.Forms.TextBox txt_PicPath2;
        private System.Windows.Forms.CheckBox chk_MotOldFile;
        private System.Windows.Forms.CheckBox chk_PrintClock;
        private System.Windows.Forms.CheckBox chk_PrintWaterMark;
        private System.Windows.Forms.CheckBox chk_StartAfterRun;
        private System.Windows.Forms.NotifyIcon notify_icon;
    }
}

