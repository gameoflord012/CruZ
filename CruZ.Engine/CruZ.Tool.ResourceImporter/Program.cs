using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace CruZ.Tool.ResourceImporter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ParseCommandArgs(args);
            ReadDotResourceImporter();
            InitImportItems();
            DoBuild();
        }

        private static void DoBuild()
        {
            if (_isBuild)
            {
                Console.WriteLine("Start build...");
                RemoveExcessDotImport();

                Console.WriteLine("Process import item...");

                foreach (var import in _importItems)
                {
                    string dotImport = import + ".import";

                    if (File.Exists(dotImport))
                    {
                        using (var reader = new StreamReader(dotImport))
                        {
                            var guid = reader.ReadToEnd().Replace("\r\n", "");

                            if (!_usedGuid.Contains(guid))
                            {
                                _usedGuid.Add(guid);
                                _getPathFromGuid[guid] = import;
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
                        var guid = GetUniqueGuid();
                        _getPathFromGuid[guid] = import;

                        writer.WriteLine(guid);
                        writer.Flush();

                        Console.WriteLine(string.Format("Guid for {0} generated", dotImport));
                    }
                }

                ExportResult();
            }
        }

        private static void RemoveExcessDotImport()
        {
            var option = new EnumerationOptions
            {
                RecurseSubdirectories = true
            };

            foreach (var dotImport in Directory.EnumerateFiles(_resourceRoot, "*.import", option))
            {
                var import = dotImport.Remove(dotImport.Length - ".import".Length);
                if (!File.Exists(import))
                {
                    File.Delete(dotImport);
                    Console.WriteLine("Remove " + dotImport);
                }
            }
        }

        private static void ExportResult()
        {
            _jObject["build-result"] = JObject.FromObject(_getPathFromGuid);
            var json = JsonConvert.SerializeObject(_jObject, _serializerSettings);

            using (var writer = new StreamWriter(ResourceImporterPath, false))
            {
                writer.WriteLine(json);
                writer.Flush();
            }
        }

        private static void InitImportItems()
        {
            Console.WriteLine(string.Format("Matches found:"));

            foreach (var pattern in _importPatterns)
            {
                var option = new EnumerationOptions
                {
                    RecurseSubdirectories = true
                };

                foreach (var match in Directory.EnumerateFiles(_resourceRoot, pattern, option))
                {
                    _importItems.Add(match);
                    Console.WriteLine(match);
                }
            }
        }

        private static void ParseCommandArgs(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    _isBuild = o.IsBuild;
                    _resourceRoot = o.ResourceRoot;
                });
        }

        private static void ReadDotResourceImporter()
        {
            if (File.Exists(ResourceImporterPath))
            {
                Console.WriteLine("Found .resourceimporter");

                using (StreamReader reader = new(ResourceImporterPath))
                {
                    var json = reader.ReadToEnd();
                    try
                    {
                        _jObject = JObject.Parse(json);
                    }
                    catch (JsonReaderException)
                    {
                        Console.WriteLine("Failed to read json file, default settings is used");
                    }

                    if (_jObject["resource-root"] != null)
                    {
                        _resourceRoot = _jObject["resource-root"].Value<string>() ?? _resourceRoot;
                    }

                    if (_jObject["import-pattern"] != null)
                    {
                        _importPatterns = _jObject["import-pattern"]
                            .Select(p => p.Value<string>()).ToList();
                    }
                }
            }

            Console.WriteLine(string.Format("ResourceRoot: {0}", Path.GetFullPath(_resourceRoot)));
        }

        public static string GetUniqueGuid()
        {
            string guid;
            do
            {
                guid = Guid.NewGuid().ToString();
            } while (_usedGuid.Contains(guid));

            return guid;
        }

        static string               _resourceRoot = ".";
        static bool                 _isBuild;

        static Dictionary<string, string>   _getPathFromGuid = [];
        static HashSet<string>              _importItems = [];
        static List<string>                 _importPatterns = [];
        static HashSet<string>              _usedGuid = [];
        static JObject                      _jObject = new();
        static JsonSerializerSettings       _serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };

        static string ResourceImporterPath { get => _resourceRoot + "\\" + ".resourceImporter"; }
    }
}
