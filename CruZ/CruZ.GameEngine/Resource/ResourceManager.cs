using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content.Pipeline;

using MonoGame.Framework.Content.Pipeline.Builder;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        string _resourceRoot = "";

        private ResourceManager(string resourceRoot)
        {
            ResourceRoot = resourceRoot;

            Directory.CreateDirectory(Path.GetDirectoryName(ResourceRoot) ?? throw new ArgumentException("resourceRoot"));
            Directory.CreateDirectory(Path.GetDirectoryName(ContentOutputDir) ?? throw new ArgumentException("resourceRoot"));

            _pipelineManager = new(ResourceRoot, ContentOutputDir, ContentOutputDir);
            _pipelineManager.Platform = TargetPlatform.Windows;
            _pipelineManager.Logger = new DebugContentBuildLogger();
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
            //foreach (var filePath in DirectoryHelper.EnumerateFiles(ResourceRoot, [ContentDir]).
            //    Where(e => ContentSupportedExtensions.Contains(Path.GetExtension(e))))
            //{
            //    BuildContent(filePath);
            //}

            if (!Path.Exists(ContentOutputDir)) return;
            //
            // remove excess .xnb and .mgcontent files in .build/ directory
            //
            foreach (string filePath in Directory.EnumerateFiles(ContentOutputDir, "*.*", SearchOption.AllDirectories).
                Where(e =>
                    Path.GetFileName(e) != Path.GetExtension(e) &&
                    (e.EndsWith(".xnb") || e.EndsWith(".mgcontent"))))
            {
                // delete if file name is not guid or resource guid doesn't exists
                var relative = Path.GetRelativePath(ContentOutputDir, filePath);
                var resourceFile = Path.Combine(ResourceRoot, Path.ChangeExtension(relative, null));

                if (!File.Exists(resourceFile))
                {
                    File.Delete(filePath);
                }
            }
        }

        public T Load<T>(string resourcePath, bool useLoadNew = true)
        {
            var content = GameApplication.GetContentManager();
            return content.LoadFromRoot<T>(resourcePath, ContentOutputDir, ContentResolver, useLoadNew);
        }

        private string ContentResolver(string assetName, Type assetType)
        {
            return BuildContent(assetName, assetType);
        }
        
        private string BuildContent(string resourcePath, Type assetType)
        {
            resourcePath = GetFormattedResourcePath(resourcePath);
            var relative = Path.GetRelativePath(ResourceRoot, resourcePath);
            var contentPath = Path.Combine(ContentOutputDir, relative);

            var processor = GetProcessor(assetType);
            if(!string.IsNullOrEmpty(processor)) contentPath += "-" + processor;

            _pipelineManager.BuildContent(resourcePath, contentPath + ".xnb", null, processor);
            _pipelineManager.ContentStats.Write(ContentOutputDir);

            return contentPath;
        }

        private string? GetProcessor(Type assetType)
        {
            if(assetType == typeof(SoundEffect))
            {
                return "SoundEffectProcessor";
            }

            return default;
        }

        private string GetFormattedResourcePath(string resourcePath)
        {
            resourcePath = Path.Combine(ResourceRoot, resourcePath);
            if (!PathHelper.IsSubpath(ResourceRoot, resourcePath)) throw new ArgumentException($"Resource Path \"{resourcePath}\" must be a subpath of resource root \"{ResourceRoot}\"");
            return Path.GetFullPath(resourcePath);
        }

        public void CopyResourceData(ResourceManager resourceRef, string relativeDestination)
        {
            if (!PathHelper.IsRelativeASubpath(relativeDestination)) throw new ArgumentException(relativeDestination);

            PathHelper.UpdateFolder(
                resourceRef.ResourceRoot,
                Path.Combine(ResourceRoot, relativeDestination),
                [".build"]);
        }

        PipelineManager _pipelineManager;

        public string ContentDir => $"{_resourceRoot}\\Content";
        public string ContentOutputDir => $"{ContentDir}\\.build";

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