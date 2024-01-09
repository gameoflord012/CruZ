using CommandLine;

namespace CruZ.Tool.ResourceImporter
{
    public class Options
    {
        [Option('r', "root", Default = ".")]
        public string ResourceRoot { get; set; }

        [Option('b', "build", Default = false)]
        public bool IsBuild { get; set; }
    }
}