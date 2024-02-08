using CommandLine;
using CruZ.Resource;
using CruZ.Tool.ResourceImporter;
using System;
using System.IO;

namespace CruZ.Editor
{
    public class Program
    {
        private const string WORKING_DIR = ".cruz";

        [STAThread]
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
            {
                var workDir = Path.Combine(o.ProjectRoot, WORKING_DIR);
                var resDir = Path.Combine(workDir, "..\\res");

                if (!Directory.Exists(workDir)) Directory.CreateDirectory(workDir);

                Environment.CurrentDirectory = workDir;
                ResourceManager.ResourceRoot = resDir;
            });

            ResourceManager.RunImport();
            EditorForm.Run();
        }

        public class Options
        {
            [Option('r', "root", Required = true)]
            public string ProjectRoot { get; set; }
        }
    }
}