using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;

namespace CruZ.Tools.ResourceImporter
{
    internal class GuidManager
    {
        public string GenerateUniqueGuid()
        {
            string guid;
            do
            {
                guid = Guid.NewGuid().ToString();
            } while (IsConsumed(guid));

            return guid;
        }

        public void ConsumeGuid(string guid, string path)
        {
            if (IsConsumed(guid))
                throw new ArgumentException("Guid consumed");

            _getPathFromGuid[guid] = path;
            _getGuidFromPath[path] = guid;
        }

        public string GetPath(string guid)
        {
            return _getPathFromGuid[guid];
        }

        //public void ModifiedGuidPath(string guid, string newPath)
        //{
        //    RemovedGuid(guid);
        //    ConsumeGuid(guid, newPath);
        //}

        public void RemovedGuid(string guid)
        {
            if (!IsConsumed(guid))
                throw new ArgumentException("Guid not included");

            var path = _getPathFromGuid[guid];

            _getGuidFromPath.Remove(path);
            _getPathFromGuid.Remove(guid);
        }

        public void RemovePath(string path)
        {
            var guid = _getGuidFromPath[path];

            if (!IsConsumed(guid))
                throw new ArgumentException("Path not included");

            _getGuidFromPath.Remove(path);
            _getPathFromGuid.Remove(guid);
        }

        private bool IsConsumed(string guid)
        {
            return _getPathFromGuid.ContainsKey(guid);
        }

        Dictionary<string, string> _getPathFromGuid = new Dictionary<string, string>();
        Dictionary<string, string> _getGuidFromPath = new Dictionary<string, string>();
    }

    /// <summary>
    /// Process:
    /// 1. Load existed imported files
    /// </summary>
    public class ResourceImporter
    {
        public ResourceImporter(string importDir)
        {
            _resourceDir = importDir;
            _guidManager = new GuidManager();

            _fileWatcher = new FileSystemWatcher(importDir);
            _fileWatcher.IncludeSubdirectories = true;
            _fileWatcher.NotifyFilter =
                NotifyFilters.LastWrite |
                NotifyFilters.FileName |
                NotifyFilters.DirectoryName |
                NotifyFilters.CreationTime;
            _fileWatcher.Created += FileWatcher_Created;
            _fileWatcher.Deleted += FileWatcher_Deleted;
            _fileWatcher.Changed += FileWatcher_Changed;
            _fileWatcher.Renamed += FileWatcher_Renamed;
            _fileWatcher.EnableRaisingEvents = false;
        }

        #region FileWatcher Functions
        public void EnableWatch() { _fileWatcher.EnableRaisingEvents = true; }

        public void DisableWatch() { _fileWatcher.EnableRaisingEvents = false; }

