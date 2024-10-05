namespace RTCV.Launcher
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pnBottomPanel = new System.Windows.Forms.Panel();
            this.lbMOTD = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pnLeftSide = new System.Windows.Forms.Panel();
            this.pbNewVersionNotification = new System.Windows.Forms.PictureBox();
            this.btnVersionDownloader = new System.Windows.Forms.Button();
            this.pnAnchorRight = new System.Windows.Forms.Panel();
            this.pnTopPanel = new System.Windows.Forms.Panel();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnMaximize = new System.Windows.Forms.Button();
            this.versionLabel = new System.Windows.Forms.Label();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.pnBottomPanel.SuspendLayout();
            this.pnLeftSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewVersionNotification)).BeginInit();
            this.pnTopPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnBottomPanel
            // 
            this.pnBottomPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.pnBottomPanel.Controls.Add(this.lbMOTD);
            this.pnBottomPanel.Controls.Add(this.label5);
            this.pnBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottomPanel.Location = new System.Drawing.Point(170, 621);
            this.pnBottomPanel.Name = "pnBottomPanel";
            this.pnBottomPanel.Size = new System.Drawing.Size(999, 45);
            this.pnBottomPanel.TabIndex = 0;
            // 
            // lbMOTD
            // 
            this.lbMOTD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMOTD.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lbMOTD.ForeColor = System.Drawing.Color.White;
            this.lbMOTD.Location = new System.Drawing.Point(2, 21);
            this.lbMOTD.Name = "lbMOTD";
            this.lbMOTD.Size = new System.Drawing.Size(997, 18);
            this.lbMOTD.TabIndex = 125;
            this.lbMOTD.Text = "...";
            this.lbMOTD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbMOTD.Visible = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(2, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(997, 18);
            this.label5.TabIndex = 132;
            this.label5.Text = "RTC, emulator mods and stubs are developed by Redscientist Labs, consult redscien" +
    "tist.com for more details. Click here to view third-party licenses.";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // pnLeftSide
            // 
            this.pnLeftSide.AutoSize = true;
            this.pnLeftSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.pnLeftSide.Controls.Add(this.pbNewVersionNotification);
            this.pnLeftSide.Controls.Add(this.btnVersionDownloader);
            this.pnLeftSide.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnLeftSide.Location = new System.Drawing.Point(0, 41);
            this.pnLeftSide.MinimumSize = new System.Drawing.Size(170, 509);
            this.pnLeftSide.Name = "pnLeftSide";
            this.pnLeftSide.Size = new System.Drawing.Size(170, 625);
            this.pnLeftSide.TabIndex = 134;
            // 
            // pbNewVersionNotification
            // 
            this.pbNewVersionNotification.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbNewVersionNotification.Image = global::RTCV.Launcher.Properties.Resources.notificationBadge;
            this.pbNewVersionNotification.InitialImage = global::RTCV.Launcher.Properties.Resources.notificationBadge;
            this.pbNewVersionNotification.Location = new System.Drawing.Point(142, 585);
            this.pbNewVersionNotification.Name = "pbNewVersionNotification";
            this.pbNewVersionNotification.Size = new System.Drawing.Size(14, 14);
            this.pbNewVersionNotification.TabIndex = 131;
            this.pbNewVersionNotification.TabStop = false;
            this.pbNewVersionNotification.Visible = false;
            // 
            // btnVersionDownloader
            // 
            this.btnVersionDownloader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnVersionDownloader.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnVersionDownloader.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnVersionDownloader.FlatAppearance.BorderSize = 0;
            this.btnVersionDownloader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVersionDownloader.Font = new System.Drawing.Font("Segoe UI Light", 12.25F);
            this.btnVersionDownloader.ForeColor = System.Drawing.Color.White;
            this.btnVersionDownloader.Location = new System.Drawing.Point(0, 580);
            this.btnVersionDownloader.Name = "btnVersionDownloader";
            this.btnVersionDownloader.Size = new System.Drawing.Size(170, 45);
            this.btnVersionDownloader.TabIndex = 132;
            this.btnVersionDownloader.TabStop = false;
            this.btnVersionDownloader.Tag = "";
            this.btnVersionDownloader.Text = "Downloader";
            this.btnVersionDownloader.UseVisualStyleBackColor = false;
            this.btnVersionDownloader.Click += new System.EventHandler(this.btnVersionDownloader_Click);
            this.btnVersionDownloader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnVersionDownloader_MouseDown);
            // 
            // pnAnchorRight
            // 
            this.pnAnchorRight.AutoSize = true;
            this.pnAnchorRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.pnAnchorRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnAnchorRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnAnchorRight.Location = new System.Drawing.Point(170, 41);
            this.pnAnchorRight.Name = "pnAnchorRight";
            this.pnAnchorRight.Size = new System.Drawing.Size(999, 580);
            this.pnAnchorRight.TabIndex = 133;
            // 
            // pnTopPanel
            // 
            this.pnTopPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.pnTopPanel.BackgroundImage = global::RTCV.Launcher.Properties.Resources.LauncherBack;
            this.pnTopPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnTopPanel.Controls.Add(this.btnUpdate);
            this.pnTopPanel.Controls.Add(this.btnMaximize);
            this.pnTopPanel.Controls.Add(this.versionLabel);
            this.pnTopPanel.Controls.Add(this.btnMinimize);
            this.pnTopPanel.Controls.Add(this.btnQuit);
            this.pnTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTopPanel.Location = new System.Drawing.Point(0, 0);
            this.pnTopPanel.Name = "pnTopPanel";
            this.pnTopPanel.Size = new System.Drawing.Size(1169, 41);
            this.pnTopPanel.TabIndex = 131;
            this.pnTopPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnTopPanel_MouseDoubleClick);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(32)))), ((int)(((byte)(16)))));
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.ForeColor = System.Drawing.Color.LawnGreen;
            this.btnUpdate.Location = new System.Drawing.Point(921, 8);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(121, 24);
            this.btnUpdate.TabIndex = 135;
            this.btnUpdate.TabStop = false;
            this.btnUpdate.Tag = "";
            this.btnUpdate.Text = "♦ Update Launcher";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Visible = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnMaximize
            // 
            this.btnMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnMaximize.FlatAppearance.BorderSize = 0;
            this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnMaximize.ForeColor = System.Drawing.Color.White;
            this.btnMaximize.Location = new System.Drawing.Point(1111, 8);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(22, 24);
            this.btnMaximize.TabIndex = 134;
            this.btnMaximize.TabStop = false;
            this.btnMaximize.Tag = "";
            this.btnMaximize.Text = "▢";
            this.btnMaximize.UseVisualStyleBackColor = false;
            this.btnMaximize.Click += new System.EventHandler(this.btnMaximize_Click);
            // 
            // versionLabel
            // 
            this.versionLabel.Font = new System.Drawing.Font("Segoe UI", 7F);
            this.versionLabel.ForeColor = System.Drawing.Color.Gray;
            this.versionLabel.Location = new System.Drawing.Point(186, 9);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(25, 23);
            this.versionLabel.TabIndex = 133;
            this.versionLabel.Text = "VER";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Location = new System.Drawing.Point(1083, 8);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(22, 24);
            this.btnMinimize.TabIndex = 131;
            this.btnMinimize.TabStop = false;
            this.btnMinimize.Tag = "";
            this.btnMinimize.Text = "_";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnQuit.FlatAppearance.BorderSize = 0;
            this.btnQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnQuit.ForeColor = System.Drawing.Color.White;
            this.btnQuit.Location = new System.Drawing.Point(1139, 8);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(22, 24);
            this.btnQuit.TabIndex = 130;
            this.btnQuit.TabStop = false;
            this.btnQuit.Tag = "";
            this.btnQuit.Text = "X";
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            this.btnQuit.MouseEnter += new System.EventHandler(this.btnQuit_MouseEnter);
            this.btnQuit.MouseLeave += new System.EventHandler(this.btnQuit_MouseLeave);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(1169, 666);
            this.Controls.Add(this.pnAnchorRight);
            this.Controls.Add(this.pnBottomPanel);
            this.Controls.Add(this.pnLeftSide);
            this.Controls.Add(this.pnTopPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1169, 666);
            this.Name = "MainForm";
            this.Tag = "";
            this.Text = "RTC Launcher";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnBottomPanel.ResumeLayout(false);
            this.pnLeftSide.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbNewVersionNotification)).EndInit();
            this.pnTopPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnBottomPanel;
        private System.Windows.Forms.Label lbMOTD;
        private System.Windows.Forms.Panel pnTopPanel;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Panel pnLeftSide;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Panel pnAnchorRight;
        public System.Windows.Forms.Button btnVersionDownloader;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Button btnMaximize;
        public System.Windows.Forms.PictureBox pbNewVersionNotification;
        private System.Windows.Forms.Button btnUpdate;
    }
}

