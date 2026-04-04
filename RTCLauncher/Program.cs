namespace RTCV.Launcher
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    static class Program
    {
        public static Process StandaloneRTCProcess = null;

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string oldLauncherExeArg = args.FirstOrDefault(a => a.StartsWith("--oldlauncherexe"));
            if (oldLauncherExeArg != null)
            {
                string[] split = oldLauncherExeArg.Split(new char[] { '=' }, 2);
                if (split.Length == 2)
                {
                    string oldFile = split[1].Trim('"');
                    if (File.Exists(oldFile))
                    {
                        File.Delete(oldFile);
                    }
                }
            }
            using (var mutex = new Mutex(true, "RTC_Launcher", out var createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());

                    bool rustic = false;
                    int e = 5;
                    if (rustic && e > 20)
                        Rusticles.Brainrot();
                }
                else
                {
                    var current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                }
            }
        }
    }
}