        private void FileWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            File.Move(DotImport(e.OldFullPath), DotImport(e.FullPath));
            File.Move(XnbFileFrom(e.OldFullPath), XnbFileFrom(e.FullPath));
            File.Move(MgcontentFileFrom(e.OldFullPath), MgcontentFileFrom(e.FullPath));
        }

        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            _buildRequests.Add(e.FullPath);
            ProcessBuildRequests();
        }

        private void FileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            InitImportItem(e.FullPath);
            _buildRequests.Add(e.FullPath);
            ProcessBuildRequests();
        }

        private void FileWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            UnimportedItem(e.FullPath);
        } 
        #endregion

        public string GetResourcePathFromGuid(string guid)
        {
            return _guidManager.GetPath(guid);
        }

        public void LoadImportedItems()
        {
            #region COMMENTS
            //UnimportedItem();

            //ProcessBuildRequests();

            //foreach (var resourceFile in GetUnimportedItems())
            //{
            //    InitImportItem(resourceFile);

            //    string dotImport = resourceFile + ".resourceFile";
            //    string relImport = GetRelativePath(_resourceDir, resourceFile);
            //}

            //_importerObject.BuildResult = _getPathFromGuid; 
            #endregion

            // iterate throw previous session imports
            foreach (var dotImport in Directory.EnumerateFiles(_resourceDir, "*.import", SearchOption.AllDirectories))
            {
                var resourceFile = dotImport.Remove(dotImport.Length - ".import".Length);
                if (File.Exists(resourceFile))
                {
                    InitImportItem(resourceFile);
                    _buildRequests.Add(resourceFile);
                }
                else
                {
                    UnimportedItem(resourceFile);
                }
            }

            ProcessBuildRequests();
        }

        #region COMMENTS
        //        public void ExportResult(string resourcePath)
        //        {
        //            using (var writer = new StreamWriter(resourcePath, false))
        //            {
        //                writer.WriteLine(JsonConvert.SerializeObject(_importerObject, _SerializerSettings));
        //                writer.Flush();
        //            }
        //        }

        //        public void ExportResult()
        //        {
        //            ExportResult(_importerObject.ImporterFilePath);
        //        }

        //        public static Dictionary<string, string> GetResults()
        //        {
        //            return new Dictionary<string, string>(_importerObject.BuildResult);
        //        }

        //        public void SetImporterObject(ResourceImporterObject importerObject)
        //        {
        //            _importerObject = importerObject;
        //        }

        //        public void CreateDotImporter(string dotImporterFile)
        //        {
        //            string dir = Path.GetDirectoryName(dotImporterFile);
        //            Directory.CreateDirectory(dir);
        //            string dotImporterTemplate =
        //    @"{
        //""resourceFile-patterns"": [
        //    ""*.jpg"",
        //    ""*.png""
        //  ]
        //}";
        //            File.WriteAllText(dotImporterFile, dotImporterTemplate);
        //        }

        //        public static ResourceImporterObject ReadImporterObject(string resourcePath)
        //        {
        //            var importerObject = new ResourceImporterObject();

        //            if (!File.Exists(resourcePath))
        //                throw new FileNotFoundException(resourcePath);

        //            string json;
        //            using (StreamReader reader = new StreamReader(resourcePath))
        //            {
        //                json = reader.ReadToEnd();
        //            }

        //            JsonConvert.PopulateObject(json, importerObject, _SerializerSettings);
        //            importerObject.ImporterFilePath = resourcePath;

        //            return importerObject;
        //        }
        /// <summary>
        /// Get files with match resourceFile pattern
        /// </summary>
        /// <returns>Full path</returns>
        //private string[] GetUnimportedItems()
        //{
        //    HashSet<string> unimportedItems = new HashSet<string>();

        //    foreach (var pattern in _importerObject.ImportPatterns)
        //    {
        //        foreach (var match in Directory.EnumerateFiles(_resourceDir, pattern, SearchOption.AllDirectories))
        //        {
        //            if (IsImported(match))
        //            {
        //                var guid = GetResourceGuid(match);
        //                _guidManager.ConsumeGuid(guid, match);
        //            }
        //            else
        //            {
        //                unimportedItems.Add(match);
        //            }
        //        }
        //    }

        //    return unimportedItems.ToArray();
        //} 
        #endregion

        #region Private Functions
        private void InitImportItem(string resourceFile)
        {
            var dotImport = resourceFile + ".import";
            string guid;
            if (File.Exists(dotImport))
            {
                // read guid from .resourceFile file
                guid = GetResourceGuid(resourceFile);
            }
            else
            {
                // Write new guid to file
                guid = _guidManager.GenerateUniqueGuid();
                using (var writer = new StreamWriter(File.Create(dotImport)))
                {
                    writer.WriteLine(guid);
                    writer.Flush();
                }
                // TODO: build to Content as-well
            }

            _guidManager.ConsumeGuid(guid, resourceFile);
        }
        
        private void UnimportedItem(string resourceFile)
        {
            _guidManager.RemovePath(resourceFile);

            // delete .import file
            var dotImport = resourceFile + ".import";
            if (File.Exists(dotImport))
            {
                File.Delete(dotImport);
                _guidManager.RemovePath(resourceFile);
            }
            // delete .xnb file
            var xnb = XnbFileFrom(resourceFile);
            if (File.Exists(xnb)) File.Delete(xnb);
            // delete .mgcontent file
            var mgcontent = MgcontentFileFrom(resourceFile);
            if (File.Exists(mgcontent)) File.Delete(mgcontent);
            #region COMMENTS
            //foreach (var dotImport in Directory.EnumerateFiles(_resourceDir, "*.resourceFile", SearchOption.AllDirectories))
            //{
            //    var resourceFile = dotImport.Remove(dotImport.Length - ".resourceFile".Length);
            //    if (!File.Exists(resourceFile))
            //    {
            //        File.Delete(dotImport);
            //        //Debug.WriteLine("Removed " + dotImport);
            //    }
            //} 
            #endregion
        }

        private void ProcessBuildRequests()
        {
            string cmdArgs =
@"/workingDir:./..
/outputDir:bin
/intermediateDir:bin/obj
/platform:Windows
/incremental
";
            foreach (var resourceFile in _buildRequests)
            {
                cmdArgs += $"/build:{resourceFile}\n";
            }
            _buildRequests.Clear();

            // Create temporary MGCB file which built imported items
            var contentDir = Path.Combine(_resourceDir, ".content");
            var tempFile = Path.Combine(contentDir, "temp\\buildcontent.temp");
            var tempDir = Path.GetDirectoryName(tempFile);
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);
            File.WriteAllText(tempFile, cmdArgs, Encoding.UTF8);

            // Bulid temporary MGCB file
            BuildMGCBContent(tempFile);

            // Rebuild predefined MGCB file
            var preExistMgcbFile = Path.Combine(contentDir, "Content.mgcb");
            if (File.Exists(preExistMgcbFile))
                BuildMGCBContent(preExistMgcbFile);
        }

        private void BuildMGCBContent(string mgcbFile)
        {
            var cmd = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "dotnet.exe",
                    WorkingDirectory = _resourceDir,
                    Arguments = $"mgcb /@:{mgcbFile}",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                }
            };

            cmd.OutputDataReceived += (sender, e) => Log(e.Data);

            cmd.Start();
            cmd.BeginOutputReadLine();
            //_importerObject.BuildLog +=
            //    cmd.StandardOutput.ReadToEnd() + Environment.NewLine;
            cmd.WaitForExit();

            cmd.Dispose();
        }

        private string XnbFileFrom(string resourceFile)
        {
            if (Path.IsPathRooted(resourceFile)) resourceFile = GetRelativePath(_resourceDir, resourceFile);
            var content = Path.GetFileNameWithoutExtension(resourceFile);
            return Path.Combine(_resourceDir, ".content\\bin", content + ".xnb");
        }

        private string MgcontentFileFrom(string resourceFile)
        {
            if (Path.IsPathRooted(resourceFile)) resourceFile = GetRelativePath(_resourceDir, resourceFile);
            var content = Path.GetFileNameWithoutExtension(resourceFile);
            return Path.Combine(_resourceDir, ".content\\bin", content + ".mgcontent");
        }

        private string DotImport(string resourceFile)
        {
            if(Path.GetExtension(resourceFile) == ".import") return resourceFile;
            return resourceFile + ".import";
        }
        #endregion

        string _resourceDir;
        FileSystemWatcher _fileWatcher;
        GuidManager _guidManager;
        List<string> _buildRequests = new List<string>(); // resource files need to rebuild after being modified

        #region Static Functions
        private static string GetResourceGuid(string resourcePath)
        {
            var dotImport = resourcePath + ".import";
            if (!File.Exists(dotImport)) throw new FileNotFoundException(dotImport);
            using (var reader = new StreamReader(dotImport))
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

        //private static bool IsImported(string filePath)
        //{
        //    return File.Exists(filePath + ".import");
        //}

        private static void Log(string msg)
        {
            Debug.WriteLine("[ResourceImporter] " + msg);
        }

        static readonly JsonSerializerSettings _SerializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
        #endregion
    }
}
