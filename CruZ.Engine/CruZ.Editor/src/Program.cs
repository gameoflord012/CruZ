using CommandLine;
using System;
using System.IO;
using System.Windows.Forms;

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
                var fullDir = Path.Combine(o.ProjectRoot, WORKING_DIR);
                if(!Directory.Exists(fullDir)) Directory.CreateDirectory(fullDir);

                Environment.CurrentDirectory = fullDir;
            });

            EditorForm.Run();
        }

        public class Options
        {
            [Option('r', "root", Required = true)]
            public string ProjectRoot { get; set; }
        }
    }
}