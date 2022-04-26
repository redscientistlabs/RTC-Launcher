using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Push
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("RTCV Unstable Branch Push Tool");
            Console.WriteLine("===============================");
            Console.WriteLine();


            var pushDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            var unstableDir = pushDir.Parent;

            var launcherDir = new DirectoryInfo(Path.Combine(unstableDir.FullName, "Launcher"));
            var rtcvDir = new DirectoryInfo(Path.Combine(unstableDir.FullName, "RTCV"));

            Console.WriteLine("Clearing Push Cache");
            Console.WriteLine();


            var pushWorkDirString = Path.Combine(pushDir.FullName, "Work");

            if (Directory.Exists(pushWorkDirString))
                Directory.Delete(pushWorkDirString, true);

            Directory.CreateDirectory(pushWorkDirString);

            string updateZipPath = Path.Combine(@"W:\rtc\unstable", "update.zip");

            var pushLauncherDirString = Path.Combine(pushWorkDirString, "Launcher");
            Directory.CreateDirectory(pushLauncherDirString);


            Console.WriteLine("Copying Launcher Files ");
            foreach (var filepath in Directory.GetFiles(launcherDir.FullName))
            {
                try
                {
                    var file = new FileInfo(filepath);
                    file.CopyTo(Path.Combine(pushLauncherDirString, file.Name));
                }
                catch(Exception ex)
                {
                    if (!filepath.Contains("Push.exe"))
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("An error has occurred;");
                        Console.WriteLine(ex.ToString());
                        Console.WriteLine();
                        Console.WriteLine("Aborting deploy, press any key to exit");
                        Console.ReadKey();
                        return;
                    }

                }
                Console.Write('#');
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Copying RTCV Files ");

            var pushRtcvDirString = Path.Combine(pushWorkDirString, "RTCV");
            Directory.CreateDirectory(pushRtcvDirString);

            foreach (var file in rtcvDir.GetFiles())
            {
                file.CopyTo(Path.Combine(pushRtcvDirString, file.Name));
                Console.Write('#');
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Zipping update package ");

            string tmpPath = updateZipPath + ".tmp";

            if (File.Exists(tmpPath))
                File.Delete(tmpPath);


            ZipFile.CreateFromDirectory(pushWorkDirString, tmpPath);

            Console.WriteLine();
            Console.WriteLine("Deploying update ");

            if (File.Exists(updateZipPath))
                File.Delete(updateZipPath);

            File.Move(tmpPath, updateZipPath);

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine($"Update deployed, press any key to quit.");
            Console.ReadKey();

        }
    }
}
