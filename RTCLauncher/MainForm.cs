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
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using RTCV.Launcher.Exceptions;

    internal partial class MainForm : Form
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        private const int HT_LEFT = 0xA;
        private const int HT_RIGHT = 0xB;
        private const int HT_TOP = 0xC;
        private const int HT_TOPLEFT = 0xD;
        private const int HT_TOPRIGHT = 0xE;
        private const int HT_BOTTOM = 0xF;
        private const int HT_BOTTOMLEFT = 0x10;
        private const int HT_BOTTOMRIGHT = 0x11;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        internal void SuggestInstallZip(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y + pnTopPanel.Height);

                var columnsMenu = new Components.BuildContextMenu();
                columnsMenu.Items.Add("Install from Zip file", null, new EventHandler((ob, ev) => InstallFromZip()));
                columnsMenu.Show(this, locate);
            }
        }

        internal static string launcherDir = Path.GetDirectoryName(Application.ExecutablePath);
        internal static string webResourceDomain = "http://redscientist.com/software";

        internal static MainForm mf = null;
        internal static VersionDownloadPanel vdppForm = null;
        internal static SidebarInfoPanel sideinfoForm = null;
        internal static SidebarVersionsPanel sideversionForm = null;

        internal static DownloadForm dForm = null;
        internal static Form lpForm = null;

        public const int launcherVer = 30;

        internal static int devCounter = 0;
        internal static string SelectedVersion = null;
        internal static string lastSelectedVersion = null;

        public MainForm()
        {
            InitializeComponent();

            mf = this;

            versionLabel.Text = "v" + launcherVer;

            var preAnchorLeftPanelSize = new Size(pnLeftSide.Width, pnLeftSide.Height - btnVersionDownloader.Height);

            sideversionForm = new SidebarVersionsPanel
            {
                BackColor = pnLeftSide.BackColor,
                TopLevel = false
            };
            pnLeftSide.Controls.Add(sideversionForm);
            sideversionForm.Location = new Point(0, 0);
            sideversionForm.Size = preAnchorLeftPanelSize;
            sideversionForm.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            sideversionForm.Show();

            sideinfoForm = new SidebarInfoPanel
            {
                BackColor = pnLeftSide.BackColor,
                TopLevel = false
            };
            pnLeftSide.Controls.Add(sideinfoForm);
            sideinfoForm.Location = new Point(0, 0);
            sideinfoForm.Size = preAnchorLeftPanelSize;
            sideinfoForm.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            RewireMouseMove();

            //creating default folders
            if (!Directory.Exists(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar))
            {
                Directory.CreateDirectory(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar);
            }

            if (!Directory.Exists(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar))
            {
                Directory.CreateDirectory(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar);
            }

            if (File.Exists(launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\dev.txt"))
            {
                webResourceDomain = "http://cc.r5x.cc";
            }

            //Will trigger after an update from the original launcher
            if (Directory.Exists(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + "Update_Launcher"))
            {
                Directory.Delete(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + "Update_Launcher", true);
                if (File.Exists(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + "Update_Launcher.zip"))
                {
                    File.Delete(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + "Update_Launcher.zip");
                }
            }
        }

        private void RewireMouseMove()
        {
            foreach (Control control in Controls)
            {
                control.MouseMove -= RedirectMouseMove;
                control.MouseMove += RedirectMouseMove;
            }

            MouseMove -= MainForm_MouseMove;
            MouseMove += MainForm_MouseMove;
        }

        public static void DownloadFile(Uri downloadURL, string downloadedFile, string extractDirectory)
        {
            mf.clearAnchorRight();

            dForm = new DownloadForm(downloadURL, downloadedFile, extractDirectory);

            mf.pnLeftSide.Visible = false;

            mf.btnVersionDownloader.Enabled = false;

            dForm.TopLevel = false;
            dForm.Location = new Point(0, 0);
            dForm.Dock = DockStyle.Fill;
            mf.Controls.Add(dForm);
            dForm.Show();
            dForm.Focus();
            dForm.BringToFront();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshInstalledVersions();

            if (sideversionForm.lbVersions.Items.Count > 0)
            {
                sideversionForm.lbVersions.SelectedIndex = 0;
            }

            try
            {
                Action a = () =>
                {
                    var motdFile = GetFileViaHttp(new Uri($"{webResourceDomain}/rtc/releases/MOTD.txt"));
                    var motd = string.Empty;
                    if (motdFile == null)
                    {
                        motd = "Couldn't load the RTC MOTD from Redscientist.com";
                    }
                    else
                    {
                        motd = Encoding.UTF8.GetString(motdFile);
                    }

                    Invoke(new MethodInvoker(() => lbMOTD.Text = motd));
                };
                Task.Run(a);
            }
            catch
            {
                lbMOTD.Text = "Couldn't load the RTC MOTD from Redscientist.com";
                MessageBox.Show("Couldn't connect to the server.");
            }

            lbMOTD.Visible = true;
            SetRTCColor(Color.FromArgb(120, 180, 155));
        }

        public void SetRTCColor(Color color, Form form = null)
        {
            //Recolors all the RTC Forms using the general skin color

            var allControls = new List<Control>();

            if (form == null)
            {
                allControls.AddRange(Controls.getControlsWithTag());
                allControls.Add(this);
            }
            else
            {
                allControls.AddRange(form.Controls.getControlsWithTag());
            }

            List<Control> lightColorControls = allControls.FindAll(it => (it.Tag as string ?? string.Empty).Contains("color:light"));
            List<Control> normalColorControls = allControls.FindAll(it => (it.Tag as string ?? string.Empty).Contains("color:normal"));
            List<Control> darkColorControls = allControls.FindAll(it => (it.Tag as string ?? string.Empty).Contains("color:dark"));
            List<Control> darkerColorControls = allControls.FindAll(it => (it.Tag as string ?? string.Empty).Contains("color:darker"));

            foreach (Control c in lightColorControls)
            {
                c.BackColor = color.ChangeColorBrightness(0.30f);
            }

            foreach (Control c in normalColorControls)
            {
                c.BackColor = color;
            }

            //spForm.dgvStockpile.BackgroundColor = color;
            //ghForm.dgvStockpile.BackgroundColor = color;

            foreach (Control c in darkColorControls)
            {
                c.BackColor = color.ChangeColorBrightness(-0.30f);
            }

            foreach (Control c in darkerColorControls)
            {
                c.BackColor = color.ChangeColorBrightness(-0.75f);
            }
        }

        public void RefreshInstalledVersions()
        {
            sideversionForm.lbVersions.Items.Clear();
            var versions = new List<string>(Directory.GetDirectories(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar));
            sideversionForm.lbVersions.Items.AddRange(versions.OrderByNaturalDescending(x => x).Select(it => getFilenameFromFullFilename(it)).ToArray<object>());

            sideversionForm.lbDefaultText.Visible = versions.Count == 0;
            sideversionForm.lbVersions.Visible = versions.Count > 0;

            PerformLayout();
            SelectedVersion = null;

            Action a = () =>
            {
                var latestVersion = VersionDownloadPanel.getLatestVersion();
                Invoke(new MethodInvoker(() => pbNewVersionNotification.Visible = !versions.Select(it => it.Substring(it.LastIndexOf('\\') + 1)).Contains(latestVersion)));
            };
            Task.Run(a);
        }

        public void RefreshPanel()
        {
            sideversionForm.lbVersions.SelectedIndex = -1;

            RefreshInstalledVersions();

            mf.pnLeftSide.Visible = true;

            if (vdppForm != null)
            {
                vdppForm.lbOnlineVersions.SelectedIndex = -1;
                vdppForm.btnDownloadVersion.Visible = false;
            }

            RefreshKeepSelectedVersion();
        }

        public static byte[] GetFileViaHttp(Uri url)
        {
            //Windows does the big dumb: part 11
            WebRequest.DefaultWebProxy = null;

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(60000);
                byte[] b = null;
                try
                {
                    b = client.GetByteArrayAsync(url).Result;
                }
                catch (AggregateException ex)
                {
                    Console.WriteLine($"{url} timed out.");
                    _ = ex;
                }

                return b;
            }
        }

        internal static string getFilenameFromFullFilename(string fullFilename)
        {
            return fullFilename.Substring(fullFilename.LastIndexOf('\\') + 1);
        }

        internal static string removeExtension(string filename)
        {
            return filename.Substring(0, filename.LastIndexOf('.'));
        }

        public void lbVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearAnchorRight();
            if (sideversionForm.lbVersions.SelectedIndex == -1)
            {
                SelectedVersion = null;
                return;
            }
            else
            {
                SelectedVersion = sideversionForm.lbVersions.SelectedItem.ToString();
                lastSelectedVersion = SelectedVersion;
            }

            if (File.Exists(Path.Combine(launcherDir, "VERSIONS", SelectedVersion, "Launcher", "launcher.json")))
            {
                lpForm = new LaunchPanelV3();
            }
            else if (File.Exists(Path.Combine(launcherDir, "VERSIONS", SelectedVersion, "Launcher", "launcher.ini")))
            {
                lpForm = new LaunchPanelV2();
            }
            else
            {
                lpForm = new LaunchPanelV1();
            }

            lpForm.Size = pnAnchorRight.Size;
            lpForm.TopLevel = false;
            pnAnchorRight.Controls.Add(lpForm);
            foreach (Control c in lpForm.Controls)
            {
                c.MouseMove += MainForm_MouseMove;
            }

            lpForm.MouseMove += MainForm_MouseMove;

            lpForm.Dock = DockStyle.Fill;
            lpForm.Show();
        }

        public void InvokeUI(Action a)
        {
            BeginInvoke(new MethodInvoker(a));
        }

        private static void UpdateLauncher(string extractDirectory)
        {
            var batchLocation = extractDirectory + Path.DirectorySeparatorChar + "Launcher\\update.bat";
            var psi = new ProcessStartInfo
            {
                FileName = Path.GetFileName(batchLocation),
                WorkingDirectory = Path.GetDirectoryName(batchLocation),
                Arguments = Path.GetFileName(Application.ExecutablePath)
            };
            Process.Start(psi);
            Environment.Exit(0);
        }

        private static void ValidateExtractedFiles(string downloadedFile, string extractDirectory)
        {
            //This checks every extracted files against the contents of the zip file
            using (ZipArchive za = ZipFile.OpenRead(downloadedFile))
            {
                var foundLockBefore = false; //this flag prompts a message to skip all
                var skipLock = false; //file locked messages and sents the flag below

                foreach (ZipArchiveEntry entry in za.Entries.Where(it => !it.FullName.EndsWith("/")))
                {
                    var targetFile = Path.Combine(extractDirectory, entry.FullName.Replace("/", "\\"));
                    if (File.Exists(targetFile))
                    {
                        var ext = entry.FullName.ToUpper().Substring(entry.FullName.Length - 3);
                        if (ext == "EXE" || ext == "DLL")
                        {
                            FileStream readCheck = null;
                            try
                            {
                                readCheck = File.OpenRead(targetFile); //test if file can be read
                                foundLockBefore = true;
                            }
                            catch
                            {
                                if (!skipLock)
                                {
                                    if (foundLockBefore)
                                    {
                                        if (MessageBox.Show($"Another file has been found locked/inaccessible.\nThere might be many more messages like this coming up.\n\nWould you like skip any remaining lock messages?", "Error",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                                        {
                                            skipLock = true;
                                        }
                                    }
                                }

                                if (!skipLock)
                                {
                                    MessageBox.Show($"An error occurred during extraction,\n\nThe file \"targetFile\" seems to have been locked/made inaccessible by an external program. It might be caused by your antivirus.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }

                            readCheck?.Close(); //close file immediately
                        }
                    }
                    else
                    {
                        MessageBox.Show($"An error occurred during extraction, rolling back changes.\n\nThe file \"{targetFile}\" could not be found. It might have been deleted by your antivirus.", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        if (Directory.Exists(extractDirectory))
                        {
                            RTC_Extensions.RecursiveDeleteNukeReadOnly(extractDirectory);
                        }
                    }
                }
            }
        }

        internal void DownloadComplete(string downloadedFile, string extractDirectory)
        {
            try
            {
                if (!Directory.Exists(extractDirectory))
                {
                    Directory.CreateDirectory(extractDirectory);
                }

                try
                {
                    ZipFile.ExtractToDirectory(downloadedFile, extractDirectory);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during extraction, rolling back changes.\n\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (Directory.Exists(extractDirectory))
                    {
                        RTC_Extensions.RecursiveDeleteNukeReadOnly(extractDirectory);
                    }

                    return;
                }

                ValidateExtractedFiles(downloadedFile, extractDirectory);

                if (File.Exists(downloadedFile))
                {
                    File.Delete(downloadedFile);
                }

                LaunchPrereqCheckerIfPresent(extractDirectory);

                try
                {
                    CheckForNeededLauncherUpdate(extractDirectory);
                }
                catch (LauncherUpdateRequiredException)
                {
                    return;
                }
            }
            finally
            {
                sideversionForm.lbVersions.SelectedIndex = -1;

                RefreshInstalledVersions();

                mf.pnLeftSide.Visible = true;

                if (vdppForm != null)
                {
                    vdppForm.lbOnlineVersions.SelectedIndex = -1;
                    vdppForm.btnDownloadVersion.Visible = false;
                }

                dForm.Close();
                dForm = null;

                RefreshKeepSelectedVersion();
            }
        }

        private static void LaunchPrereqCheckerIfPresent(string extractDirectory)
        {
            var preReqChecker = Path.Combine(extractDirectory, "Launcher", "PrereqChecker.exe");
            if (File.Exists(preReqChecker))
            {
                var psi = new ProcessStartInfo
                {
                    FileName = Path.GetFileName(preReqChecker),
                    WorkingDirectory = Path.GetDirectoryName(preReqChecker)
                };
                Process.Start(psi)?.WaitForExit();
            }
        }

        private static void CheckForNeededLauncherUpdate(string extractDirectory)
        {
            if (File.Exists(Path.Combine(extractDirectory, "Launcher", "ver.ini")))
            {
                var newVer = Convert.ToInt32(File.ReadAllText(Path.Combine(extractDirectory, "Launcher", "ver.ini")));
                if (newVer > launcherVer)
                {
                    if (File.Exists(Path.Combine(extractDirectory, "Launcher", "minver.ini")) && //Do we have minver
                        Convert.ToInt32(File.ReadAllText(Path.Combine(extractDirectory, "Launcher", "minver.ini"))) > launcherVer) //Is minver > launcherVer
                    {
                        if (MessageBox.Show("A mandatory launcher update is required to use this version. Click \"OK\" to update the launcher.",
                            "Launcher update required",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly) == DialogResult.OK)
                        {
                            UpdateLauncher(extractDirectory);
                        }
                        else
                        {
                            MessageBox.Show("Launcher update is required. Cancelling.");
                            RTC_Extensions.RecursiveDeleteNukeReadOnly(extractDirectory);
                            throw new LauncherUpdateRequiredException();
                        }
                    }

                    if (MessageBox.Show("The downloaded package contains a new launcher update.\n\nDo you want to update the Launcher?", "Launcher update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        UpdateLauncher(extractDirectory);
                    }
                }
            }
        }

        internal static void RefreshKeepSelectedVersion()
        {
            if (lastSelectedVersion != null)
            {
                var index = -1;
                for (var i = 0; i < sideversionForm.lbVersions.Items.Count; i++)
                {
                    var item = sideversionForm.lbVersions.Items[i];
                    if (item.ToString() == lastSelectedVersion)
                    {
                        index = i;
                        break;
                    }
                }

                sideversionForm.lbVersions.SelectedIndex = -1;
                sideversionForm.lbVersions.SelectedIndex = index;
            }
        }

        internal void InstallFromZip(string[] files = null)
        {
            var versionLocation = Path.Combine(MainForm.launcherDir, "VERSIONS");

            if (files != null && files.Length > 0)
            {
                var nonPkg = files.Where(it => !it.ToUpper().EndsWith(".ZIP")).ToList();
                if (nonPkg.Count > 0)
                {
                    MessageBox.Show("The installer can only process ZIP files. Aborting.");
                    return;
                }
            }

            if (files == null || files.Length == 0)
            {
                var ofd = new OpenFileDialog
                {
                    DefaultExt = "zip",
                    Title = "Open Zip Package files",
                    Filter = "ZIP files|*.zip",
                    RestoreDirectory = true,
                    Multiselect = true
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    files = ofd.FileNames;
                }
                else
                {
                    return;
                }
            }
            else if (files.Length == 1 && MessageBox.Show("You are about to install a zip package in your RTC Launcher. If an install with the same name already exists, it will be deleted.\n\nDo you wish to continue?", "Zip packge install", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
            else if (files.Length > 1 && MessageBox.Show("You are about to install multiple zip packages in your RTC Launcher. If an install with the same name already exists, it will be deleted.\n\nDo you wish to continue?", "Zip packge install", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            foreach (var file in files)
            {
                try
                {
                    using (ZipArchive archive = ZipFile.OpenRead(file))
                    {
                        var rtcvFolderExists = archive.Entries.FirstOrDefault(it => it.FullName.Contains("RTCV")) != null;
                        var launcherFolderExists = archive.Entries.FirstOrDefault(it => it.FullName.Contains("Launcher")) != null;
                        var versionFolderName = Path.GetFileNameWithoutExtension(file);

                        string versionFolderPath;

                        if (rtcvFolderExists && launcherFolderExists)
                        {
                            //this is a main RTCV Install
                            versionFolderPath = Path.Combine(versionLocation, versionFolderName);
                        }
                        else
                        {
                            if (sideversionForm.lbVersions.SelectedIndex == -1)
                            {
                                MessageBox.Show("Error: You cannot install an addon zip package without having a base install selected.");
                                return;
                            }

                            //this is most likely an emulator package
                            //in that case, use selected RTCV install path then fetch the requested emu path
                            if (!(lpForm is ILauncherJsonConfPanel))
                            {
                                MessageBox.Show("Error: Could not load Json config from base install");
                                return;
                            }

                            //fetching path segments
                            var rtcvBuildName = sideversionForm.lbVersions.SelectedItem.ToString();
                            var jsonConf = (lpForm as ILauncherJsonConfPanel).GetLauncherJsonConf();
                            var addonFolderName = jsonConf.Items.FirstOrDefault(it => it.DownloadVersion == versionFolderName)?.FolderName;

                            if (string.IsNullOrWhiteSpace(addonFolderName))
                            {
                                MessageBox.Show("Error: This addon is not meant for the selected base install");
                                return;
                            }

                            versionFolderPath = Path.Combine(versionLocation, rtcvBuildName, addonFolderName);
                        }

                        if (Directory.Exists(versionFolderPath))
                        {
                            DeleteSelected(versionFolderName);
                        }

                        if (!Directory.Exists(versionFolderPath))
                        {
                            Directory.CreateDirectory(versionFolderPath);
                        }

                        foreach (var entry in archive.Entries)
                        {
                            var entryPath = Path.Combine(versionFolderPath, entry.FullName).Replace("/", "\\");

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
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during extraction and your RTC installation is possibly corrupted. \n\nYou may need to delete your RTC installation and reinstall it from the launcher. To do so, you can right click the version on the left side panel and select Delete from the menu.\n\nIf you need to backup any downloaded emulator to keep configurations or particular setups, you will find the content to backup by right clicking the card and selecting Open Folder.\n\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                MainForm.mf.RefreshPanel();
            }
        }

        public void DeleteSelected(string version = null)
        {
            if (version == null && sideversionForm.lbVersions.SelectedIndex != -1)
            {
                version = sideversionForm.lbVersions.SelectedItem.ToString();
            }

            if (version == null)
            {
                return;
            }

            DialogResult result = MessageBox.Show($"Are you sure you want to delete version {version}?", "Build Deletion", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            if (result == DialogResult.Cancel)
            {
                return;
            }

            Directory.SetCurrentDirectory(launcherDir); //Move our working dir back

            if (File.Exists(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + version + ".zip"))
            {
                File.Delete(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + version + ".zip");
            }

            if (Directory.Exists(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version))
            {
                List<string> failed = RTC_Extensions.RecursiveDeleteNukeReadOnly(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version);
                if (failed.Count > 0)
                {
                    var sb = new StringBuilder();
                    foreach (var l in failed)
                    {
                        sb.AppendLine(Path.GetFileName(l));
                    }

                    MessageBox.Show($"Failed to delete some files!\nSomething may be locking them (is the RTC still running?)\n\nList of failed files:\n{sb}");
                }
            }

            RefreshInterface();
        }

        public void RefreshInterface()
        {
            sideversionForm.lbVersions.SelectedIndex = -1;
            RefreshInstalledVersions();
        }

        public static void OpenFolder()
        {
            if (sideversionForm.lbVersions.SelectedIndex == -1)
            {
                return;
            }

            var version = sideversionForm.lbVersions.SelectedItem.ToString();

            if (Directory.Exists(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version))
            {
                Process.Start(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version);
            }
        }

        internal void lbVersions_MouseDown(object sender, MouseEventArgs e)
        {
            var locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y + pnTopPanel.Height);

            if (sideversionForm.lbVersions.SelectedIndex == -1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    var columnsMenu = new Components.BuildContextMenu();
                    columnsMenu.Items.Add("Install from Zip file", null, new EventHandler((ob, ev) => InstallFromZip()));
                    columnsMenu.Show(this, locate);
                }
            }
            else
            {
                var version = sideversionForm.lbVersions.SelectedItem.ToString();
                if (!Directory.Exists(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    var columnsMenu = new Components.BuildContextMenu();
                    columnsMenu.Items.Add("Open Folder", null, new EventHandler((ob, ev) => OpenFolder()));
                    columnsMenu.Items.Add(new ToolStripSeparator());
                    columnsMenu.Items.Add("Delete", null, new EventHandler((ob, ev) => DeleteSelected()));
                    columnsMenu.Items.Add(new ToolStripSeparator());
                    columnsMenu.Items.Add("Install from Zip file", null, new EventHandler((ob, ev) => InstallFromZip()));
                    columnsMenu.Show(this, locate);
                }
            }
        }

        private void btnOnlineGuide_Click(object sender, EventArgs e)
        {
            Process.Start("https://corrupt.wiki/");
        }

        public void clearAnchorRight()
        {
            foreach (Control c in pnAnchorRight.Controls)
            {
                if (c is Form)
                {
                    pnAnchorRight.Controls.Remove(c);
                    (c as Form).Close();
                }
            }
        }

        private void btnVersionDownloader_Click(object sender, EventArgs e)
        {
            sideversionForm.lbVersions.SelectedIndex = -1;

            lastSelectedVersion = null;

            clearAnchorRight();

            vdppForm = new VersionDownloadPanel
            {
                TopLevel = false
            };
            pnAnchorRight.Controls.Add(vdppForm);
            vdppForm.Dock = DockStyle.Fill;
            vdppForm.Show();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lbRtcLauncher_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnQuit_MouseEnter(object sender, EventArgs e)
        {
            btnQuit.BackColor = Color.FromArgb(230, 46, 76);
        }

        private void btnQuit_MouseLeave(object sender, EventArgs e)
        {
            btnQuit.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void btnDiscord_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.corrupt.wiki/");
        }

        private const int grabBorderSize = 10; // size of the area where you can grab the borderless window

        private Rectangle RectTop => new Rectangle(0, 0, ClientSize.Width, grabBorderSize);
        private Rectangle RectLeft => new Rectangle(0, 0, grabBorderSize, ClientSize.Height);
        private Rectangle RectBottom => new Rectangle(0, ClientSize.Height - grabBorderSize, ClientSize.Width, grabBorderSize);

        private void label5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Real Time Corruptor is a Dynamic Corruptor for emulated games.\n" +
                            "It is a set of libraries that can be rigged up to various emulators and works by corrupting data into virtual memory chips of emulated systems.\n" +
                            "RTCV currently comes with implementations for Bizhawk, Dolphin, PCSX2, melonDS, and Citra.\n" +
                            "More information is available at https://redscientist.com/rtc \n\n" +
                            "RTC Launcher Software Third Party Licenses:\n\n" +
                            "Json.NET:" +
                            @"
Copyright(c) 2007 James Newton - King
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files(the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and / or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.",
                "About");
        }

        private void btnTutorials_Click(object sender, EventArgs e)
        {
            Process.Start("http://rtctutorialvideo.r5x.cc/");
        }

        private Rectangle RectRight => new Rectangle(ClientSize.Width - grabBorderSize, 0, grabBorderSize, ClientSize.Height);
        private static Rectangle RectTopLeft => new Rectangle(0, 0, grabBorderSize, grabBorderSize);
        private Rectangle RectTopRight => new Rectangle(ClientSize.Width - grabBorderSize, 0, grabBorderSize, grabBorderSize);
        private Rectangle RectBottomLeft => new Rectangle(0, ClientSize.Height - grabBorderSize, grabBorderSize, grabBorderSize);
        private Rectangle RectBottomRight => new Rectangle(ClientSize.Width - grabBorderSize, ClientSize.Height - grabBorderSize, grabBorderSize, grabBorderSize);

        private void ResizeWindow(MouseEventArgs e, int wParam)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, wParam, 0);
            }
        }

        private void RedirectMouseMove(object sender, MouseEventArgs e)
        {
            var control = (Control)sender;
            Point screenPoint = control.PointToScreen(new Point(e.X, e.Y));
            Point formPoint = PointToClient(screenPoint);
            var args = new MouseEventArgs(e.Button, e.Clicks,
                formPoint.X, formPoint.Y, e.Delta);
            OnMouseMove(args);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            Point cursor = PointToClient(Cursor.Position);
            if (RectTopLeft.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeNWSE;
                ResizeWindow(e, HT_TOPLEFT);
            }
            else if (RectTopRight.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeNESW;
                ResizeWindow(e, HT_TOPRIGHT);
            }
            else if (RectBottomLeft.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeNESW;
                ResizeWindow(e, HT_BOTTOMLEFT);
            }
            else if (RectBottomRight.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeNWSE;
                ResizeWindow(e, HT_BOTTOMRIGHT);
            }
            else if (RectTop.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeNS;
                ResizeWindow(e, HT_TOP);
            }
            else if (RectLeft.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeWE;
                ResizeWindow(e, HT_LEFT);
            }
            else if (RectRight.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeWE;
                ResizeWindow(e, HT_RIGHT);
            }
            else if (RectBottom.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeNS;
                ResizeWindow(e, HT_BOTTOM);
            }
            else if (pnTopPanel.ClientRectangle.Contains(cursor))
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
        }

        private void btnVersionDownloader_MouseDown(object sender, MouseEventArgs e) => SuggestInstallZip(sender, e);
    }
}
