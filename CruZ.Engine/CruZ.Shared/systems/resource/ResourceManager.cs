using CruZ.Exception;
using CruZ.Serialization;
using CruZ.Service;
using CruZ.Tools.ResourceImporter;
using CruZ.Utility;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;

namespace CruZ.Resource
{
    public class ResourceManager : ICustomSerializable
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

            _serializer.Converters.Add(new TextureAtlasJsonConverter(this));
            _serializer.Converters.Add(new SerializableJsonConverter());

            _importer = new(resourceRoot);
            _importer.LoadImportedItems();
        }

        #region Public Functions
        public void Create(string resourcePath, object resObj, bool renew = false)
        {
            resourcePath = GetCheckedResourcePath(resourcePath);

            object? existsResource = null;

            if (!renew)
            {
                try
                {
                    existsResource = LoadResource(resourcePath, resObj.GetType());
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
            if (host.ResourceInfo.ResourceManager == null)
                throw new ArgumentException($"Can't save {host} resource, use create resource first");

            Create(host.ResourceInfo.ResourceName, host, true);
        }

        public T Load<T>(string resourcePath)
        {
            return (T)LoadResource(resourcePath, typeof(T));
        }

        public T Load<T>(string resourcePath, out ResourceInfo resInfo)
        {
            resInfo = CreateResourceInfo(resourcePath);
            return (T)LoadResource(resourcePath, typeof(T));
        }

        public T Load<T>(Guid guid)
        {
            return Load<T>(GetResourcePathFromGuid(guid.ToString()));
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Load resource with relative or full path, the resource file should within the .resObj folder
        /// </summary>
        /// <param root="resourcePath">May be fullpath or relative path to resource root</param>
        /// <param root="ty"></param>
        /// <returns></returns>
        private object LoadResource(string resourcePath, Type ty)
        {
            resourcePath = GetCheckedResourcePath(resourcePath);
            var fullResourcePath = Path.Combine(ResourceRoot, resourcePath);

            object? resObj;
            try
            {
                // Try loading resource from built content first
                var dir = Path.GetDirectoryName(resourcePath);
                var file = Path.GetFileNameWithoutExtension(resourcePath);
                if (dir == null || file == null) throw new ArgumentException($"Invalid resourcePath value {resourcePath}");
                resObj = LoadContentNonGeneric(Path.Combine(dir, file), ty);
                goto LOAD_FINISHED;
            }
            catch
            {

            }

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

        LOAD_FINISHED:
            InitResourceHost(resObj, resourcePath);
            return resObj;
        }

        private string GetCheckedResourcePath(string resourcePath)
        {
            var fullResourcePath = Path.Combine(ResourceRoot, resourcePath);
            if (!PathHelper.IsSubPath(ResourceRoot, fullResourcePath)) throw new ArgumentException($"Resource Path \"{fullResourcePath}\" must be a subpath of resource root \"{ResourceRoot}\"");
            return Path.GetRelativePath(ResourceRoot, fullResourcePath);
        }

        //public void ImportResource()
        //{
        //    //var dotImporterFile = Path.Combine(ResourceRoot, ".resourceImporter");
        //    //if (!File.Exists(dotImporterFile)) ResourceImporter.CreateDotImporter(dotImporterFile);
        //    //var importerObject = ResourceImporter.ReadImporterObject(dotImporterFile);

        //    //ResourceImporter.ResourceRoot = ResourceRoot;
        //    //ResourceImporter.SetImporterObject(importerObject);
        //    //ResourceImporter.DoBuild();
        //    //ResourceImporter.ExportResult();

        //    //LogService.SetMsg(importerObject.BuildLog, "ResourceImporter", true);
        //    //_getResourcePathFromGuid = importerObject.BuildResult;
        //}

        private T LoadContent<T>(string resourcePath)
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

        private object LoadContentNonGeneric(string resourcePath, Type ty)
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.
                return typeof(ResourceManager).
                    GetMethod(nameof(LoadContent), BindingFlags.NonPublic | BindingFlags.Instance).
                    MakeGenericMethod(ty).
                    Invoke(this, BindingFlags.DoNotWrapExceptions, null, [resourcePath], null);
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            catch
            {
                throw;
            }
        }

        private string GetResourcePathFromGuid(string guid)
        {
            try
            {
                return _importer.GetResourcePathFromGuid(guid);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Can't find resource with guid " + guid);
            }
        }

        private void InitResourceHost(object resObj, string resourcePath)
        {
            GetCheckedResourcePath(resourcePath);

            if (resObj is IHostResource host)
                host.ResourceInfo = CreateResourceInfo(resourcePath);
        }

        private string GetFullResPath()
        {
            return Path.GetFullPath(ResourceRoot);
        }

        private ResourceInfo CreateResourceInfo(string resourcePath)
        {
            return ResourceInfo.Create(this, resourcePath);
        }
        #endregion

        #region Privates
        Serializer _serializer;
        string _resourceRoot = "res";
        string ContentRoot => $"{_resourceRoot}\\.content\\bin";
        ResourceImporter _importer;
        #endregion

        public static ResourceManager From(string resourceDir)
        {
            resourceDir = Path.GetFullPath(resourceDir);

            if (!_managers.ContainsKey(resourceDir))
                _managers[resourceDir] = new ResourceManager(resourceDir);

            return _managers[resourceDir];
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

        static Dictionary<string, ResourceManager> _managers = [];
    }
}