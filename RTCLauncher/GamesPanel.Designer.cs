namespace RTCV.Launcher
{
    partial class GamesPanel
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
            this.flowVisiblePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lbSelectedVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // flowVisiblePanel
            // 
            this.flowVisiblePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowVisiblePanel.AutoScroll = true;
            this.flowVisiblePanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowVisiblePanel.Location = new System.Drawing.Point(5, 36);
            this.flowVisiblePanel.Margin = new System.Windows.Forms.Padding(8);
            this.flowVisiblePanel.Name = "flowVisiblePanel";
            this.flowVisiblePanel.Padding = new System.Windows.Forms.Padding(4);
            this.flowVisiblePanel.Size = new System.Drawing.Size(980, 500);
            this.flowVisiblePanel.TabIndex = 138;
            this.flowVisiblePanel.WrapContents = false;
            // 
            // lbSelectedVersion
            // 
            this.lbSelectedVersion.AutoSize = true;
            this.lbSelectedVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbSelectedVersion.ForeColor = System.Drawing.Color.White;
            this.lbSelectedVersion.Location = new System.Drawing.Point(8, 13);
            this.lbSelectedVersion.Name = "lbSelectedVersion";
            this.lbSelectedVersion.Size = new System.Drawing.Size(94, 19);
            this.lbSelectedVersion.TabIndex = 137;
            this.lbSelectedVersion.Text = "VRUN Games";
            // 
            // GamesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(990, 548);
            this.Controls.Add(this.flowVisiblePanel);
            this.Controls.Add(this.lbSelectedVersion);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GamesPanel";
            this.Text = "WebPanel";
            this.Load += new System.EventHandler(this.WebPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowVisiblePanel;
        public System.Windows.Forms.Label lbSelectedVersion;
    }
}
