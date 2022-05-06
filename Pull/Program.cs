namespace Pull
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("RTCV Unstable Branch Pull Tool");
            Console.WriteLine("===============================");
            Console.WriteLine();

            var launcherDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            var unstableDir = launcherDir.Parent;

            Console.WriteLine("Clearing Pull Cache");
            Console.WriteLine();

            var pullDirString = Path.Combine(unstableDir.FullName, "Pull");

            if (Directory.Exists(pullDirString))
                Directory.Delete(pullDirString, true);

            Directory.CreateDirectory(pullDirString);

            var pullDir = new DirectoryInfo(pullDirString);

            //var pullWorkDirString = Path.Combine(pullDir.FullName, "Working");
            //Directory.CreateDirectory(pullWorkDirString);
            //var pullWorkDir = new DirectoryInfo(pullWorkDirString);

            string unstableUrl = "http://cc.r5x.cc/rtc/unstable/";
            string updateUrl = unstableUrl + "update.zip";

            string updateZipPath = Path.Combine(pullDir.FullName, "update.zip");

            using (WebClient wc = new WebClient())
            {
                Console.WriteLine("Downloading Update ");

                bool done = false;

                wc.DownloadProgressChanged += (o, e) =>
                {
                    Console.Write('#');
                };

                wc.DownloadFileCompleted += (o, e) =>
                {
                    done = true;
                };

                wc.DownloadFileAsync(new Uri(updateUrl), updateZipPath);

                while (!done)
                {
                    Thread.Sleep(69);
                }
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Extracting Update ");

            ZipArchive archive = new ZipArchive(new MemoryStream(File.ReadAllBytes(updateZipPath)));

            string errorHeader = ", ";
            List<string> errors = new List<string>();

            foreach (var entry in archive.Entries)
            {
                var entryPath = Path.Combine(unstableDir.FullName, entry.FullName).Replace("/", "\\");

                if (entryPath.EndsWith("\\"))
                {
                    if (!Directory.Exists(entryPath))
                    {
                        Directory.CreateDirectory(entryPath);
                        Console.Write('#');
                    }
                }
                else
                {
                    var s = entry.Open();
                    byte[] data;
                    using (var ms = new MemoryStream())
                    {
                        s.CopyTo(ms);
                        data = ms.ToArray();
                    }

                    try
                    {
                        File.WriteAllBytes(entryPath, data);
                    }
                    catch (Exception ex)
                    {
                        _ = ex;

                        if (!entry.FullName.Contains("Pull.exe"))
                            errors.Add($"File could not be updated: {entry.FullName}");
                    }
                    Console.Write('#');
                }
            }

            if (errors.Count > 0)
            {
                Console.WriteLine();

                foreach (var error in errors)
                    Console.WriteLine(error);

                errorHeader = " with errors, Check errors above and ";
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine($"Update installed{errorHeader}press any key to quit.");
            Console.ReadKey();
        }
    }
}
