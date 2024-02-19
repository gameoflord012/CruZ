using CruZ.Exception;
using CruZ.Serialization;
using CruZ.Tool.ResourceImporter;
using CruZ.Utility;

using Microsoft.Xna.Framework.Content;

using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CruZ.Resource
{
    public static class ResourceManager
    {
        public static string ResourceRoot { 
            get => _resourceRoot; 
            set 
            { 
                _resourceRoot = value;
            } 
        }

        static ResourceManager()
        {
            _Serializer = new Serializer();
            _Serializer.Converters.Add(new TextureAtlasJsonConverter());
            _Serializer.Converters.Add(new SerializableJsonConverter());
        }

        internal static void RunImport()
        {
            var dotImporter  = Path.Combine(ResourceRoot, ".resourceImporter");
            var importerObject = ResourceImporter.ReadImporterObject(dotImporter);

            ResourceImporter.ResourceRoot = ResourceRoot;
            ResourceImporter.SetImporterObject(importerObject);
            ResourceImporter.DoBuild();
            ResourceImporter.ExportResult();
            
            Logging.SetMsg(importerObject.BuildLog, "ResourceImporter", true);
            _GetResourcePathFromGuid = importerObject.BuildResult;
        }

        public static void CreateResource(string resourcePath, object resObj, bool renew = false)
        {
            object? existsResource = null;

            if (!renew)
            {
                try
                {
                    existsResource = LoadResource(resourcePath, resObj.GetType());
                }
                catch
                {
                    Logging.PushMsg(
                        $"Failed to load resource \"{resourcePath}\", new one will be created", "ResourceManager");
                }
            }

            if (existsResource == null)
            {
                _Serializer.SerializeToFile(resObj, Path.Combine(ResourceRoot, resourcePath));
            }

            if (existsResource is IDisposable iDisposable)
                iDisposable.Dispose();

            InitResourceHost(resObj, resourcePath);
        }

        public static void SaveResource(IHostResource host)
        {
            if (host.ResourceInfo.IsRuntime)
                throw new ArgumentException($"Can't save runtime {host} resource, use create resource instead");

            CreateResource(host.ResourceInfo.ResourceName, host, true);
        }

        public static T LoadResource<T>(string resourcePath)
        {
            return (T)LoadResource(resourcePath, typeof(T));
        }

        public static T LoadResource<T>(string resourcePath, out ResourceInfo resInfo)
        {
            resInfo = CreateResourceInfo(resourcePath);
            return (T)LoadResource(resourcePath, typeof(T));
        }

        public static T LoadResource<T>(Guid guid)
        {
            return LoadResource<T>(GetResourcePath(guid));
        }

        /// <summary>
        /// Load resource with relative or full path, the resource file should within the .resObj folder
        /// </summary>
        /// <param name="resourcePath">May be fullpath or relative path to resource root</param>
        /// <param name="ty"></param>
        /// <returns></returns>
        private static object LoadResource(string resourcePath, Type ty)
        {
            var fullResourcePath = Path.Combine(ResourceRoot, resourcePath);
            var relResourcePath = Path.GetRelativePath(ResourceRoot, fullResourcePath);

            if (!PathHelper.IsSubPath(ResourceRoot, fullResourcePath))
            {
                throw new ArgumentException($"Resource Path \"{resourcePath}\" must be a subpath of resource root \"{ResourceRoot}\"");
            }

            object resObj;

            try
            {
                var dir = Path.GetDirectoryName(relResourcePath);
                var file = Path.GetFileNameWithoutExtension(relResourcePath);

                if(dir == null || file == null)
                    throw new ArgumentException($"Invalid resourcePath value {relResourcePath}");

                resObj = LoadContentNonGeneric(Path.Combine(dir, file), ty);
            }
            catch (ContentLoadException)
            {
                try
                {
                    resObj = _Serializer.DeserializeFromFile(fullResourcePath, ty);
                }
                catch (FileNotFoundException)
                {
                    throw new FileNotFoundException(string.Format("Can't find resource file {0}", fullResourcePath));
                }
                catch(JsonReaderException)
                {
                    throw new LoadResourceFailedException($"Can't load resource \"{fullResourcePath}\" due to invalid resource formatting or not available in content");
                }
            }

            InitResourceHost(resObj, resourcePath);

            return resObj;
        }

        private static string GetFullResPath()
        {
            return Path.GetFullPath(ResourceRoot);
        }

        private static T LoadContent<T>(string resourcePath)
        {
            try
            {
                if (typeof(T) == typeof(SpriteSheet))
                {
                    return GameApplication.GetContent().Load<T>(
                        ContentRoot + "\\" + resourcePath, new JsonContentLoader());
                }

                return GameApplication.GetContent().Load<T>(
                    ContentRoot + "\\" + resourcePath);
            }
            catch (FileNotFoundException)
            {
                throw new ContentLoadException();
            }
            catch (ContentLoadException)
            {
                throw;
            }
        }

        private static object LoadContentNonGeneric(string resourcePath, Type ty)
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.
                return typeof(ResourceManager).
                    GetMethod("LoadContent", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).
                    MakeGenericMethod(ty).
                    Invoke(null, BindingFlags.DoNotWrapExceptions, null, [resourcePath], null);
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            catch
            {
                throw;
            }
        }

        private static string GetResourcePath(Guid guid)
        {
            try
            {
                return _GetResourcePathFromGuid[guid.ToString()];
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Can't find resource with guid " + guid);
            }
        }

        private static void InitResourceHost(object resObj, string resourcePath)
        {
            if (resObj is IHostResource host)
                host.ResourceInfo = CreateResourceInfo(resourcePath);
        }

        private static ResourceInfo CreateResourceInfo(string resourcePath)
        {
            return ResourceInfo.Create(resourcePath, false);
        }

        private static Serializer _Serializer;
        private static Dictionary<string, string> _GetResourcePathFromGuid = [];

        private static string _resourceRoot = "res";
        private static string ContentRoot => $"{_resourceRoot}\\.content\\bin";

    }
}