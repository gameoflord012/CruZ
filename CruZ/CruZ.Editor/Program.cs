using CommandLine;

using System;
using System.IO;
using System.Reflection;

namespace CruZ.Editor
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var domainDir = AppDomain.CurrentDomain.BaseDirectory;

            Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                EditorContext.GameProjectDir = Path.Combine(domainDir, o.ProjectRoot);
                EditorContext.GameAssembly = Assembly.LoadFile(Path.Combine(domainDir, o.GameAssembly));
            }).WithNotParsed(errors =>
            {
                string errorMsg = "";

                foreach (var e in errors)
                {
                    if (e is MissingRequiredOptionError missingOptionError)
                    {
                        errorMsg += $"Missing command line option value: \"{missingOptionError.NameInfo.LongName}\"\n";
                    }
                    else
                    {
                        errorMsg += e.ToString() + Environment.NewLine;
                    }
                }

                throw new ArgumentException(errorMsg);
            });

            EditorContext.EditorResourceDir = Path.Combine(domainDir, "Resource");
            EditorForm.Run();
        }

        public class Options
        {
            [Option('r', "root", Required = true)]
            public string ProjectRoot { get; set; } = "";

            [Option('a', "assembly", Required = true)]
            public string GameAssembly { get; set; } = "";
        }
    }
}