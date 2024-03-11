using CommandLine;

using CruZ.Editor.Global;

using System;
using System.IO;

namespace CruZ.Editor
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                EditorContext.UserProjectDir = o.ProjectRoot;
                EditorContext.UserProjectBinDir = o.ProjectBinary;
            });

            EditorContext.EditorResourceDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource\\");

            EditorForm.Run();
        }

        public class Options
        {
            [Option('r', "root", Required = true)]
            public string ProjectRoot { get; set; }

            [Option('b', "binary", Required = true)]
            public string ProjectBinary { get; set; }
        }
    }
}