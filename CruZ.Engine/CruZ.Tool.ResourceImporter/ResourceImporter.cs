using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace CruZ.Tool.ResourceImporter
{
    public static class ResourceImporter
    {
        public static string ResourceRoot = ".";
        private static ResourceImporterObject _ImporterObject = new();

        public static void DoBuild()
        {
            HashSet<string> usedGuid = [];
            Dictionary<string, string> getPathFromGuid = [];

            RemoveExcessDotImport();

            foreach (var import in GetImportItems())
            {
                string dotImport = import + ".import";

                if (File.Exists(dotImport))
                {
                    using (var reader = new StreamReader(dotImport))
                    {
                        var guid = reader.ReadToEnd().Replace("\r\n", "");

                        if (!usedGuid.Contains(guid))
                        {
                            usedGuid.Add(guid);
                            getPathFromGuid[guid] = import;
                            continue;
                        }
                    }
                }
                else
                {
                    File.Create(dotImport).Close();
                    Console.WriteLine(string.Format("Created new file {0}", dotImport));
                }

                using (var writer = new StreamWriter(dotImport, false))
                {
                    var guid = GetUniqueGuid(usedGuid);
                    getPathFromGuid[guid] = import;

                    writer.WriteLine(guid);
                    writer.Flush();

                    Console.WriteLine(string.Format("Guid for {0} generated", dotImport));
                }
            }

            _ImporterObject.BuildResult = getPathFromGuid;
        }

        public static void ExportResult(string filePath)
        {
            using (var writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine(JsonConvert.SerializeObject(_ImporterObject, _SerializerSettings));
                writer.Flush();
            }
        }

        public static ResourceImporterObject ReadImporterObject(string filePath)
        {
            _ImporterObject = new ResourceImporterObject();

            if (File.Exists(filePath))
            {
                Console.WriteLine("Reading " + Path.GetFullPath(filePath));

                using (StreamReader reader = new(filePath))
                {
                    var json = reader.ReadToEnd();
                    var deserialize = JsonConvert.DeserializeObject<ResourceImporterObject>(json, _SerializerSettings);

                    if(deserialize == null)
                    {
                        Console.WriteLine("Failed to read json file, default settings is used");
                    }
                    else
                    {
                        _ImporterObject = deserialize;
                    }
                }
            }

            return _ImporterObject;
        }

        private static string[] GetImportItems()
        {
            HashSet<string> importItems = [];

            foreach (var pattern in _ImporterObject.ImportPatterns)
            {
                var option = new EnumerationOptions
                {
                    RecurseSubdirectories = true
                };

                foreach (var match in Directory.EnumerateFiles(ResourceRoot, pattern, option))
                {
                    importItems.Add(match);
                    Console.WriteLine("Matches found: " + match);
                }
            }

            return importItems.ToArray();
        }

        private static void RemoveExcessDotImport()
        {
            var option = new EnumerationOptions
            {
                RecurseSubdirectories = true
            };

            foreach (var dotImport in Directory.EnumerateFiles(ResourceRoot, "*.import", option))
            {
                var import = dotImport.Remove(dotImport.Length - ".import".Length);
                if (!File.Exists(import))
                {
                    File.Delete(dotImport);
                    Console.WriteLine("Removed " + dotImport);
                }
            }
        }

        private static string GetUniqueGuid(HashSet<string> excludeList)
        {
            string guid;
            do
            {
                guid = Guid.NewGuid().ToString();
            } while (excludeList.Contains(guid));

            return guid;
        }

        static readonly JsonSerializerSettings _SerializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
    }
}
