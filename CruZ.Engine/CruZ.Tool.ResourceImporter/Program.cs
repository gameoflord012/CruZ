using CommandLine;
using Newtonsoft.Json;

namespace CruZ.Tool.ResourceImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isBuild = false;

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    isBuild = o.IsBuild;
                    ResourceImporter.ResourceRoot = o.ResourceRoot;
                });

            Console.WriteLine(string.Format("ResourceRoot: {0}", Path.GetFullPath(ResourceImporter.ResourceRoot)));
            string resourceImporterPath = ResourceImporter.ResourceRoot + "\\.resourceImporter";

            ResourceImporter.ReadImporterObject(resourceImporterPath);

            if (isBuild)
            {
                ResourceImporter.DoBuild();
                ResourceImporter.ExportResult(resourceImporterPath);
            }
        }
    }
}