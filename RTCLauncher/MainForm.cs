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
        const int WS_MINIMIZEBOX = 0x20000;
        const int CS_DBLCLKS = 0x8;

        public static List<string> versions => new List<string>(Directory.GetDirectories(launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar));

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                cp.ClassStyle |= CS_DBLCLKS;
                return cp;
            }
        }

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

        internal static string launcherDir => Path.GetDirectoryName(Application.ExecutablePath);
        internal static string versionsDir => Path.Combine(launcherDir, "VERSIONS");
        internal static string packagesDir => Path.Combine(launcherDir, "PACKAGES");


        internal static string releaseServer = "http://redscientist.com/software";
        internal static string devServer = "http://cc.r5x.cc";
        internal static string historicalServer = "http://historical.optional.fun";
        internal static string stepbackServer = "http://redscientist.com/stepback";
        internal static string buildFolder = null;
        internal static string webResourceDomain = releaseServer;


        internal static MainForm mf = null;
        internal static VersionDownloadPanel vdppForm = null;
        internal static SidebarInfoPanel sideinfoForm = null;
        internal static SidebarVersionsPanel sideversionForm = null;

        internal static DownloadForm dForm = null;
        internal static Form lpForm = null;

        public const int launcherVer = 37;

        internal static string SelectedVersion = null;
        internal static string lastSelectedVersion = null;

        public MainForm()
        {
            InitializeComponent();

            mf = this;
            FormSync.SyncObject = this;

            versionLabel.Text = "v" + launcherVer;

            if (new DirectoryInfo(launcherDir).Name == "Launcher")
            {
                MessageBox.Show("This is the wrong file. Please run the Launcher.exe located in the same root directory where your VERSIONS and PACKAGES folder reside. If that exe is missing, you can copy this one to that location and run it from there.");
                Environment.Exit(-1);
            }

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
            if (!Directory.Exists(versionsDir))
                Directory.CreateDirectory(versionsDir);
            if (!Directory.Exists(packagesDir))
                Directory.CreateDirectory(packagesDir);

            if (File.Exists(Path.Combine(packagesDir, "dev.txt")))
                webResourceDomain = devServer;
            else if (File.Exists(Path.Combine(packagesDir, "historical.txt")))
                webResourceDomain = historicalServer;
            else if (File.Exists(Path.Combine(packagesDir, "stepback.txt")))
                webResourceDomain = stepbackServer;
            else
                webResourceDomain = releaseServer;

            if (File.Exists(Path.Combine(packagesDir, "build.txt")))
                buildFolder = File.ReadAllText(Path.Combine(packagesDir, "build.txt"));

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

                if (MainForm.webResourceDomain == MainForm.stepbackServer)
                {
                    if (File.Exists(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\stepback.txt"))
                        File.Delete(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\stepback.txt");

                    MainForm.webResourceDomain = MainForm.releaseServer;
                    MainForm.mf.RefreshMotd();
                }
            }
            else
            {
                if (versions.Any(it => it.Contains("STEPBACK")))
                    sideversionForm.category_Click(sideversionForm.btnStepback, null);
            }

            RefreshMotd();
            CheckUpdate();


            SetRTCColor(Color.FromArgb(120, 180, 155));
        }

        public void RefreshMotd()
        {
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

                    FormSync.FormExecute(() => {
                        lbMOTD.Text = motd;
                    });
                };
                Task.Run(a);
            }
            catch
            {
                lbMOTD.Text = "Couldn't load the RTC MOTD from Redscientist.com";
                MessageBox.Show("Couldn't connect to the server.");
            }

            lbMOTD.Visible = true;
        }

        public void CheckUpdate()
        {
            try
            {
                Action a = () =>
                {
                    var motdFile = GetFileViaHttp(new Uri($"https://redscientist.com/launcher/version.txt"));
                    var motd = string.Empty;
                    if (motdFile == null)
                    {
                        FormSync.FormExecute(() => {
                        btnUpdate.Visible = false;
                        });
                    }
                    else
                    {
                        var vers = Encoding.UTF8.GetString(motdFile);
                        var parsedvers = Convert.ToInt32(vers);
                        if (parsedvers > launcherVer)
                        {
                            FormSync.FormExecute(() => {
                                btnUpdate.Visible = true;
                            });
                        }
                        else
                        {
                            FormSync.FormExecute(() => {
                                btnUpdate.Visible = false;
                            });
                        }
                    }

                };
                Task.Run(a);
            }
            catch
            {
                btnUpdate.Visible = false;
            }

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

            var items = versions
                .Where(it => !it.Contains("STEPBACK"))
                .OrderByNaturalDescending(x => x)
                .Select(it => getFilenameFromFullFilename(it))
                .ToArray<object>();

            sideversionForm.lbVersions.Items.AddRange(items);

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

        public void AnchorPanel(Form form)
        {
            clearAnchorRight();

            form.Size = pnAnchorRight.Size;
            form.TopLevel = false;
            pnAnchorRight.Controls.Add(form);
            foreach (Control c in form.Controls)
            {
                c.MouseMove += MainForm_MouseMove;
            }

            form.MouseMove += MainForm_MouseMove;

            form.Dock = DockStyle.Fill;
            form.Show();
        }

        public void lbVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sideversionForm.lbVersions.SelectedIndex == -1)
            {
                SelectedVersion = null;
                clearAnchorRight();
                return;
            }
            else
            {
                SelectedVersion = sideversionForm.lbVersions.SelectedItem.ToString();
                lastSelectedVersion = SelectedVersion;
            }

            if (File.Exists(Path.Combine(launcherDir, "VERSIONS", SelectedVersion, "Launcher", "launcher.json")))
            {
                var panelIniPath = Path.Combine(launcherDir, "VERSIONS", SelectedVersion, "Launcher", "panel.ini");
                if (!File.Exists(panelIniPath))
                    lpForm = new LaunchPanelV3();
                else
                {
                    var panelVer = File.ReadAllText(panelIniPath).Trim();
                    switch (panelVer)
                    {
                        case "4":
                            lpForm = new LaunchPanelV4();
                            break;
                        case "5":
                            lpForm = new LaunchPanelV5();
                            break;
                    }
                }
            }
            else if (File.Exists(Path.Combine(launcherDir, "VERSIONS", SelectedVersion, "Launcher", "launcher.ini")))
            {
                lpForm = new LaunchPanelV2();
            }
            else
            {
                lpForm = new LaunchPanelV1();
            }

            AnchorPanel(lpForm);
        }

        public void InvokeUI(Action a)
        {
            BeginInvoke(new MethodInvoker(a));
        }

        private static void UpdateLauncher(string extractDirectory)
        {
            var batchLocation = Path.Combine(extractDirectory, "update.bat");
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
                            UpdateLauncher(Path.Combine(extractDirectory, "Launcher"));
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
                        UpdateLauncher(Path.Combine(extractDirectory, "Launcher"));
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

        internal void InstallFromZip(string[] files = null, bool ask = true)
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

            if (ask)
            {
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
            }

            string versionFolderPath;

            foreach (var file in files)
            {
                try
                {
                    using (ZipArchive archive = ZipFile.OpenRead(file))
                    {
                        var rtcvFolderExists = archive.Entries.FirstOrDefault(it => it.FullName.Contains("RTCV")) != null;
                        var launcherFolderExists = archive.Entries.FirstOrDefault(it => it.FullName.Contains("Launcher")) != null;
                        var versionFolderName = Path.GetFileNameWithoutExtension(file);

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
                            if (!(lpForm is ILauncherJsonConfPanelV3))
                            {
                                MessageBox.Show("Error: Could not load Json config from base install");
                                return;
                            }

                            //fetching path segments
                            var rtcvBuildName = sideversionForm.lbVersions.SelectedItem.ToString();
                            var addonFolderName = (lpForm as ILauncherJsonConfPanel).GetFolderByVersion(versionFolderName);

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
                                EnsureTreeExists(entryPath);

                                try
                                {
                                    entry.ExtractToFile(entryPath, true);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("ERROR DURING EXTRACTION: " + ex.Message);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during extraction and your RTC installation is possibly corrupted. \n\nYou may need to delete your RTC installation and reinstall it from the launcher. To do so, you can right click the version on the left side panel and select Delete from the menu.\n\nIf you need to backup any downloaded emulator to keep configurations or particular setups, you will find the content to backup by right clicking the card and selecting Open Folder.\n\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                CheckForNeededLauncherUpdate(versionFolderPath);

                MainForm.mf.RefreshPanel();
            }
        }

        private void EnsureTreeExists(string entryPath)
        {
            var dir = Path.GetDirectoryName(entryPath);
            var dirParts = dir.Split(Path.DirectorySeparatorChar);

            string build = null;
            foreach (var dirPart in dirParts)
            {
                if (build == null)
                    build = dirPart + @"\";
                else
                {
                    build = Path.Combine(build, dirPart);
                }

                if (!Directory.Exists(build))
                {
                    Directory.CreateDirectory(build);
                }
            }
        }

        public void DeleteSelected(string version = null, bool ask = true)
        {
            if (version == null && sideversionForm.lbVersions.SelectedIndex != -1)
            {
                version = sideversionForm.lbVersions.SelectedItem.ToString();
            }

            if (version == null)
            {
                return;
            }

            if (ask)
            {
                DialogResult result = MessageBox.Show($"Are you sure you want to delete version {version}?", "Build Deletion", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            Directory.SetCurrentDirectory(launcherDir); //Move our working dir back

            if (version != "UNSTABLE" && File.Exists(launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + version + ".zip"))
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

        public static void UpdateCore()
        {
            if (sideversionForm.lbVersions.SelectedIndex == -1)
            {
                return;
            }

            var version = sideversionForm.lbVersions.SelectedItem.ToString();

            string rtcvFolder = Path.Combine(launcherDir, "VERSIONS", version, "RTCV");
            if (Directory.Exists(buildFolder) && Directory.Exists(rtcvFolder))
            {
                var rootFiles = new DirectoryInfo(buildFolder).GetFiles();
                int count = 0;
                string files = string.Empty;

                foreach (var file in rootFiles)
                {
                    file.CopyTo(Path.Combine(rtcvFolder, file.Name), true);
                    count++;
                    files = $"{files} {file.Name}";
                }

                MessageBox.Show($"Updated {count} files \n {files}");
            }
        }

        public static void UpdatePlugins()
        {
            if (sideversionForm.lbVersions.SelectedIndex == -1)
            {
                return;
            }

            var version = sideversionForm.lbVersions.SelectedItem.ToString();

            string rtcvPluginsFolder = Path.Combine(launcherDir, "VERSIONS", version, "RTCV", "RTC", "Plugins");
            string buildPluginsFolder = Path.Combine(buildFolder, "RTC", "Plugins");

            if (Directory.Exists(buildPluginsFolder) && Directory.Exists(rtcvPluginsFolder))
            {
                var localPluginFiles = Directory.GetFiles(rtcvPluginsFolder);
                foreach (var local in localPluginFiles)
                    File.Delete(local);

                var rootFiles = new DirectoryInfo(buildPluginsFolder).GetFiles();
                int count = 0;
                string files = string.Empty;

                foreach (var file in rootFiles)
                {
                    file.CopyTo(Path.Combine(rtcvPluginsFolder, file.Name), true);
                    count++;
                    files = $"{files} {file.Name}";
                }

                MessageBox.Show($"Updated {count} files \n {files}");
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

                    if (buildFolder != null)
                    {
                        columnsMenu.Items.Add("Update RTCV Core from Build Folder", null, new EventHandler((ob, ev) => UpdateCore()));
                        columnsMenu.Items.Add("Pull RTCV Plugins from Build Folder", null, new EventHandler((ob, ev) => UpdatePlugins()));
                        columnsMenu.Items.Add(new ToolStripSeparator());
                    }

                    columnsMenu.Items.Add("Open Folder", null, new EventHandler((ob, ev) => OpenFolder()));
                    columnsMenu.Items.Add(new ToolStripSeparator());
                    columnsMenu.Items.Add("Delete", null, new EventHandler((ob, ev) => DeleteSelected()));
                    columnsMenu.Items.Add(new ToolStripSeparator());
                    columnsMenu.Items.Add("Install from Zip file", null, new EventHandler((ob, ev) => InstallFromZip()));
                    columnsMenu.Show(this, locate);
                }
            }
        }

        public void clearAnchorRight()
        {
            foreach (Control c in pnAnchorRight.Controls)
            {
                if (c is Form)
                {
                    pnAnchorRight.Controls.Remove(c);

                    (c as Form).Close();

                    //if (c is MusicPanel)
                    //{
                    //    //don't close it.
                    //}
                    //else if (c is GamesPanel)
                    //{
                    //    //don't close it.
                    //}
                    //else
                    //    (c as Form).Close();
                }
            }
        }

        public void btnVersionDownloader_Click(object sender, EventArgs e)
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


        private const int grabBorderSize = 16; // size of the area where you can grab the borderless window

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


        private Rectangle RectRight => new Rectangle(ClientSize.Width - grabBorderSize, 0, grabBorderSize, ClientSize.Height);
        private static Rectangle RectTopLeft => new Rectangle(0, 0, grabBorderSize, grabBorderSize);
        private Rectangle RectTopRight => new Rectangle(ClientSize.Width - grabBorderSize, 0, grabBorderSize, grabBorderSize);
        private Rectangle RectBottomLeft => new Rectangle(0, ClientSize.Height - grabBorderSize, grabBorderSize, grabBorderSize);
        private Rectangle RectBottomRight => new Rectangle(ClientSize.Width - (grabBorderSize * 3), ClientSize.Height - (grabBorderSize * 3), (grabBorderSize * 3), (grabBorderSize * 3));

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
            if (RectBottomRight.Contains(cursor))
            {
                Cursor.Current = Cursors.SizeNWSE;
                ResizeWindow(e, HT_BOTTOMRIGHT);
            }
            else if (RectTopLeft.Contains(cursor))
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

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Normal;
            else if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
        }

        private void btnMaximize_MouseEnter(object sender, EventArgs e)
        {
            btnMaximize.BackColor = Color.FromArgb(230, 46, 76);
        }

        private void btnMaximize_MouseLeave(object sender, EventArgs e)
        {
            btnMaximize.BackColor = Color.FromArgb(230, 46, 76);
        }

        private void pnTopPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnMaximize_Click(sender, e);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string target = Path.Combine(packagesDir, "update.bat");

                if (File.Exists(target))
                    File.Delete(target);

                WebClient wc = new WebClient();
                wc.DownloadFile("https://redscientist.com/launcher/update.bat", target);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not fetch update script. \n Attached stacktrace: \n {ex}");
                return;
            }

            try
            {
                string target = Path.Combine(packagesDir, "RTC_Launcher.exe");

                if (File.Exists(target))
                    File.Delete(target);

                WebClient wc = new WebClient();
                wc.DownloadFile("https://redscientist.com/launcher/RTC_Launcher.exe", target);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not fetch update executable. \n Attached stacktrace: \n {ex}");
                return;
            }

            try
            {
                UpdateLauncher(packagesDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"The final step of the update has failed. \n Attached stacktrace: \n {ex}");
                return;
            }
        }
    }
}
