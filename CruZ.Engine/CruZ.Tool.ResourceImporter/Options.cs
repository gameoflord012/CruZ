using CommandLine;

namespace CruZ.Tools.ResourceImporter
{
    public class Options
    {
        [Option('r', "root", Default = ".")]
        public string ResourceRoot { get; set; }

        [Option('b', "build", Default = false)]
        public bool IsBuild { get; set; }

        //[Option('o', "output", Default = false)]
        //public bool IsOutput { get; set; }
    }
}