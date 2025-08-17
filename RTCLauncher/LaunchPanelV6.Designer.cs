namespace RTCV.Launcher
{
    partial class LaunchPanelV6
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
            this.lbSelectedVersion = new System.Windows.Forms.Label();
            this.flowVisiblePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTutorials = new System.Windows.Forms.Button();
            this.btnDiscord = new System.Windows.Forms.Button();
            this.btnOnlineGuide = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbSelectedVersion
            // 
            this.lbSelectedVersion.AutoSize = true;
            this.lbSelectedVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbSelectedVersion.ForeColor = System.Drawing.Color.White;
            this.lbSelectedVersion.Location = new System.Drawing.Point(8, 12);
            this.lbSelectedVersion.Name = "lbSelectedVersion";
            this.lbSelectedVersion.Size = new System.Drawing.Size(117, 19);
            this.lbSelectedVersion.TabIndex = 133;
            this.lbSelectedVersion.Text = "Program Selector";
            this.lbSelectedVersion.Visible = false;
            // 
            // flowVisiblePanel
            // 
            this.flowVisiblePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowVisiblePanel.AutoScroll = true;
            this.flowVisiblePanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowVisiblePanel.Location = new System.Drawing.Point(5, 35);
            this.flowVisiblePanel.Margin = new System.Windows.Forms.Padding(8);
            this.flowVisiblePanel.Name = "flowVisiblePanel";
            this.flowVisiblePanel.Padding = new System.Windows.Forms.Padding(4);
            this.flowVisiblePanel.Size = new System.Drawing.Size(980, 500);
            this.flowVisiblePanel.TabIndex = 134;
            this.flowVisiblePanel.WrapContents = false;
            this.flowVisiblePanel.SizeChanged += new System.EventHandler(this.flowVisiblePanel_SizeChanged);
            this.flowVisiblePanel.MouseEnter += new System.EventHandler(this.flowVisiblePanel_MouseEnter);
            // 
            // btnTutorials
            // 
            this.btnTutorials.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTutorials.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnTutorials.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnTutorials.FlatAppearance.BorderSize = 0;
            this.btnTutorials.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTutorials.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnTutorials.ForeColor = System.Drawing.Color.White;
            this.btnTutorials.Image = global::RTCV.Launcher.Properties.Resources.youtube;
            this.btnTutorials.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTutorials.Location = new System.Drawing.Point(774, 17);
            this.btnTutorials.Name = "btnTutorials";
            this.btnTutorials.Size = new System.Drawing.Size(81, 24);
            this.btnTutorials.TabIndex = 137;
            this.btnTutorials.TabStop = false;
            this.btnTutorials.Tag = "";
            this.btnTutorials.Text = " Tutorials";
            this.btnTutorials.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTutorials.UseVisualStyleBackColor = false;
            this.btnTutorials.Click += new System.EventHandler(this.btnTutorials_Click);
            // 
            // btnDiscord
            // 
            this.btnDiscord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDiscord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnDiscord.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDiscord.FlatAppearance.BorderSize = 0;
            this.btnDiscord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiscord.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnDiscord.ForeColor = System.Drawing.Color.White;
            this.btnDiscord.Image = global::RTCV.Launcher.Properties.Resources.discord;
            this.btnDiscord.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDiscord.Location = new System.Drawing.Point(695, 17);
            this.btnDiscord.Name = "btnDiscord";
            this.btnDiscord.Size = new System.Drawing.Size(73, 24);
            this.btnDiscord.TabIndex = 136;
            this.btnDiscord.TabStop = false;
            this.btnDiscord.Tag = "";
            this.btnDiscord.Text = " Discord";
            this.btnDiscord.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDiscord.UseVisualStyleBackColor = false;
            this.btnDiscord.Click += new System.EventHandler(this.btnDiscord_Click);
            // 
            // btnOnlineGuide
            // 
            this.btnOnlineGuide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOnlineGuide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnOnlineGuide.FlatAppearance.BorderSize = 0;
            this.btnOnlineGuide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOnlineGuide.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnOnlineGuide.ForeColor = System.Drawing.Color.White;
            this.btnOnlineGuide.Image = global::RTCV.Launcher.Properties.Resources.corruptwiki;
            this.btnOnlineGuide.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOnlineGuide.Location = new System.Drawing.Point(861, 17);
            this.btnOnlineGuide.Name = "btnOnlineGuide";
            this.btnOnlineGuide.Size = new System.Drawing.Size(113, 24);
            this.btnOnlineGuide.TabIndex = 135;
            this.btnOnlineGuide.TabStop = false;
            this.btnOnlineGuide.Tag = "";
            this.btnOnlineGuide.Text = " Online guide";
            this.btnOnlineGuide.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOnlineGuide.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOnlineGuide.UseVisualStyleBackColor = false;
            this.btnOnlineGuide.Click += new System.EventHandler(this.btnOnlineGuide_Click);
            // 
            // LaunchPanelV6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(990, 548);
            this.Controls.Add(this.btnTutorials);
            this.Controls.Add(this.btnDiscord);
            this.Controls.Add(this.btnOnlineGuide);
            this.Controls.Add(this.flowVisiblePanel);
            this.Controls.Add(this.lbSelectedVersion);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LaunchPanelV6";
            this.Text = "LaunchPanelV6";
            this.Load += new System.EventHandler(this.LaunchPanelV6_Load);
            this.Resize += new System.EventHandler(this.LaunchPanelV6_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lbSelectedVersion;
        private System.Windows.Forms.FlowLayoutPanel flowVisiblePanel;
        private System.Windows.Forms.Button btnTutorials;
        private System.Windows.Forms.Button btnDiscord;
        private System.Windows.Forms.Button btnOnlineGuide;
    }
}
