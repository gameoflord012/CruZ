using CommandLine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CruZ.Tools.ResourceImporter
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

            BuildContent();

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
                    //Debug.WriteLine(string.Format("Created new file {0}", dotImport));
                }

                using (var writer = new StreamWriter(dotImport, false))
                {
                    var guid = GetUniqueGuid(usedGuid);
                    getPathFromGuid[guid] = relImport;

                    writer.WriteLine(guid);
                    writer.Flush();

                    //Debug.WriteLine(string.Format("Guid for {0} generated", dotImport));
                }
            }

            _ImporterObject.BuildResult = getPathFromGuid;
        }

        private static void BuildContent()
        {
            string cmdArgs =
@"
/workingDir:./..
/outputDir:bin
/intermediateDir:bin/obj
/platform:Windows

/incremental
";
            foreach (var import in GetImportItems())
            {
                cmdArgs += $"/build:{import}\n";
            }

            // Create temporary MGCB file which built imported items
            var contentDir = Path.Combine(ResourceRoot, ".content");
            var tempFile = Path.Combine(contentDir, "temp\\buildcontent.temp");
            var tempDir = Path.GetDirectoryName(tempFile);
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);
            File.WriteAllText(tempFile, cmdArgs, Encoding.UTF8);

            // Bulid temporary MGCB file
            BuildMGCBFile(tempFile);

            // Rebuild predefined MGCB file
            var preExistMgcbFile = Path.Combine(contentDir, "Content.mgcb");
            if (File.Exists(preExistMgcbFile))
                BuildMGCBFile(preExistMgcbFile);
        }

        private static void BuildMGCBFile(string mgcbFile)
        {
            var cmd = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "dotnet.exe",
                    WorkingDirectory = ResourceRoot,
                    Arguments = $"mgcb /@:{mgcbFile}",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                }
            };

            //cmd.OutputDataReceived += (sender, e) => Log(e.Data);
            //cmd.BeginOutputReadLine();    

            cmd.Start();
            _ImporterObject.BuildLog +=
                cmd.StandardOutput.ReadToEnd() + Environment.NewLine;
            cmd.WaitForExit();

            cmd.Dispose();
        }

        public static string ReadResourceGuid(string resourcePath)
        {
            return ReadGuidFrom(resourcePath + ".import");
        }

        public static void ExportResult(string filePath)
        {
            using (var writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine(JsonConvert.SerializeObject(_ImporterObject, _SerializerSettings));
                writer.Flush();
            }
        }

        public static void ExportResult()
        {
            ExportResult(_ImporterObject.ImporterFilePath);
        }

        public static Dictionary<string, string> GetResults()
        {
            return new Dictionary<string, string>(_ImporterObject.BuildResult);
        }

        public static void SetImporterObject(ResourceImporterObject importerObject)
        {
            _ImporterObject = importerObject;
        }

        public static void CreateDotImporter(string dotImporterFile)
        {
            string dir = Path.GetDirectoryName(dotImporterFile);
            Directory.CreateDirectory(dir);
            string dotImporterTemplate =
@"{
""import-patterns"": [
    ""*.jpg"",
    ""*.png""
  ]
}";
            File.WriteAllText(dotImporterFile, dotImporterTemplate);
        }

        public static ResourceImporterObject ReadImporterObject(string filePath)
        {
            var importerObject = new ResourceImporterObject();

            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            string json;
            using (StreamReader reader = new StreamReader(filePath))
            {
                json = reader.ReadToEnd();
            }

            JsonConvert.PopulateObject(json, importerObject, _SerializerSettings);
            importerObject.ImporterFilePath = filePath;

            return importerObject;
        }

        #region Private Functions
        /// <summary>
        /// Get files with match import pattern
        /// </summary>
        /// <returns>Full path</returns>
        private static string[] GetImportItems()
        {
            HashSet<string> importItems = new HashSet<string>();

            foreach (var pattern in _ImporterObject.ImportPatterns)
            {
                foreach (var match in Directory.EnumerateFiles(ResourceRoot, pattern, SearchOption.AllDirectories))
                {
                    importItems.Add(match);
                    //Debug.WriteLine("Matches found: " + match);
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
                    //Debug.WriteLine("Removed " + dotImport);
                }
            }
        }

        private static string ReadGuidFrom(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
            using (var reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd().Replace("\r\n", "");
            }
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

        private static void Log(string msg)
        {
            Debug.WriteLine("<--ResourceImporter--> " + msg);
        }
        #endregion
    }
}
