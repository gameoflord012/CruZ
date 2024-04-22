using CruZ.GameEngine.Serialization;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;

using MonoGame.Framework.Content.Pipeline.Builder;

using SharpDX.MediaFoundation;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

using PathHelper = CruZ.GameEngine.Utility.PathHelper;

namespace CruZ.GameEngine.Resource
{
    /// <summary>
    /// For external content pipeline to work, the dll must be presented in domain base dir
    /// </summary>
    public class ResourceManager : IGuidValueProcessor<string>
    {
        private const string REF_DIR_NAME = ".resourceref";

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
            _serializer.Converters.Add(new TransformEntityJsonConverter());
            _serializer.Converters.Add(new ComponentJsonConverter());
            _serializer.Converters.Add(new Vector4JsonConverter());
            _serializer.Converters.Add(new Vector2JsonConverter());
            _serializer.Options.MakeReadOnly();

            _guidManager = new(this);

            _pipelineManager = new(ResourceRoot, ContentOutputDir, ContentOutputDir);
            _pipelineManager.Platform = TargetPlatform.Windows;
            AddPipelineAssemblies();

            InitResourceDir();
        }

        private void AddPipelineAssemblies()
        {
            _pipelineManager.AddAssembly(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MonoGame.Aseprite.Content.Pipeline.dll"));
            _pipelineManager.AddAssembly(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MonoGame.Extended.Content.Pipeline.dll"));
        }

        private void InitResourceDir()
        {
            foreach (var filePath in DirectoryHelper.EnumerateFiles(ResourceRoot, [REF_DIR_NAME]))
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
                            ImportResourcePath(filePath);
                        }
                        break;
                }
            }

            // enumerates .xnb and .mgcontent files
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

        private static readonly string[] ContentSupportedExtensions =
        [
            ".jpg", ".png", ".spritefont", ".fx", ".aseprite", ".fnt"
        ];

        private static readonly string[] ResourceSupportedExtensions =
        [
            .. ContentSupportedExtensions,
            .. new string[]
            {
                ".sf", ".scene",
            },
        ];

        public void Create(string resourcePath, object resObj)
        {
            resourcePath = GetFormattedResourcePath(resourcePath);
            _serializer.SerializeToFile(resObj, Path.Combine(ResourceRoot, resourcePath));
            InitResourceInstance(resObj, resourcePath, true);
        }

        public bool TrySave(IResource resource)
        {
            if (resource.Info == null) return false;
            Create(resource.Info.ResourceName, resource);
            return true;
        }

        public T Load<T>(string resourcePath)
        {
            return (T)Load(resourcePath, typeof(T), out _);
        }

        public T Load<T>(string resourcePath, out ResourceInfo info)
        {
            return (T)Load(resourcePath, typeof(T), out info);
        }

        public T Load<T>(Guid guid)
        {
            return (T)Load(_guidManager.GetValue(guid), typeof(T), out _);
        }

        public T Load<T>(ResourceInfo resourceInfo)
        {
            return GetManagerFromReferencePath(resourceInfo.ReferencePath).Load<T>(resourceInfo.Guid);
        }

