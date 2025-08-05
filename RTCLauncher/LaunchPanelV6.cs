namespace RTCV.Launcher
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.Launcher.Components;

    using System.Net.Sockets;
    using System.Text;
    using Newtonsoft.Json;

#pragma warning disable CA2213 //Component designer classes generate their own Dispose method
    internal partial class LaunchPanelV6 : Form, ILauncherJsonConfPanelV6
    {
        private readonly LauncherConfJsonV6 lc;
        private readonly Timer sidebarCloseTimer;
        private readonly List<Button> HiddenButtons = new List<Button>();

        public LaunchPanelV6(string forceVersion = null)
        {
            InitializeComponent();
            lbSelectedVersion.Visible = false;

            if (forceVersion == null)
                forceVersion = MainForm.SelectedVersion;

            lc = new LauncherConfJsonV6(forceVersion);

            sidebarCloseTimer = new Timer
            {
                Interval = 333
            };
            sidebarCloseTimer.Tick += SidebarCloseTimer_Tick;
        }



        public void DrawPanel()
        {
            if (MainForm.SelectedVersion != null)
            {
                var folderPath = Path.Combine(MainForm.versionsDir, MainForm.SelectedVersion);
                if (!Directory.Exists(folderPath))
                {
                    return;
                }
            }



            flowVisiblePanel.Controls.Clear();

            Size? btnSize = null;
            HiddenButtons.Clear();

            void InitializeItem(LauncherConfJsonV6 lc, LauncherConfJsonItemV6 lcji, FlowLayoutPanel panel) //.Where(it => !it.HideItem))
            {
                Bitmap btnImage;
                using (var bmpTemp = new Bitmap(new MemoryStream(File.ReadAllBytes(Path.Combine(lc.LauncherAssetLocation, lcji.ImageName)))))
                {
                    btnImage = new Bitmap(bmpTemp);
                    if (btnSize == null)
                    {
                        btnSize = new Size(btnImage.Width + 1, btnImage.Height + 1);
                    }
                }

                var newButton = new Button();
                newButton.Size = btnSize.Value;
                PrepareButton(lcji, newButton);
                newButton.Image = btnImage;

                var isAddon = !string.IsNullOrWhiteSpace(lcji.DownloadVersion);
                var AddonInstalled = false;


                if (isAddon)
                    AddonInstalled = PrepareButtonAsAddon(lcji, newButton);

                bool showUnstable = false;

                if (!AddonInstalled && (lcji.HideItem || (!showUnstable && lcji.ItemClass == "UNSTABLE"))) //Hidden non-installed addons that requests it
                {
                    newButton.Size = new Size(0, 0);
                    newButton.Location = new Point(0, 0);
                    HiddenButtons.Add(newButton);
                    return;
                }

                newButton.Visible = true;
                panel.Controls.Add(newButton);
            }

            var installedItems = lc.Items.Where(it => isInstalled(lc, it)).ToList();
            var notInstalledItems = lc.Items.Where(it => !isInstalled(lc, it)).ToList();

            var add = installedItems.FirstOrDefault(it => it.ItemName == "Add");
            if (add != null)
            {
                installedItems.Remove(add);
            }

            var ED = installedItems.FirstOrDefault(it => it.ItemName == "Eternal Degrade");
            if (ED != null)
            {
                installedItems.Remove(ED);
            }

            var InstalledItemsTier3 = installedItems.Where(it => it.ItemClass == "TIER3");
            var InstalledItemsTools = installedItems.Where(it => it.ItemClass == "TOOL");
            var InstalledItemsTier2 = installedItems.Where(it => it.ItemClass == "TIER2");
            var InstalledItemsTier1 = installedItems.Where(it => it.ItemClass == "TIER1");
            var InstalledItemsAddon = installedItems.Where(it => it.ItemClass == "ADDON");
            var InstalledItemsCompat = installedItems.Where(it => it.ItemClass == "COMPATIBILITY");
            var InstalledItemsUnstable = installedItems.Where(it => it.ItemClass == "UNSTABLE");

            var NotInstalledItemsTier3 = notInstalledItems.Where(it => it.ItemClass == "TIER3");
            var NotInstalledItemsTools = notInstalledItems.Where(it => it.ItemClass == "TOOL");
            var NotInstalledItemsTier2 = notInstalledItems.Where(it => it.ItemClass == "TIER2");
            var NotInstalledItemsTier1 = notInstalledItems.Where(it => it.ItemClass == "TIER1");
            var NotInstalledItemsAddon = notInstalledItems.Where(it => it.ItemClass == "ADDON");
            var NotInstalledItemsCompat = notInstalledItems.Where(it => it.ItemClass == "COMPATIBILITY");
            var NotInstalledItemsUnstable = notInstalledItems.Where(it => it.ItemClass == "UNSTABLE");




            FlowLayoutPanel flpInstalled = new FlowLayoutPanel()
            {
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
            };

            if (ED != null)
            {
                InitializeItem(lc, ED, flpInstalled);
            }

            foreach (var item in InstalledItemsTier3)
                InitializeItem(lc, item, flpInstalled);
            foreach (var item in InstalledItemsTier2)
                InitializeItem(lc, item, flpInstalled);
            foreach (var item in InstalledItemsTier1)
                InitializeItem(lc, item, flpInstalled);
            foreach (var item in InstalledItemsAddon)
                InitializeItem(lc, item, flpInstalled);
            foreach (var item in InstalledItemsCompat)
                InitializeItem(lc, item, flpInstalled);
            foreach (var item in InstalledItemsTools)
                InitializeItem(lc, item, flpInstalled);
            foreach (var item in InstalledItemsUnstable)
                InitializeItem(lc, item, flpInstalled);

            FlowLayoutPanel flpNotInstalledItemsMain = new FlowLayoutPanel()
            {
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
            };

            foreach (var item in NotInstalledItemsTier3)
                InitializeItem(lc, item, flpNotInstalledItemsMain);
            foreach (var item in NotInstalledItemsTools)
                InitializeItem(lc, item, flpNotInstalledItemsMain);
            foreach (var item in NotInstalledItemsUnstable)
                InitializeItem(lc, item, flpNotInstalledItemsMain);


            if (add != null)
            {
                InitializeItem(lc, add, flpInstalled);
            }

            FlowLayoutPanel flpNotInstalledItemsExtras = new FlowLayoutPanel()
            {
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
            };
            foreach (var item in NotInstalledItemsCompat)
                InitializeItem(lc, item, flpNotInstalledItemsExtras);

            foreach (var item in NotInstalledItemsTier2)
                InitializeItem(lc, item, flpNotInstalledItemsExtras);

            foreach (var item in NotInstalledItemsTier1)
                InitializeItem(lc, item, flpNotInstalledItemsExtras);

            foreach (var item in NotInstalledItemsAddon)
                InitializeItem(lc, item, flpNotInstalledItemsExtras);

            flowVisiblePanel.FlowDirection = FlowDirection.TopDown;

            if (flpInstalled.Controls.Count > 0)
            {
                flowVisiblePanel.Controls.Add(new Label()
                {
                    Text = "Installed Applications",
                    AutoSize = true,
                    ForeColor = Color.White,
                });
                flowVisiblePanel.Controls.Add(flpInstalled);
            }

            if (flpNotInstalledItemsMain.Controls.Count > 0)
            {
                flowVisiblePanel.Controls.Add(new Label() { Text = string.Empty });
                flowVisiblePanel.Controls.Add(new Label()
                {
                    Text = "Main Add-ons",
                    AutoSize = true,
                    ForeColor = Color.White,
                });
                flowVisiblePanel.Controls.Add(flpNotInstalledItemsMain);
            }

            if (flpNotInstalledItemsExtras.Controls.Count > 0 )
            {
                flowVisiblePanel.Controls.Add(new Label() { Text = string.Empty });
                flowVisiblePanel.Controls.Add(new Label()
                {
                    AutoSize = true,
                    Text = "Extra Betas, Prototypes and Compatibility Add-ons",
                    ForeColor = Color.White,
                });
                flowVisiblePanel.Controls.Add(flpNotInstalledItemsExtras);

            }


            lbSelectedVersion.Text = lc.Version;
            lbSelectedVersion.Visible = true;
        }

        private bool isInstalled(LauncherConfJsonV6 lc, LauncherConfJsonItemV6 lcji)
        {
            return Directory.Exists(Path.Combine(lc.VersionLocation, lcji.FolderName));
        }

        private bool PrepareButtonAsAddon(LauncherConfJsonItemV6 lcji, Button newButton)
        {

            var AddonInstalled = Directory.Exists(Path.Combine(lc.VersionLocation, lcji.FolderName));

            newButton.MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    var button = (sender as Control);
                    var categoryPanel = button.Parent;
                    var panelV6 = categoryPanel.Parent;

                    int absoluteX = e.Location.X + button.Location.X + categoryPanel.Location.X + panelV6.Location.X;
                    int absoluteY = e.Location.Y + button.Location.Y + categoryPanel.Location.Y + panelV6.Location.Y;

                    var locate = new Point(absoluteX, absoluteY);

                    var columnsMenu = new Components.BuildContextMenu();

                    columnsMenu.Items.Add("Open Folder", null, (ob, ev) =>
                    {
                        var addonFolderPath = Path.Combine(MainForm.versionsDir, lc.Version, lcji.FolderName);

                        if (Directory.Exists(addonFolderPath))
                        {
                            Process.Start(addonFolderPath);
                        }
                    }).Enabled = AddonInstalled;
                    columnsMenu.Items.Add(new ToolStripSeparator());
                    columnsMenu.Items.Add("Delete Addon", null, (ob, ev) => DeleteAddon(lcji)).Enabled = (lcji.IsAddon || AddonInstalled);

                    columnsMenu.Show(this, locate);
                }
            };


            var p = new Pen((AddonInstalled ? Color.FromArgb(57, 255, 20) : Color.Red), 1);
            var b = new System.Drawing.SolidBrush((AddonInstalled ? Color.FromArgb(57, 255, 20) : Color.Red));

            var x1 = 3;
            var y1 = newButton.Image.Height - 7;
            var x2 = 4;
            var y2 = 4;
            // Draw line to screen.
            using (var graphics = Graphics.FromImage(newButton.Image))
            {
                graphics.FillRectangle(b, x1, y1, x2, y2);
            }

            return AddonInstalled;
        }

        private void PrepareButton(LauncherConfJsonItemV6 lcji, Button newButton)
        {

            newButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            newButton.FlatAppearance.BorderSize = 0;
            newButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            newButton.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            newButton.ForeColor = System.Drawing.Color.Black;
            newButton.Name = lcji.FolderName;
            newButton.TabIndex = 134;
            newButton.TabStop = false;
            newButton.Tag = lcji;
            newButton.Text = string.Empty;
            newButton.UseVisualStyleBackColor = false;

            if (lcji.ImageName == "Add.png")
            {
                newButton.AllowDrop = true;
                newButton.MouseDown += AddButton_MouseDown;
                newButton.DragEnter += AddButton_DragEnter;
                newButton.DragDrop += AddButton_DragDrop;
            }
            else
            {
                newButton.Click += this.btnBatchfile_Click;
            }

            newButton.MouseEnter += NewButton_MouseEnter;
            newButton.MouseLeave += NewButton_MouseLeave;
        }

        public void InstallCustomPackages()
        {
            var ofd = new OpenFileDialog
            {
                DefaultExt = "pkg",
                Title = "Open Package files",
                Filter = "PKG files|*.pkg",
                RestoreDirectory = true,
                Multiselect = true
            };

            string[] fileNames;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileNames = ofd.FileNames;
            }
            else
            {
                return;
            }

            if (fileNames != null && fileNames.Length > 0)
            {
                InstallCustomPackages(fileNames);
            }
        }

        internal void InstallCustomPackages(string[] files)
        {
            if (files != null && files.Length > 0)
            {
                var nonPkg = files.Where(it => !it.ToUpper().EndsWith(".PKG")).ToList();
                if (nonPkg.Count > 0)
                {
                    MessageBox.Show("The custom package installer can only process PKG files. Aborting.");
                    return;
                }
            }

            if (files.Length == 0)
            {
                return;
            }
            else if (files.Length == 1 && MessageBox.Show("You are about to install a custom package in your RTC installation. Any changes done by the package will overwrite files in the installation.\n\nDo you wish to continue?", "Custom packge install", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
            else if (files.Length > 1 && MessageBox.Show("You are about to install multiple custom packages in your RTC installation. Any changes done by the packages will overwrite files in the installation.\n\nDo you wish to continue?", "Custom packge install", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            var versionFolder = lc.VersionLocation;
            foreach (var file in files)
            {
                try
                {
                    using (ZipArchive archive = ZipFile.OpenRead(file))
                    {
                        foreach (var entry in archive.Entries)
                        {
                            var entryPath = Path.Combine(versionFolder, entry.FullName).Replace("/", "\\");

                            if (entryPath.EndsWith("\\"))
                            {
                                if (!Directory.Exists(entryPath))
                                {
                                    Directory.CreateDirectory(entryPath);
                                }
                            }
                            else
                            {
                                entry.ExtractToFile(entryPath, true);
                            }
                        }
                    }

                    //System.IO.Compression.ZipFile.ExtractToDirectory(file, versionFolder,);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during extraction and your RTC installation is possibly corrupted. \n\nYou may need to delete your RTC installation and reinstall it from the launcher. To do so, you can right click the version on the left side panel and select Delete from the menu.\n\nIf you need to backup any downloaded emulator to keep configurations or particular setups, you will find the content to backup by right clicking the card and selecting Open Folder.\n\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                MainForm.mf.RefreshPanel();
            }
        }

        private void AddButton_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

            var fd = (string[])e.Data.GetData(DataFormats.FileDrop); //file drop

            InstallCustomPackages(fd);
        }

        private void AddButton_MouseDown(object sender, MouseEventArgs e)
        {
            InstallCustomPackages();

            return;
            var button = (sender as Control);
            var categoryPanel = button.Parent;
            var panelV6 = categoryPanel.Parent;

            int absoluteX = e.Location.X + button.Location.X + categoryPanel.Location.X + panelV6.Location.X;
            int absoluteY = e.Location.Y + button.Location.Y + categoryPanel.Location.Y + panelV6.Location.Y;

            var locate = new Point(absoluteX, absoluteY);

            var columnsMenu = new BuildContextMenu();

            var allControls = new List<Control>();

            allControls.AddRange(flowVisiblePanel.Controls.Cast<Control>());
            allControls.AddRange(HiddenButtons);

            foreach (var ctrl in allControls)
            {
                if (ctrl is Button btn)
                {
                    if (btn.Tag is LauncherConfJsonItemV6 lcji && lcji.FolderName != null)
                    {
                        var AddonInstalled = Directory.Exists(Path.Combine(lc.VersionLocation, lcji.FolderName));

                        if (lcji.HideItem && !AddonInstalled)
                        {
                            columnsMenu.Items.Add(lcji.ItemName, null, new EventHandler((ob, ev) => btnBatchfile_Click(btn, e)));
                        }
                    }
                }
            }

            if (columnsMenu.Items.Count == 0)
            {
                columnsMenu.Items.Add("No available addons", null, new EventHandler((ob, ev) => { })).Enabled = false;
            }

            columnsMenu.Items.Add(new ToolStripSeparator());
            columnsMenu.Items.Add("Load Custom Package..", null, new EventHandler((ob, ev) => InstallCustomPackages()));

            var title = new ToolStripMenuItem("Extra addons for this RTC version")
            {
                Enabled = false
            };
            var sep = new ToolStripSeparator();

            columnsMenu.Items.Insert(0, sep);
            columnsMenu.Items.Insert(0, title);

            columnsMenu.Show(this, locate);
        }

        private void AddButton_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void SidebarCloseTimer_Tick(object sender, EventArgs e)
        {
            sidebarCloseTimer.Stop();

            MainForm.sideinfoForm.Hide();
            MainForm.sideversionForm.Show();
        }

        private void NewButton_MouseLeave(object sender, EventArgs e)
        {
            var currentButton = (Button)sender;
            var lcji = (LauncherConfJsonItemV6)currentButton.Tag;

            if (!string.IsNullOrWhiteSpace(lcji.ItemName))
            {
                sidebarCloseTimer.Stop();
                sidebarCloseTimer.Start();

                currentButton.FlatAppearance.BorderSize = 0;

                string name = (lcji.ItemName ?? string.Empty);
                string subtitle = (lcji.ItemSubtitle ?? string.Empty);

                if (!string.IsNullOrWhiteSpace(subtitle) && name.Contains(subtitle))
                    name = name.Replace(subtitle, string.Empty);

                string description = (lcji.ItemDescription ?? string.Empty);

                MainForm.sideinfoForm.lbName.Text = name;
                MainForm.sideinfoForm.lbSubtitle.Text = subtitle;
                MainForm.sideinfoForm.lbDescription.Text = description;
            }
        }

        private void NewButton_MouseEnter(object sender, EventArgs e)
        {
            var currentButton = (Button)sender;
            var lcji = (LauncherConfJsonItemV6)currentButton.Tag;

            if (!string.IsNullOrWhiteSpace(lcji.ItemName))
            {
                sidebarCloseTimer.Stop();

                currentButton.FlatAppearance.BorderColor = Color.Gray;
                currentButton.FlatAppearance.BorderSize = 1;

                string itemClass = $"{(lcji.ItemClass ?? string.Empty)}";
                string name = $"{(lcji.ItemName ?? string.Empty)}";
                string subtitle = $"{(lcji.ItemSubtitle ?? string.Empty)}";

                bool isVanguard = subtitle.ToUpper().Contains("VANGUARD");

                string describer;
                switch (itemClass)
                {
                    case "TIER2":
                        describer = "(Beta)";
                        break;
                    case "TIER1":
                        describer = "(Prototype)";
                        break;
                    case "Compatibility":
                        describer = "(Compatibility)";
                        break;
                    default:
                        describer = string.Empty;
                        break;
                }

                if (!string.IsNullOrWhiteSpace(subtitle) && name.Contains(subtitle))
                    name = name.Replace(subtitle, string.Empty);

                string description = (lcji.ItemDescription ?? string.Empty);

                MainForm.sideinfoForm.lbName.Text = name;
                MainForm.sideinfoForm.lbSubtitle.Text = subtitle;

                MainForm.sideinfoForm.lbDescription.Text = description;

                MainForm.sideinfoForm.Show();
                MainForm.sideversionForm.Hide();
            }
        }

        internal void DeleteAddon(LauncherConfJsonItemV6 lcji)
        {
            var AddonFolderName = lcji.FolderName;
            var targetFolder = Path.Combine(MainForm.versionsDir, lc.Version, AddonFolderName);

            if (Directory.Exists(targetFolder))
            {
                string CustomPackage = null;

                if (lcji.IsAddon)
                {
                    CustomPackage = "This addon is a Custom Package\n\n";
                }

                if (MessageBox.Show(CustomPackage + "Deleting this addon will also wipe the configuration and temporary files that it contains.\n\nDo you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                if (lcji.IsAddon)
                {
                    var ImageFilename = Path.Combine(MainForm.versionsDir, "Launcher", lcji.ImageName);

                    if (File.Exists(lcji.ConfigFilename))
                    {
                        File.Delete(lcji.ConfigFilename);
                    }

                    if (File.Exists(ImageFilename))
                    {
                        File.Delete(ImageFilename);
                    }
                }

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
                    DeleteAddon(lcji);
                    return;
                }
            }

            MainForm.RefreshKeepSelectedVersion();
            //MainForm.mf.RefreshInterface();
        }

        private void LaunchPanelV6_Load(object sender, EventArgs e) => DrawPanel();

        private void btnBatchfile_Click(object sender, EventArgs e)
        {
            var currentButton = (Button)sender;

            var lcji = (LauncherConfJsonItemV6)currentButton.Tag;

            if (!string.IsNullOrEmpty(lcji.FolderName) && !Directory.Exists(Path.Combine(lc.VersionLocation, lcji.FolderName)))
            {
                LauncherConfJsonV6 lcCandidateForPull = getFolderFromPreviousVersion(lcji.DownloadVersion);
                if (lcCandidateForPull != null)
                {
                    var resultAskPull = MessageBox.Show($"The component {lcji.FolderName} could be imported from {lcCandidateForPull.Version}\nDo you wish import it?", "Import candidate found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultAskPull == DialogResult.Yes)
                    {
                        LauncherConfJsonItemV6 candidate = lcCandidateForPull.Items.FirstOrDefault(it => it.DownloadVersion == lcji.DownloadVersion);
                        //handle it here
                        try
                        {
                            RTC_Extensions.RecursiveCopyNukeReadOnly(new DirectoryInfo(Path.Combine(lcCandidateForPull.VersionLocation, candidate.FolderName)), new DirectoryInfo(Path.Combine(lc.VersionLocation, lcji.FolderName)));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Couldn't copy {Path.Combine(lcCandidateForPull.VersionLocation, candidate?.FolderName ?? "NULL") ?? "NULL"} to {lcji.FolderName}.\nIs the file in use?\nException:{ex.Message}");
                            try
                            {
                                RTC_Extensions.RecursiveDeleteNukeReadOnly(new DirectoryInfo(Path.Combine(lc.VersionLocation, lcji.FolderName)));
                            }
                            catch (Exception _ex) //f
                            {
                                Console.WriteLine(_ex);
                            }
                            return;
                        }
                        try
                        {
                            RTC_Extensions.RecursiveDeleteNukeReadOnly(new DirectoryInfo(Path.Combine(lcCandidateForPull.VersionLocation, candidate.FolderName)));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to delete old version {Path.Combine(lcCandidateForPull.VersionLocation, candidate?.FolderName ?? "NULL") ?? "NULL"}. Is the file in use?\nException:{ex.Message}");
                            return;
                        }
                        MainForm.RefreshKeepSelectedVersion();
                        return;
                    }
                }

                if (lcji.IsAddon)
                {
                    MessageBox.Show("This is a card for a missing Custom Package. You can reinstall the package with the PKG file or delete this addon.", "Missing folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = MessageBox.Show($"The following component is missing: {lcji.FolderName}\nDo you wish to download it?", "Additional download required", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var correctServer = MainForm.GetCorrectServer(lc.VersionLocation);
                    if (correctServer >= 0)
                    {
                        MainForm.UpdateSelectedServer(correctServer);
                    }

                    var downloadUrl = $"{MainForm.webResourceDomain}/rtc/addons/" + lcji.DownloadVersion + ".zip";
                    var downloadedFile = Path.Combine(MainForm.launcherDir, "PACKAGES", lcji.DownloadVersion + ".zip");
                    var extractDirectory = Path.Combine(lc.VersionLocation, lcji.FolderName);




                    MainForm.DownloadFile(new Uri(downloadUrl), downloadedFile, extractDirectory);
                }

                return;
            }

            bool checkForRTC = lcji.ExecutableCommands.Values
                .SelectMany(d => d.PreExecuteCommands)
                .Any(c => c != null && c.FileName.Contains("StandaloneRTC"));


            if (checkForRTC && Program.StandaloneRTCProcess != null && !Program.StandaloneRTCProcess.HasExited)
            {
                var form = new RTCOpenForm(lcji.FolderName);
                if (form.ShowDialog() == DialogResult.No)
                    return;

                var message = new { Type = "UI|Remote_SwapImplementation", objectValue = new object[] { lcji.FolderName, null } };
                string jsonString = JsonConvert.SerializeObject(message);
                byte[] sendBytes = Encoding.UTF8.GetBytes(jsonString);

                string IPAddress = "127.0.0.1";
                int port = 42069;

                UdpClient udpClient = new UdpClient(IPAddress, port);

                try
                {
                    udpClient.Send(sendBytes, sendBytes.Length);
                    Console.WriteLine($"UDP packet {jsonString} sent to {IPAddress} {port}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending UDP packet: {ex.ToString()}");
                }
                finally
                {
                    udpClient.Close();
                }
            }
            else
                lcji.Execute();
        }

        private static LauncherConfJsonV6 getFolderFromPreviousVersion(string downloadVersion)
        {
            foreach (var ver in MainForm.sideversionForm.lbVersions.Items.Cast<string>())
            {
                if (downloadVersion == ver)
                {
                    continue;
                }

                var _lc = new LauncherConfJsonV6(ver);
                LauncherConfJsonItemV6 lcji = _lc.Items.FirstOrDefault(it => it.DownloadVersion == downloadVersion);
                if (lcji != null)
                {
                    if (Directory.Exists(Path.Combine(_lc.VersionLocation, lcji.FolderName)))
                    {
                        return _lc;
                    }
                }
            }

            return null;
        }

        public LauncherConfJsonV6 GetLauncherJsonConf()
        {
            return lc;
        }

        public string GetFolderByVersion(string versionFolderName)
        {
            var jsonConf = this.GetLauncherJsonConf();
            var addonFolderName = jsonConf.Items.FirstOrDefault(it => it.DownloadVersion == versionFolderName)?.FolderName;
            return addonFolderName;
        }

        private void LaunchPanelV6_Resize(object sender, EventArgs e)
        {
            //flowVisiblePanel.MaximumSize = new Size(980, int.MaxValue);
        }

        private void flowVisiblePanel_SizeChanged(object sender, EventArgs e)
        {
            //foreach (var control in flowVisiblePanel.Controls)
            //    if(control is FlowLayoutPanel flp)
            //        flp.Size = new P
        }

        private void btnOnlineGuide_Click(object sender, EventArgs e)
        {
            Process.Start("https://corrupt.wiki/");
        }

        private void btnTutorials_Click(object sender, EventArgs e)
        {
            Process.Start("http://rtctutorialvideo.r5x.cc/");
        }

        private void btnDiscord_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.corrupt.wiki/");
        }

        private void flowVisiblePanel_MouseEnter(object sender, EventArgs e)
        {
            flowVisiblePanel.Focus();
        }
    }
}
