using CommandLine;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace CruZ.Tools.ResourceImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isBuild = false;

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    isBuild = o.IsBuild;
                    ResourceImporter.ResourceRoot = o.ResourceRoot;
                });

            Debug.WriteLine(string.Format("ResourceRoot: {0}", Path.GetFullPath(ResourceImporter.ResourceRoot)));
            string resourceImporterPath = ResourceImporter.ResourceRoot + "\\.resourceImporter";

            ResourceImporter.SetImporterObject(
                ResourceImporter.ReadImporterObject(resourceImporterPath));

            if (isBuild)
            {
                ResourceImporter.DoBuild();
                ResourceImporter.ExportResult(resourceImporterPath);

                BuildContent();
            }
        }

        private static void BuildContent()
        {
            Console.WriteLine("Build content...");

            var resFullDir = Path.GetFullPath(ResourceImporter.ResourceRoot);

            string args = string.Format("/outputDir:{0}\\bin\n/intermediateDir:{0}\\obj\n/platform:Windows\n/incremental\n", CONTENT_ROOT);
            foreach (var res in ResourceImporter.GetResults())
            {
                args += string.Format("/build:{0}\n", resFullDir + "\\" + res.Value);
            }

            var tempFile = resFullDir + "\\resourceImporter.temp";
            File.WriteAllText(tempFile, args);

            Process p = new Process();
            p.StartInfo.FileName = "mgcb.exe";
            p.StartInfo.Arguments = "/@:" + tempFile;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.WorkingDirectory = resFullDir;

            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            p.WaitForExit();

            Console.WriteLine(output);
            Console.WriteLine(error);

            File.Delete(tempFile);
        }

        static readonly string CONTENT_ROOT = "Content";
    }
}