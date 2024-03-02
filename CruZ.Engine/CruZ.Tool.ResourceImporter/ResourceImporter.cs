using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CruZ.Tools.ResourceImporter
{
    internal class GuidManager<T>
    {
        public Guid GenerateUniqueGuid()
        {
            Guid guid;
            do
            {
                guid = Guid.NewGuid();
            } while (IsConsumed(guid));

            return guid;
        }

        public void ConsumeGuid(Guid guid, T value)
        {
            if (IsConsumed(guid)) return;
            _getValueFromGuid[guid] = value;
            _getGuidFromValue[value] = guid;
        }

        public T GetValue(Guid guid)
        {
            if (!_getValueFromGuid.ContainsKey(guid))
                throw new ResourcePathNotFoundException($"Resource with guid \"{guid}\" is unavaiable or unimported");
            return _getValueFromGuid[guid];
        }

        public Guid GetGuid(T value)
        {
            if (!_getGuidFromValue.ContainsKey(value))
                throw new ResourceGuidNotFoundException($"Resource with path \"{value}\" is unavaiable or unimported");
            return _getGuidFromValue[value];
        }

        public void RemovedGuid(Guid guid)
        {
            if (!IsConsumed(guid))
                throw new ArgumentException("Guid not included");

            var path = _getValueFromGuid[guid];

            _getGuidFromValue.Remove(path);
            _getValueFromGuid.Remove(guid);
        }

        public void RemoveValue(T value)
        {
            if(!_getGuidFromValue.ContainsKey(value)) return;
            var guid = _getGuidFromValue[value];
            _getGuidFromValue.Remove(value);
            _getValueFromGuid.Remove(guid);
        }

        private bool IsConsumed(Guid guid)
        {
            return _getValueFromGuid.ContainsKey(guid);
        }

        Dictionary<Guid, T> _getValueFromGuid = new Dictionary<Guid, T>();
        Dictionary<T, Guid> _getGuidFromValue = new Dictionary<T, Guid>();
    }

    /// <summary>
    /// Manage imported resources, its Guid and MGCB backends
    /// </summary>
    public class ResourceImporter
    {
        #region Inner Classes
        /// <summary>
        /// Raw resource path container
        /// </summary>
        public struct NonContextResourcePath
        {
            public NonContextResourcePath(string resourcePath)
            {
                _resourcePath = resourcePath;
            }

            public static implicit operator NonContextResourcePath(string resourcePath)
            {
                return new NonContextResourcePath(resourcePath);
            }

            public ResourcePath CheckedBy(ResourceImporter importer)
            {
                return new ResourcePath(importer, _resourcePath);
            }

            string _resourcePath;
        }

        /// <summary>
        /// Unify formated resource path
        /// </summary>
        public struct ResourcePath
        {
            public ResourcePath(ResourceImporter importer, string NonContextResourcePath)
            {
                var resourceFullPath = Path.Combine(importer._resourceDir, NonContextResourcePath);
                _resourcePath = Path.GetFullPath(resourceFullPath);
            }

            public static implicit operator string(ResourcePath resourcePath)
            {
                return resourcePath._resourcePath;
            }

            public override string ToString()
            {
                return _resourcePath;
            }

            string _resourcePath;
        }
        #endregion

        public ResourceImporter(string importDir)
        {
            _resourceDir = Path.GetFullPath(importDir);
            _contentOutputDir = Path.GetFullPath(Path.Combine(importDir, ".content\\bin"));
            _guidManager = new GuidManager<ResourcePath>();

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

        #region Public Functions
        public void InitializeImporter()
        {
            // iterate through last session imported items
            foreach (var filePath in Directory.EnumerateFiles(_resourceDir, "*.*", SearchOption.AllDirectories))
            {
                var extension = Path.GetExtension(filePath);
                switch (extension)
                {
                    // remove excess .import files
                    case ".import":
                        var resourceFile = filePath.Substring(0, filePath.LastIndexOf(extension));
                        if (!File.Exists(resourceFile)) UnimportResource(resourceFile);
                        break;

                    // remove last content build
                    case ".xnb":
                    case ".mgcontent":
                        File.Delete(filePath);
                        break;

                    default:
                        // initialize resource if filePath is a resource path
                        if (ResourceSupportedExtensions.Contains(Path.GetExtension(filePath).ToLower()))
                        {
                            InitImportingResource(filePath);
                        }
                        break;
                }
            }
        }

        public ResourcePath GetResourcePathFromGuid(Guid guid)
        {
            return _guidManager.GetValue(guid);
        }

        public Guid GetResourceGuid(NonContextResourcePath uncheckedResourcePath)
        {
            var resourcePath = uncheckedResourcePath.CheckedBy(this);
            return _guidManager.GetGuid(resourcePath);
        }

        public void ImportResources(params NonContextResourcePath[] importRequests)
        {
            foreach (var request in importRequests)
            {
                InitImportingResource(request);
                _buildRequests.Add(request);
            }

            ProcessBuildRequests();
        }
        #endregion

        #region FileWatcher Functions
        // TODO: implement filePath watcher
        public void EnableWatch() { _fileWatcher.EnableRaisingEvents = true; }

        public void DisableWatch() { _fileWatcher.EnableRaisingEvents = false; }

        private void FileWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            File.Move(DotImport(e.OldFullPath), DotImport(e.FullPath));
            string xnbOld = GetXnb(e.OldFullPath), xnbNew = GetXnb(e.FullPath), mgcontentOld = GetMgcontent(e.OldFullPath), mgcontentNew = GetMgcontent(e.FullPath);
            if (File.Exists(xnbOld)) File.Move(xnbOld, xnbNew);
            if (File.Exists(mgcontentOld)) File.Move(mgcontentOld, mgcontentNew);
        }

        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            _buildRequests.Add(e.FullPath);
            ProcessBuildRequests();
        }

        private void FileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            InitImportingResource(e.FullPath);
            _buildRequests.Add(e.FullPath);
            ProcessBuildRequests();
        }

        private void FileWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            UnimportResource(e.FullPath);
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Read Guid or auto-generated new Guid and .import files
        /// </summary>
        /// <param name="resourcePath"></param>
        private void InitImportingResource(NonContextResourcePath uncheckedResourcePath)
        {
            var resourcePath = uncheckedResourcePath.CheckedBy(this);
            var dotImport = DotImport(resourcePath);
            Guid guid;
            if (File.Exists(dotImport)) // if .import exists
            {
                // read guid from .import
                guid = ReadGuidFromImportFile(resourcePath);
            }
            else // if don't
            {
                // Write new guid to .import
                guid = _guidManager.GenerateUniqueGuid();
                using (var writer = new StreamWriter(File.Create(dotImport)))
                {
                    writer.WriteLine(guid);
                    writer.Flush();
                }
            }

            _guidManager.ConsumeGuid(guid, resourcePath);
        }

        private void UnimportResource(NonContextResourcePath uncheckedResourcePath)
        {
            var resourcePath = uncheckedResourcePath.CheckedBy(this);
            _guidManager.RemoveValue(resourcePath);

            // clean .import filePath
            var dotImport = DotImport(resourcePath);
            if (File.Exists(dotImport))
            {
                File.Delete(dotImport);
                _guidManager.RemoveValue(resourcePath);
            }

            UnloadContent(uncheckedResourcePath);
        }

        private void ProcessBuildRequests()
        {
            // Filters build requests
            _buildRequests = _buildRequests.Where(e => IsContentSupported(e)).ToList();
            if (_buildRequests.Count == 0) return;

            string cmdArgs =
@"/workingDir:./..
/outputDir:bin
/intermediateDir:bin
/platform:Windows
/incremental
";
            foreach (var request in _buildRequests)
            {
                var resourcePath = request.CheckedBy(this);
                cmdArgs += $"/build:{resourcePath};{PathHelper.GetRelativePath(_resourceDir, resourcePath)}.xnb\n";
            }
            _buildRequests.Clear();

            // Create temporary MGCB filePath which built imported items
            var dotContent = Path.Combine(_resourceDir, ".content");
            var tempFile = Path.Combine(dotContent, "temp\\buildcontent.temp");
            var tempDir = Path.GetDirectoryName(tempFile);
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);
            File.WriteAllText(tempFile, cmdArgs, Encoding.UTF8);

            // Bulid temporary MGCB filePath
            BuildMGCBContent(tempFile);
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
            cmd.WaitForExit();
            cmd.Dispose();
        }

        /// <summary>
        /// Content value is .xna filePath value without extension
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        private void UnloadContent(NonContextResourcePath uncheckedResourcePath)
        {
            var xnb = GetXnb(uncheckedResourcePath);
            var mgcontent = GetMgcontent(uncheckedResourcePath);
            if (File.Exists(xnb)) File.Delete(xnb);
            if (File.Exists(mgcontent)) File.Delete(mgcontent);
            _guidManager.RemoveValue(xnb);
        }

        private ResourcePath GetXnb(NonContextResourcePath resourcePath)
        {
            var relativePath = GetResourceRelativePath(resourcePath);
            return new ResourcePath(this, Path.Combine(_contentOutputDir, relativePath + ".xnb"));
        }

        private string GetMgcontent(NonContextResourcePath resourcePath)
        {
            var relativePath = GetResourceRelativePath(resourcePath);
            return Path.Combine(_contentOutputDir, relativePath + ".mgcontent");
        }

        private string GetResourceRelativePath(NonContextResourcePath uncheckedResourcePath)
        {
            return PathHelper.GetRelativePath(
                _resourceDir,
                uncheckedResourcePath.CheckedBy(this));
        }

        private bool IsContentSupported(NonContextResourcePath resourcePath)
        {
            return ContentSupportedExtensions.Contains(Path.GetExtension(resourcePath.CheckedBy(this)).ToLower());
        }
        #endregion

        string _resourceDir;
        string _contentOutputDir;
        FileSystemWatcher _fileWatcher;
        GuidManager<ResourcePath> _guidManager;
        List<NonContextResourcePath> _buildRequests = new List<NonContextResourcePath>(); // resource files need to rebuild after being modified

        private static readonly string[] ContentSupportedExtensions = new string[]
        {
            ".jpg", ".png"
        };

        private static readonly string[] ResourceSupportedExtensions = new string[]
        {
            ".jpg", ".png", ".sf", ".scene"
        };

        #region Static Functions
        private static Guid ReadGuidFromImportFile(string resourcePath)
        {
            var dotImport = DotImport(resourcePath);
            if (!File.Exists(dotImport)) throw new FileNotFoundException(dotImport);
            using (var reader = new StreamReader(dotImport))
            {
                return new Guid(reader.ReadToEnd().Replace("\r\n", ""));
            }
        }

        private static string DotImport(string resourcePath)
        {
            if (Path.GetExtension(resourcePath) == ".import") return resourcePath;
            return resourcePath + ".import";
        }

        private static void Log(string msg)
        {
            Debug.WriteLine("[ResourceImporter] " + msg);
        }

        static readonly JsonSerializerSettings _SerializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
        #endregion
    }
}
