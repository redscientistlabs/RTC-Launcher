namespace RTCV.Launcher
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal partial class SidebarVersionsPanel : Form
    {
        public SidebarVersionsPanel()
        {
            InitializeComponent();
        }

        private void lbVersions_SelectedIndexChanged(object sender, EventArgs e) => MainForm.mf.lbVersions_SelectedIndexChanged(sender, e);

        private void lbVersions_MouseDown(object sender, MouseEventArgs e) => MainForm.mf.lbVersions_MouseDown(sender, e);

        private void SidebarVersionsPanel_Load(object sender, EventArgs e)
        {
            VerticalScroll.Enabled = true;
        }

        private void lbVersions_DragDrop(object sender, DragEventArgs e)
        {
                e.Effect = DragDropEffects.Move;

                var fd = (string[])e.Data.GetData(DataFormats.FileDrop); //file drop

                MainForm.mf.InstallFromZip(fd);
        }

        private void lbVersions_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void SidebarVersionsPanel_MouseDown(object sender, MouseEventArgs e)
        {
            var locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

            if (!lbVersions.Visible)
            {
                if (e.Button == MouseButtons.Right)
                {
                    var columnsMenu = new Components.BuildContextMenu();
                    columnsMenu.Items.Add("Install from Zip file", null, new EventHandler((ob, ev) => MainForm.mf.InstallFromZip()));
                    columnsMenu.Show(this, locate);
                }
            }
        }

        private void category_Click(object sender, EventArgs e)
        {
            btnGames.BackColor = Color.FromArgb(20, 20, 20);
            btnMusic.BackColor = Color.FromArgb(20, 20, 20);
            btnRTCV.BackColor = Color.FromArgb(20, 20, 20);

            lbVersions.Visible = false;
            lbDefaultText.Visible = false;


            if (sender == btnRTCV)
            {
                btnRTCV.BackColor = Color.FromArgb(32, 32, 32);
                MainForm.mf.btnVersionDownloader.Visible = true;
                MainForm.mf.RefreshInstalledVersions();
                lbNameVersions.Visible = true;

                if (lbVersions.Items.Count > 0)
                    lbVersions.SelectedIndex = 0;
            }
            else if (sender == btnGames)
            {
                btnGames.BackColor = Color.FromArgb(32, 32, 32);
                MainForm.mf.btnVersionDownloader.Visible = false;
                MainForm.mf.pbNewVersionNotification.Visible = false;
                lbNameVersions.Visible = false;

                //MainForm.mf.AnchorPanel(GamesPanel.gpForm);
            }
            else if (sender == btnMusic)
            {
                btnMusic.BackColor = Color.FromArgb(32, 32, 32);
                MainForm.mf.btnVersionDownloader.Visible = false;
                MainForm.mf.pbNewVersionNotification.Visible = false;
                lbNameVersions.Visible = false;

                //MainForm.mf.AnchorPanel(MusicPanel.mpForm);
            }


        }
    }
}
