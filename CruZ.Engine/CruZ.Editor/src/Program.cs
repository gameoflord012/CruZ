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
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                EditorGlobal.UserProjectDir = Path.Combine(
                    o.ProjectRoot, EditorGlobal.USER_PROJECT_PROFILE_DIR_NAME);
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