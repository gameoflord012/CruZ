using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CruZ.Tool.ResourceImporter
{
    public static class ResourceImporter
    {
        public static string ResourceRoot = ".";
        private static ResourceImporterObject _ImporterObject = new ResourceImporterObject();

        public static void DoBuild()
        {
            HashSet<string> usedGuid = new HashSet<string>();
            Dictionary<string, string> getPathFromGuid = new Dictionary<string, string>();

            RemoveExcessDotImport();

            foreach (var import in GetImportItems())
            {
                string dotImport = import + ".import";
                string relImport = GetRelativePath(ResourceRoot, import);

                if (File.Exists(dotImport))
                {
                    var guid = ReadGuidFrom(dotImport);

                    if (!usedGuid.Contains(guid))
                    {
                        usedGuid.Add(guid);
                        getPathFromGuid[guid] = relImport;
                        continue;
                    }

                }
                else
                {
                    File.Create(dotImport).Close();
                    Debug.WriteLine(string.Format("Created new file {0}", dotImport));
                }

                using (var writer = new StreamWriter(dotImport, false))
                {
                    var guid = GetUniqueGuid(usedGuid);
                    getPathFromGuid[guid] = relImport;

                    writer.WriteLine(guid);
                    writer.Flush();

                    Debug.WriteLine(string.Format("Guid for {0} generated", dotImport));
                }
            }

            _ImporterObject.BuildResult = getPathFromGuid;
        }

        private static string GetRelativePath(string relativeFolder, string destinationFile)
        {
            Uri folder = new Uri(Path.GetFullPath(relativeFolder).TrimEnd('\\') + "\\");
            Uri file = new Uri(Path.GetFullPath(destinationFile));

            return Uri.UnescapeDataString(
                folder.MakeRelativeUri(file)
                    .ToString()
                    .Replace('/', Path.DirectorySeparatorChar)
                );
        }

        public static void ExportResult(string filePath)
        {
            using (var writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine(JsonConvert.SerializeObject(_ImporterObject, _SerializerSettings));
                writer.Flush();
            }
        }

        public static Dictionary<string, string> GetResults()
        {
            return new Dictionary<string, string>(_ImporterObject.BuildResult);
        }

        public static void SetImporterObject(ResourceImporterObject importerObject)
        {
            _ImporterObject = importerObject;
        }

        public static ResourceImporterObject ReadImporterObject(string filePath)
        {
            var importerObject = new ResourceImporterObject();

            if (File.Exists(filePath))
            {
                Debug.WriteLine("Reading " + Path.GetFullPath(filePath));

                using (StreamReader reader = new StreamReader(filePath))
                {
                    var json = reader.ReadToEnd();
                    var deserialize = JsonConvert.DeserializeObject<ResourceImporterObject>(json, _SerializerSettings);

                    if(deserialize == null)
                    {
                        Debug.WriteLine("Failed to read json file, default settings is used");
                    }
                    else
                    {
                        importerObject = deserialize;
                    }
                }
            }

            return importerObject;
        }

        private static string[] GetImportItems()
        {
            HashSet<string> importItems = new HashSet<string>();

            foreach (var pattern in _ImporterObject.ImportPatterns)
            {
                foreach (var match in Directory.EnumerateFiles(ResourceRoot, pattern, SearchOption.AllDirectories))
                {
                    importItems.Add(match);
                    Debug.WriteLine("Matches found: " + match);
                }
            }

            return importItems.ToArray();
        }

        private static void RemoveExcessDotImport()
        {
            foreach (var dotImport in Directory.EnumerateFiles(ResourceRoot, "*.import", SearchOption.AllDirectories))
            {
                var import = dotImport.Remove(dotImport.Length - ".import".Length);
                if (!File.Exists(import))
                {
                    File.Delete(dotImport);
                    Debug.WriteLine("Removed " + dotImport);
                }
            }
        }

        public static string ReadResourceGuid(string resourcePath)
        {
            return ReadGuidFrom(resourcePath + ".import");
        }

        private static string ReadGuidFrom(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
            using (var reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd().Replace("\r\n", "");
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
