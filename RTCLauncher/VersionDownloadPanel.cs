namespace RTCV.Launcher
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using RTCV.Launcher.Components;

    internal partial class VersionDownloadPanel : Form
    {
        internal string latestVersionString = " (Latest version)";
        private List<dynamic> onlineVersionsObjects = null;

        public static Color backgroundColor = Color.FromArgb(16, 16, 16);

        public VersionDownloadPanel()
        {
            InitializeComponent();

            ReloadPanel(true);
        }

        public bool ignoreServerChange = false;

        public void ReloadPanel(bool Init = false)
        {
            if (Init)
                ignoreServerChange = true;

            if (File.Exists(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\dev.txt"))
            {
                backgroundColor = Color.FromArgb(32, 16, 16);
                MainForm.webResourceDomain = MainForm.devServer;

                if (Init)
                    cbSelectedServer.SelectedIndex = 1;
            }
            else if (File.Exists(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\historical.txt"))
            {
                backgroundColor = Color.FromArgb(16, 16, 32);
                MainForm.webResourceDomain = MainForm.historicalServer;

                if (Init)
                    cbSelectedServer.SelectedIndex = 2;
            }
            else
            {
                backgroundColor = Color.FromArgb(16, 16, 16);
                MainForm.webResourceDomain = MainForm.releaseServer;

                if (Init)
                    cbSelectedServer.SelectedIndex = 0;
            }

            lbOnlineVersions.BackColor = backgroundColor;

            refreshVersions();
            MainForm.mf.RefreshMotd();

            if (Init)
                ignoreServerChange = false;
        }

        public static string getLatestVersion()
        {
            try
            {
                var versionFile = MainForm.GetFileViaHttp(new Uri($"{MainForm.webResourceDomain}/rtc/releases/version.php"));
                if (versionFile == null)
                {
                    return null;
                }

                var str = Encoding.UTF8.GetString(versionFile);
                var onlineVersions = new List<string>(str.Split('|').Where(it => !it.Contains("Launcher")).ToArray());

                var returnValue = onlineVersions.OrderByNaturalDescending(x => x).Select(it => it.Replace(".zip", string.Empty)).ToArray()[0];

                return returnValue;
            }
            catch
            {
                return null;
            }
        }

        public void refreshVersions()
        {
            Action a = () =>
            {
                var versionFile = MainForm.GetFileViaHttp(new Uri($"{MainForm.webResourceDomain}/rtc/releases/version.php"));

                if (versionFile == null)
                {
                    return;
                }

                var str = Encoding.UTF8.GetString(versionFile);

                //Ignores any build containing the word Launcher in it
                var onlineVersions = str.Split('|').Where(it => !it.Contains("Launcher")).OrderByNaturalDescending(x => x).Select(it => it.Replace(".zip", string.Empty)).ToArray();

                FormSync.FormExecute(() =>
                {
                    onlineVersionsObjects = new List<dynamic>();

                    lbOnlineVersions.DataSource = null;
                    lbOnlineVersions.DisplayMember = "Text";
                    lbOnlineVersions.ValueMember = "Value";

                    lbOnlineVersions.Items.Clear();
                    if (onlineVersions.Length > 0)
                    {
                        for (var i = 0; i < onlineVersions.Length; i++)
                        {
                            var value = onlineVersions[i];

                            if (i == 0)
                            {
                                onlineVersions[i] += latestVersionString;
                            }

                            var key = onlineVersions[i];

                            onlineVersionsObjects.Add(new { Text = key, Value = value });
                        }
                    }

                    lbOnlineVersions.DataSource = null;
                    lbOnlineVersions.DataSource = onlineVersionsObjects;

                    //lbOnlineVersions.Items.AddRange(onlineVersionsTuples);
                });
            };
            Task.Run(a);
        }

        private void lbOnlineVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbOnlineVersions.SelectedIndex == -1)
            {
                return;
            }

            btnDownloadVersion.Visible = true;
        }

        private void btnDownloadVersion_Click(object sender, EventArgs e)
        {
            if (lbOnlineVersions.SelectedIndex == -1)
            {
                return;
            }

            dynamic itemData = lbOnlineVersions.SelectedItem;

            string version = itemData.Value;
            version = version.Replace(latestVersionString, string.Empty);

            if (Directory.Exists(MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version))
            {
                if (MessageBox.Show($"The version {version} is already installed.\nThis will DELETE version {version} and redownload it.\n\nWould you like to continue?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                {
                    Directory.Delete(MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version, true);
                }
                else
                {
                    return;
                }
            }

            var downloadUrl = $"{MainForm.webResourceDomain}/rtc/releases/" + version + ".zip";
            var downloadedFile = MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES" + Path.DirectorySeparatorChar + version + ".zip";
            var extractDirectory = MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version;

            MainForm.lastSelectedVersion = version;

            MainForm.DownloadFile(new Uri(downloadUrl), downloadedFile, extractDirectory);
        }

        private void lbOnlineVersions_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            btnDownloadVersion_Click(sender, e);
        }

        private void lbOnlineVersions_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var locate = new Point(((Control)sender).Location.X + e.Location.X, ((Control)sender).Location.Y + e.Location.Y);

                var columnsMenu = new BuildContextMenu();
                columnsMenu.Items.Add("Download", null, new EventHandler((ob, ev) => btnDownloadVersion_Click(sender, e)));
                columnsMenu.Show(this, locate);
            }
        }

        private void btnOfflineInstall_Click(object sender, EventArgs e) => MainForm.mf.InstallFromZip();

        private void cbSelectedServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoreServerChange)
                return;

            if (File.Exists(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\dev.txt"))
                File.Delete(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\dev.txt");
            if (File.Exists(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\historical.txt"))
                File.Delete(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\historical.txt");

            switch (cbSelectedServer.SelectedIndex)
            {
                case 0: //release
                    break;
                case 1: //dev
                    File.WriteAllText(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\dev.txt", "DEV");
                    break;
                case 2: //historical
                    File.WriteAllText(MainForm.launcherDir + Path.DirectorySeparatorChar + "PACKAGES\\historical.txt", "HISTORICAL");
                    break;
                default:
                    return;
            }

            ReloadPanel();
        }
    }
}