        /// <summary>
        /// Get <see cref="ResourceInfo"/> with given imported resource path
        /// </summary>
        public ResourceInfo RetriveResourceInfo(string resourcePath)
        {
            resourcePath = GetFormattedResourcePath(resourcePath);
            var relative = Path.GetRelativePath(ResourceRoot, resourcePath).Replace("/", "\\").AsSpan();
            StringBuilder sb = new();

            const string REF_DIR = $"{REF_DIR_NAME}\\";
            while (relative.StartsWith(REF_DIR))
            {
                relative = relative.Slice(REF_DIR.Length);
                sb.Append(REF_DIR);

                int slashIndex = relative.IndexOf("\\");

                sb.Append(relative.Slice(0, slashIndex + 1));
                relative = relative.Slice(0, slashIndex + 1);
            }

            try
            {
                var refPath = sb.ToString();
                ResourceManager manager = GetManagerFromReferencePath(refPath);

                return ResourceInfo.Create(manager._guidManager.GetGuid(resourcePath), resourcePath, refPath);

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

        private ResourceManager GetManagerFromReferencePath(string referencePath)
        {
            return 
                string.IsNullOrEmpty(referencePath) ? this :
                From(Path.Combine(ResourceRoot, referencePath));
        }

        string IGuidValueProcessor<string>.GetProcessedGuidValue(string value)
        {
            return GetFormattedResourcePath(value).ToLower();
        }

        /// <summary>
        /// Load resource with relative or full path, the resource fileName should within the .resourceInstance folder
        /// </summary>
        /// <returns></returns>
        private object Load(string resourcePath, Type ty, out ResourceInfo resourceInfo)
        {
            resourcePath = GetFormattedResourcePath(resourcePath);

            resourceInfo = RetriveResourceInfo(resourcePath);
            var refPath = resourceInfo.ReferencePath;

            ResourceManager manager = GetManagerFromReferencePath(refPath);

            object? resInstance;

            if (ContentSupportedExtensions.Contains(Path.GetExtension(resourcePath)))
            {
                resInstance = manager.LoadContentNonGeneric(resourcePath, ty);
            }
            else
            {
                resInstance = manager.LoadResource(resourcePath, ty);
            }

            InitResourceInstance(resInstance, resourceInfo);
            return resInstance;
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
            catch (JsonException)
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

            var contentFileName = resourceGuid.ToString();
            BuildContent(typeof(T), resourcePath, contentFileName);

            try
            {
                return GameApplication.GetContent().Load<T>(Path.Combine(ContentOutputDir, contentFileName));
            }
            catch
            {
                throw;
            }
        }

        private void BuildContent(Type ty, string resourcePath, string contentFileName)
        {
            if (!string.IsNullOrEmpty(Path.GetDirectoryName(contentFileName)))
                throw new ArgumentException("contentFileName not a file name");

            resourcePath = GetFormattedResourcePath(resourcePath);
            _pipelineManager.BuildContent(resourcePath, Path.Combine(ContentOutputDir, contentFileName), processorParameters: GetProcessorParam(ty));
            _pipelineManager.ContentStats.Write(ContentOutputDir);
        }

        private object LoadContentNonGeneric(string resourcePath, Type ty)
        {
            try
            {
                return typeof(ResourceManager).
                    GetMethod(nameof(LoadContent), BindingFlags.NonPublic | BindingFlags.Instance).
                    MakeGenericMethod(ty).
                    Invoke(this, [resourcePath]);
            }
            catch (Exception e)
            {
                throw new ContentLoadException($"Cannot load content {resourcePath}", e);
            }
        }

        private void InitResourceInstance(object resourceInstance, string resourcePath, bool autoImportResourcePath = false)
        {
            if (autoImportResourcePath) ImportResourcePath(resourcePath);
            InitResourceInstance(resourceInstance, RetriveResourceInfo(resourcePath));
        }

        private void InitResourceInstance(object resourceInstance, ResourceInfo resourceInfo)
        {
            if (resourceInstance is IResource resource)
                resource.Info = resourceInfo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourcePath">a subpath or full subpath of this resource dir</param>
        /// <returns>formatted</returns>
        private string GetFormattedResourcePath(string resourcePath)
        {
            resourcePath = Path.Combine(ResourceRoot, resourcePath);
            if (!PathHelper.IsSubPath(ResourceRoot, resourcePath))
                throw new ArgumentException($"Resource Path \"{resourcePath}\" must be a subpath of resource root \"{ResourceRoot}\"");
            return Path.GetFullPath(resourcePath);
        }

        /// <summary>
        /// Read Guid or auto-generated new Guid and .import files
        /// </summary>
        /// <param name="resourcePath"></param>
        private void ImportResourcePath(string resourcePath)
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

        //public ResourceManager CreateResourceReference(string referencePath)
        //{
        //    DirectoryInfo referenceDir = GetReferenceDir();
        //    referenceDir.MoveTo(referencePath);
        //    if (referenceDir.Exists) throw new InvalidOperationException("Reference already exists");
        //    referenceDir.Create();
        //    return From(referenceDir.FullName);
        //}

        public void UpdateReferenceData(ResourceManager resourceRef, string referenceId)
        {
            if(referenceId.Contains(REF_DIR_NAME))
                throw new ArgumentException($"referenceId should not contain special name '{REF_DIR_NAME}'");

            PathHelper.UpdateFolder(
                resourceRef.ResourceRoot,
                Path.Combine(ResourceRoot, REF_DIR_NAME, referenceId),
                "*", true, true);
        }

        private OpaqueDataDictionary GetProcessorParam(Type ty)
        {
            if (!_processorParams.ContainsKey(ty))
                _processorParams[ty] = [];
            return _processorParams[ty];
        }

        Dictionary<Type, OpaqueDataDictionary> _processorParams = [];

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

        string ContentOutputDir => $"{_resourceRoot}\\.content\\";
        string _resourceRoot = "res";

        Serializer _serializer;
        GuidManager<string> _guidManager;
        PipelineManager _pipelineManager;

        public static ResourceManager From(string resourceDir)
        {
            resourceDir = Path.GetFullPath(resourceDir);

            if (!_managers.ContainsKey(resourceDir))
                _managers[resourceDir] = new ResourceManager(resourceDir);

            return _managers[resourceDir];
        }
        static Dictionary<string, ResourceManager> _managers = [];
    }
}