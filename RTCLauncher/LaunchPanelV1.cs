namespace RTCV.Launcher
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    internal partial class LaunchPanelV1 : Form
    {
        private readonly Button[] buttons;

        public LaunchPanelV1()
        {
            InitializeComponent();

            buttons = new Button[]
            {
            btnBatchfile01,
            btnBatchfile02,
            btnBatchfile03,
            btnBatchfile04,
            btnBatchfile05,
            btnBatchfile06,
            btnBatchfile07,
            btnBatchfile08,
            btnBatchfile09,
            btnBatchfile10,
            };

            lbSelectedVersion.Visible = false;
            pnVersionBatchFiles.Visible = false;

            DisplayVersion();
        }

        public void DisplayVersion()
        {
            var folderPath = Path.Combine(MainForm.launcherDir, "VERSIONS", MainForm.SelectedVersion);
            if (!Directory.Exists(folderPath))
            {
                return;
            }

            var batchFiles = new List<string>(Directory.GetFiles(folderPath));
            var batchFileNames = new List<string>(batchFiles.Select(it => MainForm.removeExtension(MainForm.getFilenameFromFullFilename(it))));

            var isDefaultStartPresent = false;

            if (batchFileNames.Contains("START"))
            {
                batchFileNames.Remove("START");
                isDefaultStartPresent = true;
            }

            string startfilename = null;

            foreach (var file in batchFiles)
            {
                if (file.ToUpper().Contains(Path.DirectorySeparatorChar + "START.BAT"))
                {
                    startfilename = file;
                    break;
                }
            }

            if (startfilename != null)
            {
                batchFiles.Remove(startfilename);
            }

            foreach (Button btn in buttons)
            {
                btn.Visible = false;
            }

            if (batchFileNames.Count == 0)
            {
                lbError.Visible = true;
                return;
            }
            lbError.Visible = false;

            for (var i = 0; i < batchFileNames.Count; i++)
            {
                buttons[i].Visible = true;
                buttons[i].Text = batchFileNames[i];
                buttons[i].Tag = (buttons[i].Tag as string) + ";" + batchFiles[i];
            }
            btnStart.Visible = isDefaultStartPresent;

            lbSelectedVersion.Text = MainForm.SelectedVersion;
            lbSelectedVersion.Visible = true;
            pnVersionBatchFiles.Visible = true;

            new object();
        }

        private void OldLaunchPanel_Load(object sender, EventArgs e)
        {
        }

        private void btnBatchfile_Click(object sender, EventArgs e)
        {
            var currentButton = (Button)sender;

            //string version = MainForm.mf.lbVersions.SelectedItem.ToString();

            var version = MainForm.SelectedVersion;
            string fullPath;

            if (currentButton.Text == "START")
            {
                fullPath = MainForm.launcherDir + Path.DirectorySeparatorChar + "VERSIONS" + Path.DirectorySeparatorChar + version + Path.DirectorySeparatorChar + "START.bat";
            }
            else
            {
                fullPath = (currentButton.Tag as string).Split(';')[1];
            }

            var psi = new ProcessStartInfo
            {
                FileName = Path.GetFileName(fullPath),
                WorkingDirectory = Path.GetDirectoryName(fullPath)
            };
            Process.Start(psi);
        }
    }
}
