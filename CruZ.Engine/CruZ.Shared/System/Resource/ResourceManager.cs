using Assimp.Configs;
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
using System.Linq;
using System.Linq.Expressions;
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

        public static void RunImport()
        {
            var dotImporter  = Path.Combine(ResourceRoot, ".resourceImporter");
            var importerObject = ResourceImporter.ReadImporterObject(dotImporter);

            ResourceImporter.ResourceRoot = ResourceRoot;
            ResourceImporter.SetImporterObject(importerObject);
            ResourceImporter.DoBuild();
            ResourceImporter.ExportResult();
            
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
                    Logging.PushMsg("Failed to load resource \"{0}\", new one will be created", resourcePath);
                }
            }

            if (existsResource == null)
            {
                _Serializer.SerializeToFile(resObj, Path.Combine(ResourceRoot, resourcePath));
            }

            if (existsResource is IDisposable idispose)
                idispose.Dispose();

            InitResourceHost(resObj, resourcePath);
        }

        public static void SaveResource(IHostResource hostRes)
        {
            if (hostRes.ResourceInfo.IsRuntime)
                throw new ArgumentException($"Can't save runtime {hostRes} resource, use create resource instead");

            CreateResource(hostRes.ResourceInfo.ResourceName, hostRes, true);
        }

        public static T LoadResource<T>(string resourcePath)
        {
            return (T)LoadResource(resourcePath, typeof(T));
        }

        public static T LoadResource<T>(string resPath, out ResourceInfo resInfo)
        {
            resInfo = CreateResourceInfo(resPath);
            return (T)LoadResource(resPath, typeof(T));
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
            object resObj;

            try
            {
                var dir = Path.GetDirectoryName(resourcePath);
                var file = Path.GetFileNameWithoutExtension(resourcePath);

                if(dir == null || file == null)
                    throw new ArgumentException($"Invalid resourcePath value {resourcePath}");

                resObj = LoadContentNonGeneric(Path.Combine(dir, file), ty);
            }
            catch (ContentLoadException)
            {
                try
                {
                    resObj = _Serializer.DeserializeFromFile(Path.Combine(ResourceRoot, resourcePath), ty);
                }
                catch (FileNotFoundException)
                {
                    throw new FileNotFoundException(string.Format("Can't find resource file {0}", resourcePath));
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

        private static ResourceInfo CreateResourceInfo(string resPath)
        {
            return ResourceInfo.Create(resPath, false);
        }

        private static Serializer _Serializer;
        private static Dictionary<string, string> _GetResourcePathFromGuid = [];

        private static string _resourceRoot = "res";
        private static string ContentRoot => $"{_resourceRoot}\\Content\\bin";

    }
}