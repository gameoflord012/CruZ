using CommandLine;

using System;
using System.IO;

namespace CruZ.Editor
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            string userProjectDir = "";
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                userProjectDir = o.ProjectRoot;
            });

            EditorContext.UserProjectDir = userProjectDir;
            EditorContext.EditorResourceDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "res\\");
            
            EditorForm.Run();
        }

        public class Options
        {
            [Option('r', "root", Required = true)]
            public string ProjectRoot { get; set; }
        }
    }
}