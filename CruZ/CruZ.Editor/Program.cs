using CommandLine;

using CruZ.Editor.Global;

using System;
using System.IO;
using System.Linq;
using System.Reflection;

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
                EditorContext.GameProjectDir = o.ProjectRoot;
                EditorContext.GameAssembly = Assembly.LoadFile(o.GameAssembly);
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

            EditorContext.EditorResourceDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource\\");

            EditorContext.AssemblyResolver = (assName) =>
                AppDomain.CurrentDomain.GetAssemblies()
                    .First(e => e.FullName == assName.FullName);

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