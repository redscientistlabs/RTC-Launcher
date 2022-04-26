namespace RTCV.Launcher
{
    partial class VersionDownloadPanel
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
            this.lbOnlineVersions = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDownloadVersion = new System.Windows.Forms.Button();
            this.btnOfflineInstall = new System.Windows.Forms.Button();
            this.cbSelectedServer = new System.Windows.Forms.ComboBox();
            this.versionLabel = new System.Windows.Forms.Label();
            this.btnDevUnstable = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbOnlineVersions
            // 
            this.lbOnlineVersions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbOnlineVersions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.lbOnlineVersions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbOnlineVersions.DisplayMember = "Text";
            this.lbOnlineVersions.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOnlineVersions.ForeColor = System.Drawing.Color.White;
            this.lbOnlineVersions.FormattingEnabled = true;
            this.lbOnlineVersions.IntegralHeight = false;
            this.lbOnlineVersions.ItemHeight = 30;
            this.lbOnlineVersions.Location = new System.Drawing.Point(12, 34);
            this.lbOnlineVersions.Name = "lbOnlineVersions";
            this.lbOnlineVersions.Size = new System.Drawing.Size(616, 185);
            this.lbOnlineVersions.TabIndex = 129;
            this.lbOnlineVersions.Tag = "color:normal";
            this.lbOnlineVersions.ValueMember = "Value";
            this.lbOnlineVersions.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbOnlineVersions_MouseDoubleClick_1);
            this.lbOnlineVersions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbOnlineVersions_MouseDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(10, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 19);
            this.label3.TabIndex = 130;
            this.label3.Text = "Online Downloader";
            // 
            // btnDownloadVersion
            // 
            this.btnDownloadVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownloadVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnDownloadVersion.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnDownloadVersion.FlatAppearance.BorderSize = 0;
            this.btnDownloadVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadVersion.Font = new System.Drawing.Font("Segoe UI Light", 12.25F);
            this.btnDownloadVersion.ForeColor = System.Drawing.Color.White;
            this.btnDownloadVersion.Location = new System.Drawing.Point(12, 224);
            this.btnDownloadVersion.Name = "btnDownloadVersion";
            this.btnDownloadVersion.Size = new System.Drawing.Size(616, 34);
            this.btnDownloadVersion.TabIndex = 131;
            this.btnDownloadVersion.TabStop = false;
            this.btnDownloadVersion.Tag = "color:light";
            this.btnDownloadVersion.Text = "Download selected version";
            this.btnDownloadVersion.UseVisualStyleBackColor = false;
            this.btnDownloadVersion.Click += new System.EventHandler(this.btnDownloadVersion_Click);
            // 
            // btnOfflineInstall
            // 
            this.btnOfflineInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOfflineInstall.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnOfflineInstall.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnOfflineInstall.FlatAppearance.BorderSize = 0;
            this.btnOfflineInstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOfflineInstall.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnOfflineInstall.ForeColor = System.Drawing.Color.White;
            this.btnOfflineInstall.Location = new System.Drawing.Point(326, 10);
            this.btnOfflineInstall.Margin = new System.Windows.Forms.Padding(0);
            this.btnOfflineInstall.Name = "btnOfflineInstall";
            this.btnOfflineInstall.Size = new System.Drawing.Size(85, 20);
            this.btnOfflineInstall.TabIndex = 133;
            this.btnOfflineInstall.TabStop = false;
            this.btnOfflineInstall.Tag = "color:light";
            this.btnOfflineInstall.Text = "Offline Install";
            this.btnOfflineInstall.UseVisualStyleBackColor = false;
            this.btnOfflineInstall.Click += new System.EventHandler(this.btnOfflineInstall_Click);
            // 
            // cbSelectedServer
            // 
            this.cbSelectedServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSelectedServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.cbSelectedServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectedServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSelectedServer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbSelectedServer.ForeColor = System.Drawing.Color.White;
            this.cbSelectedServer.FormattingEnabled = true;
            this.cbSelectedServer.Items.AddRange(new object[] {
            "Stable Releases",
            "Development",
            "Historical"});
            this.cbSelectedServer.Location = new System.Drawing.Point(516, 10);
            this.cbSelectedServer.Name = "cbSelectedServer";
            this.cbSelectedServer.Size = new System.Drawing.Size(112, 21);
            this.cbSelectedServer.TabIndex = 134;
            this.cbSelectedServer.Tag = "color:normal";
            this.cbSelectedServer.SelectedIndexChanged += new System.EventHandler(this.cbSelectedServer_SelectedIndexChanged);
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.versionLabel.BackColor = System.Drawing.Color.Transparent;
            this.versionLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.versionLabel.ForeColor = System.Drawing.Color.White;
            this.versionLabel.Location = new System.Drawing.Point(428, 8);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(87, 19);
            this.versionLabel.TabIndex = 135;
            this.versionLabel.Text = "Selected Server:";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // btnDevUnstable
            // 
            this.btnDevUnstable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDevUnstable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnDevUnstable.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnDevUnstable.FlatAppearance.BorderSize = 0;
            this.btnDevUnstable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDevUnstable.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnDevUnstable.ForeColor = System.Drawing.Color.White;
            this.btnDevUnstable.Location = new System.Drawing.Point(229, 8);
            this.btnDevUnstable.Margin = new System.Windows.Forms.Padding(0);
            this.btnDevUnstable.Name = "btnDevUnstable";
            this.btnDevUnstable.Size = new System.Drawing.Size(88, 20);
            this.btnDevUnstable.TabIndex = 136;
            this.btnDevUnstable.TabStop = false;
            this.btnDevUnstable.Tag = "color:light";
            this.btnDevUnstable.Text = "Dev Unstable";
            this.btnDevUnstable.UseVisualStyleBackColor = false;
            this.btnDevUnstable.Click += new System.EventHandler(this.btnDevUnstable_Click);
            // 
            // VersionDownloadPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(643, 268);
            this.Controls.Add(this.btnDevUnstable);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.cbSelectedServer);
            this.Controls.Add(this.btnOfflineInstall);
            this.Controls.Add(this.btnDownloadVersion);
            this.Controls.Add(this.lbOnlineVersions);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VersionDownloadPanel";
            this.Text = "VersionDownloadPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        public System.Windows.Forms.ListBox lbOnlineVersions;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Button btnDownloadVersion;
        public System.Windows.Forms.Button btnOfflineInstall;
        public System.Windows.Forms.ComboBox cbSelectedServer;
        private System.Windows.Forms.Label versionLabel;
        public System.Windows.Forms.Button btnDevUnstable;
    }
}
