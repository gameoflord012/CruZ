using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;

using static CruZ.Tools.ResourceImporter.ResourceImporter;

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
            if (IsConsumed(guid))
                throw new ArgumentException("Guid consumed");

            _getPathFromGuid[guid] = value;
            _getGuidFromPath[value] = guid;
        }

        public T GetValue(Guid guid)
        {
            if (!_getPathFromGuid.ContainsKey(guid))
                throw new ResourcePathNotFoundException($"Resource with guid \"{guid}\" is unavaiable or unimported");
            return _getPathFromGuid[guid];
        }

        public Guid GetGuid(T value)
        {
            if(!_getGuidFromPath.ContainsKey(value))
                throw new ResourceGuidNotFoundException($"Resource with path \"{value}\" is unavaiable or unimported");
            return _getGuidFromPath[value];
        }

        public void RemovedGuid(Guid guid)
        {
            if (!IsConsumed(guid))
                throw new ArgumentException("Guid not included");

            var path = _getPathFromGuid[guid];

            _getGuidFromPath.Remove(path);
            _getPathFromGuid.Remove(guid);
        }

        public void RemoveValue(T value)
        {
            var guid = _getGuidFromPath[value];

            if (!IsConsumed(guid))
                throw new ArgumentException("Path not included");

            _getGuidFromPath.Remove(value);
            _getPathFromGuid.Remove(guid);
        }

        private bool IsConsumed(Guid guid)
        {
            return _getPathFromGuid.ContainsKey(guid);
        }

        Dictionary<Guid, T> _getPathFromGuid = new Dictionary<Guid, T>();
        Dictionary<T, Guid> _getGuidFromPath = new Dictionary<T, Guid>();
    }

    /// <summary>
    /// Manage imported resources, its Guid and MGCB backends
    /// </summary>
    public class ResourceImporter
    {
        #region Inner Classes
        public struct NonContextResourcePath
        {
            private NonContextResourcePath(string resourcePath)
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
        /// Checked resource value is full value to resource file with unify format
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
            _contentRoot = Path.GetFullPath(Path.Combine(importDir, ".content\\bin"));
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
            /**
             * Steps:
             * 1. Remove excess .import
             * 2. Process reosurce which have .import
             * 3. Import unimport files:
             *      - Files can be built into content
             *      - Non content file
             */

            // iterate through last session import items
            foreach (var filePath in Directory.EnumerateFiles(_resourceDir, "*.*", SearchOption.AllDirectories))
            {
                // Pocess excess .import files
                if(Path.GetExtension(filePath) == ".import")
                {
                    var resourceFile = filePath.Remove(filePath.Length - ".import".Length);
                    if (!File.Exists(resourceFile))
                    {
                        UnimportedItem(resourceFile);
                    }
                }
                // Import file can be built into content
                else if(ContentSupportedExtensions.Contains(Path.GetExtension(filePath).ToLower()))
                {
                    InitImportingResourceGuid(filePath);
                    _buildRequests.Add(filePath);
                }
                // If it's non content
                else if(NonContentSupportedExtensions.Contains(Path.GetExtension(filePath).ToLower()))
                {
                    InitImportingResourceGuid(filePath);
                }
            }

            ProcessBuildRequests();
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
        #endregion

        #region FileWatcher Functions
        // TODO: implement filePath watcher
        public void EnableWatch() { _fileWatcher.EnableRaisingEvents = true; }

        public void DisableWatch() { _fileWatcher.EnableRaisingEvents = false; }

        private void FileWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            File.Move(DotImport(e.OldFullPath), DotImport(e.FullPath));
            string xnbOld = GetXnb(e.OldFullPath), xnbNew = GetXnb(e.FullPath), mgcontentOld = GetMgcontent(e.OldFullPath), mgcontentNew = GetMgcontent(e.FullPath);
            if(File.Exists(xnbOld)) File.Move(xnbOld, xnbNew);
            if(File.Exists(mgcontentOld)) File.Move(mgcontentOld, mgcontentNew);
        }

        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            _buildRequests.Add(e.FullPath);
            ProcessBuildRequests();
        }

        private void FileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            InitImportingResourceGuid(e.FullPath);
            _buildRequests.Add(e.FullPath);
            ProcessBuildRequests();
        }

        private void FileWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            UnimportedItem(e.FullPath);
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Read Guid or auto-generated new Guid and .import files
        /// </summary>
        /// <param name="resourcePath"></param>
        private void InitImportingResourceGuid(NonContextResourcePath uncheckedResourcePath)
        {
            var resourcePath = uncheckedResourcePath.CheckedBy(this);
            var dotImport = DotImport(resourcePath);
            Guid guid;
            if (File.Exists(dotImport))
            {
                // read guid from .import
                guid = ReadGuidFromImportFile(resourcePath);
            }
            else
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
        
        private void UnimportedItem(NonContextResourcePath uncheckedResourcePath)
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
            string cmdArgs =
@"/workingDir:./..
/outputDir:bin
/intermediateDir:bin/obj
/platform:Windows
/incremental
";
            foreach (var resourcePath in _buildRequests)
            {
                cmdArgs += $"/build:{resourcePath}\n";
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

            // Rebuild predefined MGCB filePath
            var preExistMgcbFile = Path.Combine(dotContent, "Content.mgcb");
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

        /// <summary>
        /// Get relative NonContextResourcePath from resource root with unify formmat
        /// </summary>
        /// <param name="uncheckedResourcePath"></param>
        /// <returns></returns>

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

        /// <summary>
        /// Content value is value relative to resource root without extension
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        private string GetContentPath(NonContextResourcePath uncheckedResourcePath)
        {
            var noExtension = PathHelper.GetPathWithoutExtension(uncheckedResourcePath.CheckedBy(this));
            return PathHelper.GetRelativePath(_resourceDir, noExtension);
        }

        private ResourcePath GetXnb(NonContextResourcePath resourcePath)
        {
            var content = GetContentPath(resourcePath);
            return new ResourcePath(this, Path.Combine(_contentRoot, content + ".xnb"));
        }

        private string GetMgcontent(NonContextResourcePath resourcePath)
        {
            var content = GetContentPath(resourcePath);
            return Path.Combine(_contentRoot, content + ".mgcontent");
        }
        #endregion

        string _resourceDir;
        string _contentRoot;
        FileSystemWatcher _fileWatcher;
        GuidManager<ResourcePath> _guidManager;
        List<string> _buildRequests = new List<string>(); // resource files need to rebuild after being modified

        private static readonly string[] ContentSupportedExtensions = new string[]
        {
            ".jpg", ".png"
        };

        private static readonly string[] NonContentSupportedExtensions = new string[]
        {
            ".xnb", ".sf", ".scene"
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
