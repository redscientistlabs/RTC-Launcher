namespace RTCV.Launcher
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.Launcher.Components;

    internal partial class LaunchPanelV2 : Form
    {
        readonly LauncherConf lc;

        public LaunchPanelV2()
        {
            InitializeComponent();
            lbSelectedVersion.Visible = false;

            lc = new LauncherConf(MainForm.SelectedVersion);
        }

        public void DisplayVersion()
        {
            Size? btnSize = null;

            var folderPath = Path.Combine(MainForm.versionsDir, MainForm.SelectedVersion);
            if (!Directory.Exists(folderPath))
            {
                return;
            }

            foreach (LauncherConfItem lci in lc.items)
            {
                var newButton = new Button
                {
                    BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))))
                };
                newButton.FlatAppearance.BorderSize = 0;
                newButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                newButton.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
                newButton.ForeColor = System.Drawing.Color.Black;

                Bitmap btnImage;
                using (var bmpTemp = new Bitmap(lci.imageLocation))
                {
                    btnImage = new Bitmap(bmpTemp);
                }

                if (btnSize == null)
                {
                    //The first image sets the parameters for display
                    btnSize = new Size(btnImage.Width + 1, btnImage.Height + 1);
                }

                newButton.Name = "btnDefaultSize";
                newButton.Size = (Size)btnSize;
                newButton.TabIndex = 134;
                newButton.TabStop = false;
                newButton.Tag = lci.line;
                newButton.Text = string.Empty;
                newButton.UseVisualStyleBackColor = false;
                newButton.Click += new System.EventHandler(this.btnBatchfile_Click);

                var isAddon = !string.IsNullOrWhiteSpace(lci.downloadVersion);
                var AddonInstalled = false;

                if (isAddon)
                {
                    AddonInstalled = Directory.Exists(lci.folderLocation);
                    newButton.MouseDown += new MouseEventHandler((sender, e) =>
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            var locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

                            var columnsMenu = new BuildContextMenu();
                            columnsMenu.Items.Add("Delete", null, new EventHandler((ob, ev) => DeleteAddon(lci.folderName))).Enabled = AddonInstalled;
                            columnsMenu.Show(this, locate);
                        }

                        return;
                    });
                }

                if (isAddon)
                {
                    var p = new Pen((AddonInstalled ? Color.FromArgb(57, 255, 20) : Color.Red), 2);

                    var x1 = 8;
                    var y1 = btnImage.Height - 8;
                    var x2 = 24;
                    var y2 = btnImage.Height - 8;
                    // Draw line to screen.
                    using (var graphics = Graphics.FromImage(btnImage))
                    {
                        graphics.DrawLine(p, x1, y1, x2, y2);
                    }
                }

                newButton.Image = btnImage;
                newButton.Visible = true;
                flowLayoutPanel1.Controls.Add(newButton);
            }

            lbSelectedVersion.Text = lc.version;
            lbSelectedVersion.Visible = true;
        }

        public void DeleteAddon(string AddonFolderName)
        {
            try
            {
                var targetFolder = Path.Combine(MainForm.versionsDir, lc.version, AddonFolderName);

                if (Directory.Exists(targetFolder))
                {
                    Directory.Delete(targetFolder, true);
                }
            }
            catch (Exception ex)
            {
                var result = MessageBox.Show($"Could not delete addon {AddonFolderName} because of the following error:\n{ex}", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (result == DialogResult.Retry)
                {
                    DeleteAddon(AddonFolderName);
                    return;
                }
            }

            MainForm.RefreshKeepSelectedVersion();
            //MainForm.mf.RefreshInterface();
        }

        private void NewLaunchPanel_Load(object sender, EventArgs e)
        {
            DisplayVersion();
        }

        private void btnBatchfile_Click(object sender, EventArgs e)
        {
            var currentButton = (Button)sender;

            var line = (string)currentButton.Tag;
            var lci = new LauncherConfItem(lc, line);

            if (!Directory.Exists(lci.folderLocation))
            {
                if (string.IsNullOrWhiteSpace(lci.downloadVersion))
                {
                    MessageBox.Show($"A required folder is missing: {lci.downloadVersion}\nNo download location was provided", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                LauncherConf lcCandidateForPull = getFolderFromPreviousVersion(lci.downloadVersion);
                if (lcCandidateForPull != null)
                {
                    var resultAskPull = MessageBox.Show($"The component {lci.folderName} could be imported from {lcCandidateForPull.version}\nDo you wish import it?", "Import candidate found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultAskPull == DialogResult.Yes)
                    {
                        LauncherConfItem candidate = lcCandidateForPull.items.FirstOrDefault(it => it.downloadVersion == lci.downloadVersion);
                        //handle it here
                        try
                        {
                            RTC_Extensions.RecursiveCopyNukeReadOnly(new DirectoryInfo(candidate.folderLocation), new DirectoryInfo(lci.folderLocation));
                            RTC_Extensions.RecursiveDeleteNukeReadOnly(new DirectoryInfo(candidate.folderLocation));
                            MainForm.RefreshKeepSelectedVersion();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Couldn't copy {candidate.folderLocation ?? "NULL"} to {lci.folderLocation}.\nIs the file in use?\nException:{ex.Message}");
                            try
                            {
                                RTC_Extensions.RecursiveDeleteNukeReadOnly(new DirectoryInfo(lci.folderLocation));
                            }
                            catch (Exception _ex) //f
                            {
                                Console.WriteLine(_ex);
                            }
                        }
                        return;
                    }
                }

                var result = MessageBox.Show($"The following component is missing: {lci.folderName}\nDo you wish to download it?", "Additional download required", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var downloadUrl = $"{MainForm.webResourceDomain}/rtc/addons/" + lci.downloadVersion + ".zip";
                    var downloadedFile = Path.Combine(MainForm.launcherDir, "PACKAGES", lci.downloadVersion + ".zip");
                    var extractDirectory = lci.folderLocation;

                    MainForm.DownloadFile(new Uri(downloadUrl), downloadedFile, extractDirectory);
                }

                return;
            }

            if (lci.batchLocation.Contains("http"))
            {
                Process.Start(lci.batchName);
                return;
            }

            var psi = new ProcessStartInfo
            {
                FileName = Path.GetFileName(lci.batchLocation),
                WorkingDirectory = Path.GetDirectoryName(lci.batchLocation)
            };
            Process.Start(psi);
        }

        private static LauncherConf getFolderFromPreviousVersion(string downloadVersion)
        {
            foreach (var ver in MainForm.sideversionForm.lbVersions.Items.Cast<string>())
            {
                if (downloadVersion == ver)
                {
                    continue;
                }

                var lc = new LauncherConf(ver);

                LauncherConfItem lci = lc.items.FirstOrDefault(it => it.downloadVersion == downloadVersion);
                if (lci != null)
                {
                    if (Directory.Exists(lci.folderLocation))
                    {
                        return lc;
                    }
                }
            }

            return null;
        }

        private void flowLayoutPanel1_MouseEnter(object sender, EventArgs e)
        {
            flowLayoutPanel1.Focus();
        }
    }
}
