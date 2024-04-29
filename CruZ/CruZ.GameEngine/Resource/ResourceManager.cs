using AsepriteDotNet.Aseprite;

using CruZ.GameEngine.Serialization;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.BitmapFonts;
using MonoGame.Framework.Content.Pipeline.Builder;

using System;
using System.Collections.Generic;
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
    public class ResourceManager
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

            //_serializer.Converters.Add(new TextureAtlasJsonConverter(this));
            _serializer.Converters.Add(new TransformEntityJsonConverter());
            _serializer.Converters.Add(new ComponentJsonConverter());
            _serializer.Converters.Add(new Vector4JsonConverter());
            _serializer.Converters.Add(new Vector2JsonConverter());
            _serializer.Options.MakeReadOnly();

            _pipelineManager = new(ResourceRoot, ContentOutputDir, ContentOutputDir);
            _pipelineManager.Platform = TargetPlatform.Windows;
            AddPipelineAssemblies();

            PrepareResourceDir();
        }

        private void AddPipelineAssemblies()
        {
            _pipelineManager.AddAssembly(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MonoGame.Aseprite.Content.Pipeline.dll"));
            _pipelineManager.AddAssembly(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MonoGame.Extended.Content.Pipeline.dll"));
            _pipelineManager.AddAssembly(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MonoGame.Extended.dll"));
        }

        private void PrepareResourceDir()
        {
            if(!Path.Exists(ContentOutputDir)) return;

            foreach (var filePath in Directory.EnumerateFiles(ContentOutputDir, "*.*", SearchOption.AllDirectories).
                Where(e =>
                    Path.GetFileName(e) != ".mgcontent" &&
                    (e.EndsWith(".xnb") || e.EndsWith(".mgcontent"))))
            {
                // delete if file name is not guid or resource guid doesn't exists
                var relative = Path.GetRelativePath(ContentOutputDir, filePath);
                var resourceFile = Path.Combine(ResourceRoot, Path.ChangeExtension(filePath, null));
                
                if(!File.Exists(resourceFile))
                {
                    File.Delete(filePath);
                }
            }
        }

        private static readonly string[] ContentSupportedExtensions =
        [
            ".jpg", ".png", // texture file
            ".fx", 
            ".aseprite", 
            ".fnt"
        ];

        private static readonly Type[] ContentSupportedTypes =
        [
            typeof(Texture2D),
            typeof(AsepriteFile),
            typeof(Effect),
            typeof(BitmapFont)
        ];

        private static readonly string[] ResourceSupportedExtensions =
        [
            .. ContentSupportedExtensions,
            .. new string[]
            {
                ".sf", ".scene",
            },
        ];

        public T Load<T>(string resourcePath)
        {
            return (T)Load(resourcePath, typeof(T));
        }

        public T LoadContent<T>(string contentName)
        {
            var content = GameApplication.GetContentManager();
            content.AssetNameResolver = null;
            content.RootDirectory = ContentDir;

            T loaded = content.Load<T>(contentName);

            content.AssetNameResolver = default;
            content.RootDirectory = default;

            return loaded;
        }

        /// <summary>
        /// Load resource with relative or full path, the resource fileName should within the .resourceInstance folder
        /// </summary>
        /// <returns></returns>
        private object Load(string resourcePath, Type ty)
        {
            resourcePath = GetFormattedResourcePath(resourcePath);

            //ResourceManager manager = GetManagerFromReferencePath(refPath);

            object? returnResource;

            if (ContentSupportedTypes.Contains(ty))
            {
                returnResource = LoadContentNonGeneric(resourcePath, ty);
            }
            else
            {
                returnResource = LoadResource(resourcePath, ty);
            }

            //InitResourceInstance(returnResource, resourceInfo);
            return returnResource;
        }

        private object LoadResource(string resourcePath, Type ty)
        {
            throw new NotImplementedException();

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

        private T LoadBuiltContent<T>(string resourcePath)
        {
            resourcePath = GetFormattedResourcePath(resourcePath);
            var content = GameApplication.GetContentManager();

            // Setup content context
            content.RootDirectory = ContentOutputDir;
            content.AssetNameResolver = ResolveAssetName;
            
            var loaded = content.Load<T>(resourcePath);
            
            // Return content context to default
            content.AssetNameResolver = default;
            content.RootDirectory = default;
            
            return loaded;
        }

        private string ResolveAssetName(string assetName, Type assetType, ContentManager content)
        {
            var resourcePath = GetFormattedResourcePath(assetName);

            BuildContent(assetType, resourcePath);

            return GetContentPathFromResourcePath(resourcePath);
        }

        private void BuildContent(Type ty, string resourcePath)
        {
            resourcePath = GetFormattedResourcePath(resourcePath);
            var contentPath = GetContentPathFromResourcePath(resourcePath) + ".xnb";
            _pipelineManager.BuildContent(resourcePath, contentPath, processorParameters: GetProcessorParam(ty));
            _pipelineManager.ContentStats.Write(ContentOutputDir);
        }

        private string GetContentPathFromResourcePath(string resourcePath)
        {
            resourcePath = GetFormattedResourcePath(resourcePath);
            var relative = Path.GetRelativePath(ResourceRoot, resourcePath);
            return Path.Combine(ContentOutputDir, relative);
        }

        private object LoadContentNonGeneric(string resourcePath, Type ty)
        {
            try
            {
                return typeof(ResourceManager).
                    GetMethod(nameof(LoadBuiltContent), BindingFlags.NonPublic | BindingFlags.Instance)!.
                    MakeGenericMethod(ty).
                    Invoke(this, [resourcePath])!;
            }
            catch (Exception e)
            {
                throw new ContentLoadException($"Cannot load content {resourcePath}", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourcePath">a subpath or full subpath of this resource dir</param>
        /// <returns>formatted</returns>
        private string GetFormattedResourcePath(string resourcePath)
        {
            resourcePath = Path.Combine(ResourceRoot, resourcePath);
            if (!PathHelper.IsSubpath(ResourceRoot, resourcePath))
                throw new ArgumentException($"Resource Path \"{resourcePath}\" must be a subpath of resource root \"{ResourceRoot}\"");
            return Path.GetFullPath(resourcePath);
        }

        public void CopyResourceData(ResourceManager resourceRef, string relativeDestination)
        {
            if(!PathHelper.IsRelativeASubpath(relativeDestination)) throw new ArgumentException(relativeDestination);

            PathHelper.UpdateFolder(
                resourceRef.ResourceRoot,
                Path.Combine(ResourceRoot, relativeDestination),
                "*", true, true);
        }

        private OpaqueDataDictionary GetProcessorParam(Type ty)
        {
            if (!_processorParams.ContainsKey(ty))
                _processorParams[ty] = [];
            return _processorParams[ty];
        }

        Dictionary<Type, OpaqueDataDictionary> _processorParams = [];

        public string ContentDir => $"{_resourceRoot}\\Content";
        public string ContentOutputDir => $"{ContentDir}\\.build";

        string _resourceRoot = "";

        Serializer _serializer;
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