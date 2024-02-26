using CommandLine;

using CruZ.Resource;

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
                EditorVariables.UserResDir = Path.Combine(o.ProjectRoot, "res");
                EditorVariables.UserProjectProfileDir = Path.Combine(
                    o.ProjectRoot, EditorVariables.USER_PROJECT_PROFILE_DIR_NAME);

                ResourceManager.User.ResourceRoot = EditorVariables.UserResDir;
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