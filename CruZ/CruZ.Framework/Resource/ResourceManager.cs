using CruZ.Common.GameSystem.Resource;
using CruZ.Common.Serialization;
using CruZ.Common.Service;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Framework.Content.Pipeline.Builder;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CruZ.Common.Resource
{
    public class ResourceManager : ICustomSerializable, IGuidValueProcessor<string>
    {
        public string ResourceRoot
        {
            get => _resourceRoot;
            private set
            {
                _resourceRoot = Path.GetFullPath(value);
            }
        }

        private ResourceManager(string resourceRoot)
        {
            _serializer = new Serializer();
            ResourceRoot = resourceRoot;

            Directory.CreateDirectory(Path.GetDirectoryName(ResourceRoot) ?? throw new ArgumentException("resourceRoot"));
            Directory.CreateDirectory(Path.GetDirectoryName(ContentOutputDir) ?? throw new ArgumentException("resourceRoot"));

            _serializer.Converters.Add(new TextureAtlasJsonConverter(this));
            _serializer.Converters.Add(new SerializableJsonConverter());

            _guidManager = new(this);

            _pipelineManager = new(ResourceRoot, ContentOutputDir, ContentOutputDir);
            _pipelineManager.Platform = TargetPlatform.Windows;

            InitResourceDir();

            //SetProcessorParam(typeof(Effect), "DebugMode", "Auto");
            //_importer.EnableWatch();
        }

        private void InitResourceDir()
        {
            foreach (var filePath in Directory.EnumerateFiles(ResourceRoot, "*.*", SearchOption.AllDirectories))
            {
                var extension = Path.GetExtension(filePath);
                switch (extension)
                {
                    // remove excess .import files
                    case ".import":
                        var resourceFile = filePath.Substring(0, filePath.LastIndexOf(extension));
                        if (!File.Exists(resourceFile)) File.Delete(filePath);
                        break;

                    default:
                        // initialize resource if filePath is a resource
                        if (ResourceSupportedExtensions.Contains(Path.GetExtension(filePath).ToLower()))
                        {
                            InitImportingResource(filePath);
                        }
                        break;
                }
            }

            foreach (var filePath in Directory.EnumerateFiles(ContentOutputDir, "*.*", SearchOption.AllDirectories).
                Where(e =>
                    Path.GetFileName(e) != ".mgcontent" &&
                    (e.EndsWith(".xnb") || e.EndsWith(".mgcontent"))))
            {
                // delete if file name is not guid or resource guid doesn't exists
                if (!Guid.TryParse(Path.GetFileNameWithoutExtension(filePath), out Guid guid) || !_guidManager.IsConsumed(guid))
                    File.Delete(filePath);
            }
        }

        #region Public Functions
        public void Create(string resourcePath, object resObj, bool replaceIfExists = false)
        {
            resourcePath = GetFormattedResourcePath(resourcePath);
            object? existsResource = null;

            if (!replaceIfExists)
            {
                try
                {
                    existsResource = Load(resourcePath, resObj.GetType());
                }
                catch
                {
                    LogService.PushMsg(
                        $"Failed to load resource \"{resourcePath}\", new one will be created", "ResourceManager");
                }
            }

            if (existsResource == null)
            {
                _serializer.SerializeToFile(resObj, Path.Combine(ResourceRoot, resourcePath));
            }

            if (existsResource is IDisposable iDisposable)
                iDisposable.Dispose();

            InitResourceHost(resObj, resourcePath);
        }

        public void Save(IHostResource host)
        {
            if (host.ResourceInfo == null)
                throw new ArgumentException($"Can't save {host} resource, use create resource first");

            Create(host.ResourceInfo.ResourceName, host, true);
        }

        public T Load<T>(string resourcePath)
        {
            return (T)Load(resourcePath, typeof(T));
        }

        public T Load<T>(Guid guid)
        {
            return (T)Load(_guidManager.GetValue(guid), typeof(T));
        }

        public T Load<T>(ResourceInfo resourceInfo)
        {
            return Load<T>(resourceInfo.Guid);
        }

        public ResourceInfo RetriveResourceInfo(string resourcePath)
        {
            resourcePath = GetFormattedResourcePath(resourcePath);
            try
            {
                return ResourceInfo.Create(_guidManager.GetGuid(resourcePath), resourcePath);
            }
            catch (InvalidGuidException)
            {
                throw new ArgumentException($"Resource \"{resourcePath}\" maybe unimported");
            }
            catch (InvalidGuidValueException)
            {
                throw new ArgumentException($"Resource \"{resourcePath}\" maybe unimported");
            }
        }
        #endregion

        #region Interface Implementations
        public string GetProcessedGuidValue(string value)
        {
            return GetFormattedResourcePath(value);
        }

        public object ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            JObject jO = JObject.Load(reader);
            var root = jO[nameof(ResourceRoot)].Value<string>();
            return From(root);
        }

        public void WriteJson(JsonWriter writer, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(ResourceRoot));
            writer.WriteValue(ResourceRoot);
            writer.WriteEnd();
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load resource with relative or full path, the resource fileName should within the .resObj folder
        /// </summary>
        /// <param root="resourcePath">May be fullpath or relative path to resource root</param>
        /// <param root="ty"></param>
        /// <returns></returns>
        private object Load(string resourcePath, Type ty)
        {
            object? resObj;

            if (ContentSupportedTypes.Contains(ty))
            {
                resObj = LoadContentNonGeneric(resourcePath, ty);
            }
            else
            {
                resObj = LoadResource(resourcePath, ty);
            }

            InitResourceHost(resObj, resourcePath);
            return resObj;
        }

        private object LoadResource(string resourcePath, Type ty)
        {
            resourcePath = GetFormattedResourcePath(resourcePath);
            var fullResourcePath = Path.Combine(ResourceRoot, resourcePath);

            object? resObj;
            try
            {
                resObj = _serializer.DeserializeFromFile(fullResourcePath, ty);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException(string.Format("Can't find resource file {0}", fullResourcePath));
            }
            catch (JsonReaderException)
            {
                throw new LoadResourceFailedException($"Can't load resource \"{fullResourcePath}\" due to invalid resource formatting or not available in content");
            }

            return resObj;
        }

        private T LoadContent<T>(string resourcePath)
        {
            resourcePath = GetFormattedResourcePath(resourcePath);
            Guid resourceGuid;
            try
            {
                resourceGuid = _guidManager.GetGuid(resourcePath);
            }
            catch (InvalidGuidValueException)
            {
                throw new ContentLoadException($"resource \"{resourcePath}\" is invalid or unimported");
            }

            var contentPath = ContentOutputDir + "\\" + resourceGuid;

            BuildContent(typeof(T), resourcePath, contentPath);

            try
            {
                return GameApplication.GetContent().Load<T>(contentPath);
            }
            catch
            {
                throw;
            }
        }

        private void BuildContent(Type ty, string resourcePath, string? outputFilePath = null)
        {
            resourcePath = GetFormattedResourcePath(resourcePath);
            _pipelineManager.BuildContent(resourcePath, outputFilePath, processorParameters: GetProcessorParam(ty));
        }

        private object LoadContentNonGeneric(string resourcePath, Type ty)
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.
                return typeof(ResourceManager).
                    GetMethod(nameof(LoadContent), BindingFlags.NonPublic | BindingFlags.Instance).
                    MakeGenericMethod(ty).
                    Invoke(this, [resourcePath]);
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            catch (Exception e)
            {
                throw new ContentLoadException($"Cannot load content {resourcePath}", e);
            }
        }

        private void InitResourceHost(object resObj, string resourcePath)
        {
            var info = RetriveResourceInfo(resourcePath);
            if (resObj is IHostResource host) host.ResourceInfo = info;
        }

        private string GetFormattedResourcePath(string resourcePath)
        {
            resourcePath = Path.Combine(ResourceRoot, resourcePath);
            if (!Utility.PathHelper.IsSubPath(ResourceRoot, resourcePath))
                throw new ArgumentException($"Resource Path \"{resourcePath}\" must be a subpath of resource root \"{ResourceRoot}\"");
            return Path.GetFullPath(resourcePath);
        }

        /// <summary>
        /// Read Guid or auto-generated new Guid and .import files
        /// </summary>
        /// <param name="resourcePath"></param>
        private void InitImportingResource(string resourcePath)
        {
            resourcePath = GetFormattedResourcePath(resourcePath);
            Guid guid;

            if (TryReadGuidFromImportFile(resourcePath, out guid)) // if .import exists
            {

            }
            else // if don't
            {
                // Write new guid to .import
                guid = _guidManager.GenerateUniqueGuid();
                using (var writer = new StreamWriter(File.Create(resourcePath + ".import")))
                {
                    writer.WriteLine(guid);
                    writer.Flush();
                }
            }

            _guidManager.ConsumeGuid(guid, resourcePath);
        }

        private string GetXnb(string rawResourcePath)
        {
            return Path.Combine(ContentOutputDir, _guidManager.GetGuid(rawResourcePath) + ".xnb");
        }

        private string GetMgcontent(string rawResourcePath)
        {
            return Path.Combine(ContentOutputDir, _guidManager.GetGuid(rawResourcePath) + ".mgcontent");
        }

        private void SetProcessorParam(Type ty, string key, string value)
        {
            var @params = GetProcessorParam(ty);
            @params.Add(key, value);
        }

        private OpaqueDataDictionary GetProcessorParam(Type ty)
        {
            if (!_processorParams.ContainsKey(ty))
                _processorParams[ty] = [];
            return _processorParams[ty];
        }

        /// <summary>
        /// Get .import file from normal file
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        private static bool TryReadGuidFromImportFile(string filePath, out Guid guid)
        {
            var dotImport = filePath + ".import";
            guid = default;
            if (!File.Exists(dotImport)) return false;
            return Guid.TryParse(File.ReadLines(dotImport).First(), out guid);
        }
        #endregion

        #region Private Variables
        Serializer _serializer;
        GuidManager<string> _guidManager;
        string _resourceRoot = "res";
        string ContentOutputDir => $"{_resourceRoot}\\.content\\";
        PipelineManager _pipelineManager;

        Dictionary<Type, OpaqueDataDictionary> _processorParams = [];

        private static readonly Type[] ContentSupportedTypes =
        {
            typeof(Texture2D),
            typeof(SpriteFont),
            typeof(Effect)
        };

        private static readonly string[] ContentSupportedExtensions =
        [
            ".jpg", ".png", ".spritefont", ".fx"
        ];

        private static readonly string[] ResourceSupportedExtensions =
        [
            .. ContentSupportedExtensions,
            .. new string[]
            {
                ".sf", ".scene",
            },
        ];
        #endregion

        #region Static
        public static ResourceManager From(string resourceDir)
        {
            resourceDir = Path.GetFullPath(resourceDir);

            if (!_managers.ContainsKey(resourceDir))
                _managers[resourceDir] = new ResourceManager(resourceDir);

            return _managers[resourceDir];
        }
        static Dictionary<string, ResourceManager> _managers = [];
        #endregion
    }
}