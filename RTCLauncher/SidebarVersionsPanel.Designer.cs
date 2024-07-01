namespace RTCV.Launcher
{
    partial class SidebarVersionsPanel
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
            this.lbVersions = new System.Windows.Forms.ListBox();
            this.lbDefaultText = new System.Windows.Forms.Label();
            this.lbNameVersions = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStepback = new System.Windows.Forms.Button();
            this.btnRTCV = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbVersions
            // 
            this.lbVersions.AllowDrop = true;
            this.lbVersions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbVersions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(8)))), ((int)(((byte)(8)))));
            this.lbVersions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbVersions.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Bold);
            this.lbVersions.ForeColor = System.Drawing.Color.White;
            this.lbVersions.FormattingEnabled = true;
            this.lbVersions.IntegralHeight = false;
            this.lbVersions.ItemHeight = 21;
            this.lbVersions.Location = new System.Drawing.Point(9, 168);
            this.lbVersions.Margin = new System.Windows.Forms.Padding(0);
            this.lbVersions.Name = "lbVersions";
            this.lbVersions.Size = new System.Drawing.Size(136, 332);
            this.lbVersions.TabIndex = 83;
            this.lbVersions.Tag = "";
            this.lbVersions.SelectedIndexChanged += new System.EventHandler(this.lbVersions_SelectedIndexChanged);
            this.lbVersions.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbVersions_DragDrop);
            this.lbVersions.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbVersions_DragEnter);
            this.lbVersions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbVersions_MouseDown);
            // 
            // lbDefaultText
            // 
            this.lbDefaultText.AutoSize = true;
            this.lbDefaultText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.lbDefaultText.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Bold);
            this.lbDefaultText.ForeColor = System.Drawing.Color.White;
            this.lbDefaultText.Location = new System.Drawing.Point(16, 172);
            this.lbDefaultText.Name = "lbDefaultText";
            this.lbDefaultText.Size = new System.Drawing.Size(120, 21);
            this.lbDefaultText.TabIndex = 84;
            this.lbDefaultText.Text = "None Installed";
            this.lbDefaultText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbVersions_MouseDown);
            // 
            // lbNameVersions
            // 
            this.lbNameVersions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbNameVersions.AutoSize = true;
            this.lbNameVersions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.lbNameVersions.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNameVersions.ForeColor = System.Drawing.Color.White;
            this.lbNameVersions.Location = new System.Drawing.Point(12, 143);
            this.lbNameVersions.Name = "lbNameVersions";
            this.lbNameVersions.Size = new System.Drawing.Size(94, 21);
            this.lbNameVersions.TabIndex = 140;
            this.lbNameVersions.Text = "RTC Versions";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.label1.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 21);
            this.label1.TabIndex = 142;
            this.label1.Text = "Environment";

            // 
            // btnStepback
            // 
            this.btnStepback.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStepback.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnStepback.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnStepback.FlatAppearance.BorderSize = 0;
            this.btnStepback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStepback.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnStepback.ForeColor = System.Drawing.Color.White;
            this.btnStepback.Image = global::RTCV.Launcher.Properties.Resources.p16;
            this.btnStepback.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStepback.Location = new System.Drawing.Point(9, 73);
            this.btnStepback.Name = "btnStepback";
            this.btnStepback.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnStepback.Size = new System.Drawing.Size(136, 31);
            this.btnStepback.TabIndex = 143;
            this.btnStepback.TabStop = false;
            this.btnStepback.Tag = "";
            this.btnStepback.Text = "  STEPBACK";
            this.btnStepback.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStepback.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStepback.UseVisualStyleBackColor = false;
            this.btnStepback.Click += new System.EventHandler(this.category_Click);
            // 
            // btnRTCV
            // 
            this.btnRTCV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRTCV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnRTCV.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnRTCV.FlatAppearance.BorderSize = 0;
            this.btnRTCV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRTCV.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnRTCV.ForeColor = System.Drawing.Color.White;
            this.btnRTCV.Image = global::RTCV.Launcher.Properties.Resources.RTCV_16x16;
            this.btnRTCV.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRTCV.Location = new System.Drawing.Point(9, 36);
            this.btnRTCV.Name = "btnRTCV";
            this.btnRTCV.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnRTCV.Size = new System.Drawing.Size(136, 31);
            this.btnRTCV.TabIndex = 133;
            this.btnRTCV.TabStop = false;
            this.btnRTCV.Tag = "";
            this.btnRTCV.Text = "  RTCV";
            this.btnRTCV.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRTCV.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRTCV.UseVisualStyleBackColor = false;
            this.btnRTCV.Click += new System.EventHandler(this.category_Click);
            // 
            // SidebarVersionsPanel
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(154, 509);
            this.Controls.Add(this.btnStepback);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbNameVersions);
            this.Controls.Add(this.btnRTCV);
            this.Controls.Add(this.lbDefaultText);
            this.Controls.Add(this.lbVersions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SidebarVersionsPanel";
            this.Text = "VesionSelectPanel";
            this.Load += new System.EventHandler(this.SidebarVersionsPanel_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbVersions_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbVersions_DragEnter);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SidebarVersionsPanel_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox lbVersions;
        public System.Windows.Forms.Label lbDefaultText;
        public System.Windows.Forms.Label lbNameVersions;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btnRTCV;
        public System.Windows.Forms.Button btnStepback;
    }
}
