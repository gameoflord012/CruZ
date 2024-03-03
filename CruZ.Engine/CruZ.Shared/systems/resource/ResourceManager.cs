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
using System.IO;
using System.Linq;
using System.Reflection;

using static CruZ.Tools.ResourceImporter.ResourceImporter;

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
            _importer.InitializeImporter();
            //_importer.EnableWatch();
        }

        #region Public Functions
        public void Create(string nonContextResourcePath, object resObj, bool replaceIfExists = false)
        {
            var resourcePath = CheckedResourcePath(nonContextResourcePath);          

            object? existsResource = null;

            if (!replaceIfExists)
            {
                try
                {
                    existsResource = Load(nonContextResourcePath, resObj.GetType());
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

            InitResourceHost(resObj, nonContextResourcePath);
        }

        public void Save(IHostResource host)
        {
            if (host.ResourceInfo == null)
                throw new ArgumentException($"Can't save {host} resource, use create resource first");

            Create(host.ResourceInfo.ResourceName, host, true);
        }

        //public ResourceInfo PreLoad(string nonContextResourcePath)
        //{
        //    PreLoad([nonContextResourcePath]);
        //    return RetriveResourceInfo(nonContextResourcePath);
        //}

        //public void PreLoad(params string[] nonContextResourcePath)
        //{
        //    var resourcePaths = nonContextResourcePath.Select(e => CheckedResourcePath(e).ToString()).ToArray();
        //    _importer.ImportResources(resourcePaths);
        //}

        public T Load<T>(string nonContextResourcePath)
        {
            return (T)Load(nonContextResourcePath, typeof(T));
        }

        public T Load<T>(ResourceInfo resourceInfo)
        {
            return (T)Load(resourceInfo.Guid.ToString(), typeof(T));
        }
        
        public ResourceInfo RetriveResourceInfo(string nonContextResourcePath)
        {
            ResourcePath resourcePath = CheckedResourcePath(nonContextResourcePath);

            try
            {
                return ResourceInfo.Create(_importer.GetResourceGuid((string)resourcePath), resourcePath);
            }
            catch (ResourcePathNotFoundException)
            {
                throw new ArgumentException($"Resource \"{resourcePath}\" maybe unimported");
            }
            catch (ResourceGuidNotFoundException)
            {
                throw new ArgumentException($"Resource \"{resourcePath}\" maybe unimported");
            }
        }
        #endregion

        #region Interface Implementations
        public ResourcePath CheckedResourcePath(string nonContextResourcePath)
        {
            // If it is Guid representation
            if(
                Guid.TryParse(nonContextResourcePath, out Guid guid) && 
                _importer.ContainGuid(guid))
            {
                return ResourcePath.Create(guid, _importer.GetResourcePathFromGuid(guid));
            }

            // If it is filepath representation
            var fullResourcePath = Path.Combine(ResourceRoot, nonContextResourcePath);
            if (!PathHelper.IsSubPath(ResourceRoot, fullResourcePath)) throw new ArgumentException($"Resource Path \"{fullResourcePath}\" must be a subpath of resource root \"{ResourceRoot}\"");
            var formated = Path.GetRelativePath(ResourceRoot, fullResourcePath);
            return ResourcePath.Create(_importer.GetResourceGuid(formated), formated);
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
        /// <param root="nonContextResourcePath">May be fullpath or relative path to resource root</param>
        /// <param root="ty"></param>
        /// <returns></returns>
        private object Load(string nonContextResourcePath, Type ty)
        {
            ResourcePath resourcePath = CheckedResourcePath(nonContextResourcePath);
            object? resObj;

            if (ContentSupportedTypes.Contains(ty))
            {
                resObj = LoadContentNonGeneric(resourcePath.ToString(), ty);
            }
            else
            {
                resObj = LoadResource(nonContextResourcePath, ty);
            }

            InitResourceHost(resObj, nonContextResourcePath);
            return resObj;
        }

        private object LoadResource(string nonContextResourcePath, Type ty)
        {
            ResourcePath resourcePath = CheckedResourcePath(nonContextResourcePath);
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

        private T LoadContent<T>(string nonContextResourcePath)
        {
            ResourcePath resourcePath = CheckedResourcePath(nonContextResourcePath);
            T resultObject;

            try
            {
                resultObject = GameApplication.GetContent().Load<T>(
                        ContentOutputDir + "\\" + resourcePath.Guid);
            }
            catch (FileNotFoundException)
            {
                throw new ContentLoadException();
            }
            catch (ContentLoadException)
            {
                throw;
            }

            return resultObject;
        }

        private object LoadContentNonGeneric(string nonContextResourcePath, Type ty)
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.
                return typeof(ResourceManager).
                    GetMethod(nameof(LoadContent), BindingFlags.NonPublic | BindingFlags.Instance).
                    MakeGenericMethod(ty).
                    Invoke(this, [nonContextResourcePath]);
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            catch (System.Exception e)
            {
                throw new ContentLoadException($"Cannot load content {nonContextResourcePath}", e);
            }
        }

        private string GetResourcePathFromGuid(Guid guid)
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

        private void InitResourceHost(object resObj, string nonContextResourcePath)
        {
            var info = RetriveResourceInfo(nonContextResourcePath);
            if (resObj is IHostResource host) host.ResourceInfo = info;
        }
        #endregion

        #region Private Variables
        Serializer _serializer;
        ResourceImporter _importer;
        string _resourceRoot = "res";
        string ContentOutputDir => $"{_resourceRoot}\\.content\\bin";

        private static readonly Type[] ContentSupportedTypes =
        {
            typeof(Texture2D),
            typeof(SpriteFont),
            typeof(Effect)
        };
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