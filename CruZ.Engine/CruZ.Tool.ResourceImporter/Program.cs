using CommandLine;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CruZ.Tool.ResourceImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isBuild = false;

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    isBuild = o.IsBuild;
                    ResourceImporter.ResourceRoot = o.ResourceRoot;
                });

            Console.WriteLine(string.Format("ResourceRoot: {0}", Path.GetFullPath(ResourceImporter.ResourceRoot)));
            string resourceImporterPath = ResourceImporter.ResourceRoot + "\\.resourceImporter";

            ResourceImporter.SetImporterObject(
                ResourceImporter.ReadImporterObject(resourceImporterPath));

            if (isBuild)
            {
                ResourceImporter.DoBuild();
                ResourceImporter.ExportResult(resourceImporterPath);
            }
        }
    }
}