namespace RTCV.Launcher
{
    partial class MusicPanel
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
            this.lbPanelText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // flowVisiblePanel
            // 
            this.flowVisiblePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowVisiblePanel.AutoScroll = true;
            this.flowVisiblePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.flowVisiblePanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowVisiblePanel.Location = new System.Drawing.Point(5, 36);
            this.flowVisiblePanel.Margin = new System.Windows.Forms.Padding(8);
            this.flowVisiblePanel.Name = "flowVisiblePanel";
            this.flowVisiblePanel.Padding = new System.Windows.Forms.Padding(16);
            this.flowVisiblePanel.Size = new System.Drawing.Size(980, 500);
            this.flowVisiblePanel.TabIndex = 136;
            this.flowVisiblePanel.WrapContents = false;
            this.flowVisiblePanel.MouseEnter += new System.EventHandler(this.flowVisiblePanel_MouseEnter);
            this.flowVisiblePanel.MouseHover += new System.EventHandler(this.flowVisiblePanel_MouseEnter);
            // 
            // lbPanelText
            // 
            this.lbPanelText.AutoSize = true;
            this.lbPanelText.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbPanelText.ForeColor = System.Drawing.Color.White;
            this.lbPanelText.Location = new System.Drawing.Point(8, 13);
            this.lbPanelText.Name = "lbPanelText";
            this.lbPanelText.Size = new System.Drawing.Size(127, 19);
            this.lbPanelText.TabIndex = 135;
            this.lbPanelText.Text = "Redscientist Music";
            // 
            // MusicPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(990, 548);
            this.Controls.Add(this.flowVisiblePanel);
            this.Controls.Add(this.lbPanelText);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MusicPanel";
            this.Text = "WebPanel";
            this.Load += new System.EventHandler(this.MusicPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowVisiblePanel;
        public System.Windows.Forms.Label lbPanelText;
    }
}
