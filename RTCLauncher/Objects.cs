namespace RTCV.Launcher
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class LauncherConf
    {
        public string launcherAssetLocation;
        public readonly string launcherConfLocation;
        public string batchFilesLocation;
        public string version;

        public LauncherConfItem[] items = { };

        public LauncherConf(string _version)
        {
            version = _version;
            launcherAssetLocation = Path.Combine(MainForm.launcherDir, "VERSIONS" + Path.DirectorySeparatorChar + version + Path.DirectorySeparatorChar + "Launcher");
            launcherConfLocation = Path.Combine(launcherAssetLocation, "launcher.ini");
            batchFilesLocation = Path.Combine(MainForm.launcherDir, "VERSIONS", version);

            if (!File.Exists(launcherConfLocation))
            {
                return;
            }

            var confLines = File.ReadAllLines(launcherConfLocation);

            items = confLines.Select(it => new LauncherConfItem(this, it)).ToArray();
        }
    }

    public class LauncherConfItem
    {
        public string[] lineItems;
        public string imageLocation;
        public string batchName;
        public string batchLocation;
        public string folderName;
        public string folderLocation;
        public string downloadVersion;
        public string line;

        public LauncherConfItem(LauncherConf lc, string _line)
        {
            line = _line;
            lineItems = _line.Split('|');
            imageLocation = Path.Combine(lc.launcherAssetLocation, lineItems[0]);
            batchName = lineItems[1];
            batchLocation = Path.Combine(lc.batchFilesLocation, batchName);
            folderName = lineItems[2];
            folderLocation = Path.Combine(lc.batchFilesLocation, folderName);
            downloadVersion = lineItems[3];
        }
    }

    [SuppressMessage("Microsoft.Design", "CA1812: Avoid uninstantiated public classes", Justification = "Passed as a type parameter")]
    public class ExecutableCommand
    {
        public string DisplayName;
        public string FileName;
        public string Arguments;
        public bool WaitForExit;
        public int WaitForExitTimeout = int.MaxValue;
        [JsonConverter(typeof(StringEnumConverter))]
        public ProcessWindowStyle WindowStyle = ProcessWindowStyle.Normal;
        public List<ExecutableCommand> PreExecuteCommands = new List<ExecutableCommand>();
        public List<ExecutableCommand> PostExecuteCommands = new List<ExecutableCommand>();

        public ExecutableCommand(string displayName, string fileName, string arguments, bool waitForExit)
        {
            DisplayName = displayName;
            FileName = fileName;
            Arguments = arguments;
            WaitForExit = waitForExit;
        }

        public bool Execute(bool runPreExecute = true, bool runPostExecute = true)
        {
            var success = true;

            if (runPreExecute)
            {
                foreach (var exe in PreExecuteCommands)
                {
                    if (!exe.Execute())
                    {
                        Console.WriteLine($"Executing PreExecuteCommand {exe.DisplayName} failed!");
                    }
                }
            }

            var psi = new ProcessStartInfo
            {
                WindowStyle = WindowStyle,
                Arguments = Arguments,
                UseShellExecute = true
            };

            if (File.Exists(Path.GetFullPath(FileName)))
            {
                psi.FileName = Path.GetFullPath(FileName);
                psi.WorkingDirectory = Path.GetDirectoryName(Path.GetFullPath(FileName)) ?? string.Empty;
            }
            else
            {
                psi.FileName = FileName;
            }

            try
            {
                var p = Process.Start(psi);

                if (WaitForExit)
                {
                    success = p?.WaitForExit(WaitForExitTimeout) ?? false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Failed to start process {psi.FileName}.\nException: {e.Message}");
            }

            if (runPostExecute)
            {
                foreach (var exe in PostExecuteCommands)
                {
                    if (!exe.Execute())
                    {
                        Console.WriteLine($"Executing PostExecuteCommand {exe.DisplayName} failed!");
                    }
                }
            }

            return success;
        }
    }


    public class NoclickLabel : Label
    {
        public NoclickLabel()
        {
            this.SetStyle(ControlStyles.Selectable, false);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTTRANSPARENT = -1;

            if (m.Msg == WM_NCHITTEST)
            {
                m.Result = (IntPtr)HTTRANSPARENT;
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }


    #region JsonV3
    public class LauncherConfJsonV3
    {
        public string LauncherAssetLocation;
        public string LauncherConfLocation;
        public string[] LauncherAddonConfLocations;
        public string VersionLocation;
        public string Version;

        public LauncherConfJsonItemV3[] Items = { };

        public LauncherConfJsonV3(string _version)
        {
            Version = _version;
            LauncherAssetLocation = Path.Combine(MainForm.launcherDir, "VERSIONS", Version, "Launcher");
            LauncherConfLocation = Path.Combine(LauncherAssetLocation, "launcher.json");

            if (Directory.Exists(LauncherAssetLocation))
            {
                LauncherAddonConfLocations = Directory.GetFiles(LauncherAssetLocation).Where(it => it.Contains("addon_") && it.Contains(".json")).ToArray();
            }

            VersionLocation = Path.Combine(MainForm.launcherDir, "VERSIONS", Version);

            if (!File.Exists(LauncherConfLocation))
            {
                return;
            }

            Directory.SetCurrentDirectory(VersionLocation); //Move ourselves to this working directory

            var launcherJson = File.ReadAllText(LauncherConfLocation);
            var lcjiList = new List<LauncherConfJsonItemV3>();
            lcjiList.AddRange(JsonConvert.DeserializeObject<LauncherConfJsonItemV3[]>(launcherJson));

            foreach (var addonJsonConfLocation in LauncherAddonConfLocations)
            {
                try
                {
                    var addonJson = File.ReadAllText(addonJsonConfLocation);
                    var addonConfigs = JsonConvert.DeserializeObject<LauncherConfJsonItemV3[]>(addonJson);

                    foreach (var conf in addonConfigs)
                    {
                        conf.ConfigFilename = addonJsonConfLocation;
                        conf.IsAddon = true;
                    }

                    lcjiList.AddRange(addonConfigs);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not load addon json config file at\n{addonJsonConfLocation}\n\n{ex}\n\n{(ex.InnerException != null ? ex.InnerException.ToString() : string.Empty)}");
                    //eat it
                }
            }

            if (lcjiList[0].ItemSubtitle != null) //means we can use metadata to reorder in categories
            {
                var vanguardImplementations = lcjiList.Where(it => it.ItemSubtitle != null && it.ItemSubtitle.ToUpper().Contains("VANGUARD")).ToList();
                var stubVanguardImplementations = lcjiList.Where(it => it.ItemSubtitle != null && it.ItemSubtitle.ToUpper().Contains("STUB")).ToList();
                var everythingElse = lcjiList.Where(it => it.ItemSubtitle != null && !it.ItemSubtitle.ToUpper().Contains("STUB") && !it.ItemSubtitle.ToUpper().Contains("VANGUARD")).ToList();
                var addButton = lcjiList.FirstOrDefault(it => it.ImageName == "Add.png");

                lcjiList.Clear();
                lcjiList.AddRange(vanguardImplementations);
                lcjiList.AddRange(stubVanguardImplementations);
                lcjiList.AddRange(everythingElse);

                if (addButton != null)
                    lcjiList.Add(addButton);
            }

            Items = lcjiList.ToArray();
        }
    }

    [SuppressMessage("Microsoft.Design", "CA1812: Avoid uninstantiated public classes", Justification = "Passed as a type parameter")]
    public class LauncherConfJsonItemV3
    {
        [JsonProperty]
        public readonly string FolderName;
        [JsonProperty]
        public readonly string ImageName;
        [JsonProperty]
        public readonly string DownloadVersion;
        [JsonProperty]
        public readonly ReadOnlyDictionary<string, ExecutableCommand> ExecutableCommands;

        //Used for the sidepanel and ordering of cards
        [JsonProperty]
        public readonly string ItemName;
        [JsonProperty]
        public readonly string ItemSubtitle;
        [JsonProperty]
        public readonly string ItemDescription;

        [JsonProperty]
        public readonly bool HideItem; //makes the card hide

        //Addon vars that are automatically set when the json is loaded
        public bool IsAddon;
        public string ConfigFilename;

        public LauncherConfJsonItemV3(string imageName, string downloadVersion, string folderName, ReadOnlyDictionary<string, ExecutableCommand> executableCommands, string itemName, string itemSubtitle, string itemDescription, bool hideItem, bool isAddon, string configFilename)
        {
            ImageName = imageName;
            DownloadVersion = downloadVersion;
            FolderName = folderName;
            ExecutableCommands = executableCommands;

            ItemName = itemName;
            ItemSubtitle = itemSubtitle;
            ItemDescription = itemDescription;

            HideItem = hideItem;
            IsAddon = isAddon;
            ConfigFilename = configFilename;
        }

        public bool Execute(bool runPreExecute = true, bool runPostExecute = true)
        {
            var success = true;
            foreach (var e in ExecutableCommands.Values)
            {
                success = (e.Execute(runPreExecute, runPostExecute) & success);
            }

            return success;
        }

        public bool Execute(string key, bool runPreExecute = true, bool runPostExecute = true)
        {
            ExecutableCommands.TryGetValue(key, out ExecutableCommand e);
            return e?.Execute(runPreExecute, runPostExecute) ?? false;
        }
    }

    public interface ILauncherJsonConfPanelV3 : ILauncherJsonConfPanel
    {
        LauncherConfJsonV3 GetLauncherJsonConf();
    }

    #endregion


    #region JsonV4
    public class LauncherConfJsonV4
    {
        public string LauncherAssetLocation;
        public string LauncherConfLocation;
        public string[] LauncherAddonConfLocations;
        public string VersionLocation;
        public string Version;

        public LauncherConfJsonItemV4[] Items = { };

        public LauncherConfJsonV4(string _version)
        {
            Version = _version;
            LauncherAssetLocation = Path.Combine(MainForm.launcherDir, "VERSIONS", Version, "Launcher");
            LauncherConfLocation = Path.Combine(LauncherAssetLocation, "launcher.json");

            if (Directory.Exists(LauncherAssetLocation))
            {
                LauncherAddonConfLocations = Directory.GetFiles(LauncherAssetLocation).Where(it => it.Contains("addon_") && it.Contains(".json")).ToArray();
            }

            VersionLocation = Path.Combine(MainForm.launcherDir, "VERSIONS", Version);

            if (!File.Exists(LauncherConfLocation))
            {
                return;
            }

            Directory.SetCurrentDirectory(VersionLocation); //Move ourselves to this working directory

            var launcherJson = File.ReadAllText(LauncherConfLocation);
            var lcjiList = new List<LauncherConfJsonItemV4>();
            lcjiList.AddRange(JsonConvert.DeserializeObject<LauncherConfJsonItemV4[]>(launcherJson));

            foreach (var addonJsonConfLocation in LauncherAddonConfLocations)
            {
                try
                {
                    var addonJson = File.ReadAllText(addonJsonConfLocation);
                    var addonConfigs = JsonConvert.DeserializeObject<LauncherConfJsonItemV4[]>(addonJson);

                    foreach (var conf in addonConfigs)
                    {
                        conf.ConfigFilename = addonJsonConfLocation;
                        conf.IsAddon = true;
                        conf.ItemClass = "ADDON";
                    }

                    lcjiList.AddRange(addonConfigs);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not load addon json config file at\n{addonJsonConfLocation}\n\n{ex}\n\n{(ex.InnerException != null ? ex.InnerException.ToString() : string.Empty)}");
                    //eat it
                }
            }

            if (lcjiList[0].ItemSubtitle != null) //means we can use metadata to reorder in categories
            {
                var vanguardImplementations = lcjiList.Where(it => it.ItemSubtitle != null && it.ItemSubtitle.ToUpper().Contains("VANGUARD")).ToList();
                var stubVanguardImplementations = lcjiList.Where(it => it.ItemSubtitle != null && it.ItemSubtitle.ToUpper().Contains("STUB")).ToList();
                var everythingElse = lcjiList.Where(it => it.ItemSubtitle != null && !it.ItemSubtitle.ToUpper().Contains("STUB") && !it.ItemSubtitle.ToUpper().Contains("VANGUARD")).ToList();
                var addButton = lcjiList.FirstOrDefault(it => it.ImageName == "Add.png");

                lcjiList.Clear();
                lcjiList.AddRange(vanguardImplementations);
                lcjiList.AddRange(stubVanguardImplementations);
                lcjiList.AddRange(everythingElse);

                if (addButton != null)
                    lcjiList.Add(addButton);
            }

            Items = lcjiList.ToArray();
        }
    }

    [SuppressMessage("Microsoft.Design", "CA1812: Avoid uninstantiated public classes", Justification = "Passed as a type parameter")]
    public class LauncherConfJsonItemV4
    {
        [JsonProperty]
        public readonly string FolderName;
        [JsonProperty]
        public readonly string ImageName;
        [JsonProperty]
        public readonly string DownloadVersion;
        [JsonProperty]
        public readonly string FirmwareFolder;
        [JsonProperty]
        public readonly ReadOnlyDictionary<string, ExecutableCommand> ExecutableCommands;

        //Used for the sidepanel and ordering of cards
        [JsonProperty]
        public string ItemClass;
        [JsonProperty]
        public readonly string ItemName;
        [JsonProperty]
        public readonly string ItemSubtitle;
        [JsonProperty]
        public readonly string ItemDescription;

        [JsonProperty]
        public readonly bool HideItem; //makes the card hide

        //Addon vars that are automatically set when the json is loaded
        public bool IsAddon;
        public string ConfigFilename;

        public LauncherConfJsonItemV4(string imageName, string downloadVersion, string folderName, ReadOnlyDictionary<string, ExecutableCommand> executableCommands, string itemName, string itemSubtitle, string itemDescription, bool hideItem, bool isAddon, string configFilename)
        {
            ImageName = imageName;
            DownloadVersion = downloadVersion;
            FolderName = folderName;
            ExecutableCommands = executableCommands;

            ItemName = itemName;
            ItemSubtitle = itemSubtitle;
            ItemDescription = itemDescription;

            HideItem = hideItem;
            IsAddon = isAddon;
            ConfigFilename = configFilename;
        }

        public bool Execute(bool runPreExecute = true, bool runPostExecute = true)
        {
            var success = true;
            foreach (var e in ExecutableCommands.Values)
            {
                success = (e.Execute(runPreExecute, runPostExecute) & success);
            }

            return success;
        }

        public bool Execute(string key, bool runPreExecute = true, bool runPostExecute = true)
        {
            ExecutableCommands.TryGetValue(key, out ExecutableCommand e);
            return e?.Execute(runPreExecute, runPostExecute) ?? false;
        }
    }

    public interface ILauncherJsonConfPanelV4 : ILauncherJsonConfPanel
    {
        LauncherConfJsonV4 GetLauncherJsonConf();
    }

    public interface ILauncherJsonConfPanel
    {
        string GetFolderByVersion(string version);
    }

    #endregion


    #region JsonV4
    public class LauncherConfJsonV5
    {
        public string LauncherAssetLocation;
        public string LauncherStepbackLocation;
        public string LauncherConfLocation;
        public string[] LauncherAddonConfLocations;
        public string VersionLocation;
        public string Version;

        public LauncherConfJsonItemV5[] Items = { };

        public LauncherConfJsonV5(string _version)
        {
            Version = _version;
            LauncherAssetLocation = Path.Combine(MainForm.launcherDir, "VERSIONS", Version, "Launcher");
            LauncherConfLocation = Path.Combine(LauncherAssetLocation, "launcher.json");
            LauncherStepbackLocation = Path.Combine(MainForm.launcherDir, "VERSIONS", Version, "EternalDegrade", "DATA", "ADDONS");

            if (Directory.Exists(LauncherAssetLocation))
            {
                if (Directory.Exists(LauncherStepbackLocation))
                    LauncherAddonConfLocations = Directory.GetFiles(LauncherStepbackLocation).Where(it => it.Contains("addon_") && it.Contains(".json")).ToArray();
                else
                    LauncherAddonConfLocations = Directory.GetFiles(LauncherAssetLocation).Where(it => it.Contains("addon_") && it.Contains(".json")).ToArray();

            }

            VersionLocation = Path.Combine(MainForm.launcherDir, "VERSIONS", Version);

            if (!File.Exists(LauncherConfLocation))
            {
                return;
            }

            Directory.SetCurrentDirectory(VersionLocation); //Move ourselves to this working directory

            var launcherJson = File.ReadAllText(LauncherConfLocation);
            var lcjiList = new List<LauncherConfJsonItemV5>();
            lcjiList.AddRange(JsonConvert.DeserializeObject<LauncherConfJsonItemV5[]>(launcherJson));

            foreach (var addonJsonConfLocation in LauncherAddonConfLocations)
            {
                try
                {
                    var addonJson = File.ReadAllText(addonJsonConfLocation);
                    var addonConfigs = JsonConvert.DeserializeObject<LauncherConfJsonItemV5[]>(addonJson);

                    foreach (var conf in addonConfigs)
                    {
                        conf.ConfigFilename = addonJsonConfLocation;
                        conf.IsAddon = true;
                        conf.ItemClass = "ADDON";
                    }

                    lcjiList.AddRange(addonConfigs);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not load addon json config file at\n{addonJsonConfLocation}\n\n{ex}\n\n{(ex.InnerException != null ? ex.InnerException.ToString() : string.Empty)}");
                    //eat it
                }
            }

            if (lcjiList[0].ItemSubtitle != null) //means we can use metadata to reorder in categories
            {
                var vanguardImplementations = lcjiList.Where(it => it.ItemSubtitle != null && it.ItemSubtitle.ToUpper().Contains("VANGUARD")).ToList();
                var stubVanguardImplementations = lcjiList.Where(it => it.ItemSubtitle != null && it.ItemSubtitle.ToUpper().Contains("STUB")).ToList();
                var everythingElse = lcjiList.Where(it => it.ItemSubtitle != null && !it.ItemSubtitle.ToUpper().Contains("STUB") && !it.ItemSubtitle.ToUpper().Contains("VANGUARD")).ToList();
                var addButton = lcjiList.FirstOrDefault(it => it.ImageName == "Add.png");

                lcjiList.Clear();
                lcjiList.AddRange(vanguardImplementations);
                lcjiList.AddRange(stubVanguardImplementations);
                lcjiList.AddRange(everythingElse);

                if (addButton != null)
                    lcjiList.Add(addButton);
            }

            Items = lcjiList.ToArray();
        }
    }

    [SuppressMessage("Microsoft.Design", "CA1812: Avoid uninstantiated public classes", Justification = "Passed as a type parameter")]
    public class LauncherConfJsonItemV5
    {
        [JsonProperty]
        public readonly string FolderName;
        [JsonProperty]
        public readonly string ImageName;
        [JsonProperty]
        public readonly string DownloadVersion;
        [JsonProperty]
        public readonly string FirmwareFolder;
        [JsonProperty]
        public readonly ReadOnlyDictionary<string, ExecutableCommand> ExecutableCommands;

        //Used for the sidepanel and ordering of cards
        [JsonProperty]
        public string ItemClass;
        [JsonProperty]
        public readonly string ItemName;
        [JsonProperty]
        public readonly string ItemSubtitle;
        [JsonProperty]
        public readonly string ItemDescription;

        [JsonProperty]
        public readonly bool HideItem; //makes the card hide

        //Addon vars that are automatically set when the json is loaded
        public bool IsAddon;
        public string ConfigFilename;

        public LauncherConfJsonItemV5(string imageName, string downloadVersion, string folderName, ReadOnlyDictionary<string, ExecutableCommand> executableCommands, string itemName, string itemSubtitle, string itemDescription, bool hideItem, bool isAddon, string configFilename)
        {
            ImageName = imageName;
            DownloadVersion = downloadVersion;
            FolderName = folderName;
            ExecutableCommands = executableCommands;

            ItemName = itemName;
            ItemSubtitle = itemSubtitle;
            ItemDescription = itemDescription;

            HideItem = hideItem;
            IsAddon = isAddon;
            ConfigFilename = configFilename;
        }

        public bool Execute(bool runPreExecute = true, bool runPostExecute = true)
        {
            var success = true;
            foreach (var e in ExecutableCommands.Values)
            {
                success = (e.Execute(runPreExecute, runPostExecute) & success);
            }

            return success;
        }

        public bool Execute(string key, bool runPreExecute = true, bool runPostExecute = true)
        {
            ExecutableCommands.TryGetValue(key, out ExecutableCommand e);
            return e?.Execute(runPreExecute, runPostExecute) ?? false;
        }
    }

    public interface ILauncherJsonConfPanelV5 : ILauncherJsonConfPanel
    {
        LauncherConfJsonV5 GetLauncherJsonConf();
    }


    #endregion
}
