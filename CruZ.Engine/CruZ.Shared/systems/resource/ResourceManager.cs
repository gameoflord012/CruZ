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
    public class ResourceManager : ICustomSerializable, ICheckResourcePath
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
        public void Create(NonContextResourcePath nonContextResourcePath, object resObj, bool replaceIfExists = false)
        {
            ResourcePath resourcePath = nonContextResourcePath.CheckedBy(this);

            object? existsResource = null;

            if (!replaceIfExists)
            {
                try
                {
                    existsResource = Load(nonContextResourcePath, resObj.GetType(), out _);
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

        public void PreLoad<T>(params NonContextResourcePath[] resourcePath)
        {
            ResourceImporter.NonContextResourcePath[] buildingContent =
                Array.ConvertAll(
                    resourcePath,
                    e => new ResourceImporter.NonContextResourcePath(e.CheckedBy(this)));
            _importer.ImportResources(buildingContent);
        }

        public T Load<T>(NonContextResourcePath resourcePath)
        {
            return (T)Load(resourcePath, typeof(T), out _);
        }

        public T Load<T>(NonContextResourcePath resourcePath, out ResourceInfo infoOut)
        {
            return (T)Load(resourcePath, typeof(T), out infoOut);
        }

        public T Load<T>(Guid guid)
        {
            return Load<T>(GetResourcePathFromGuid(guid));
        }
        #endregion

        #region Interface Implementations
        public string CheckedResourcePath(string nonContextResourcePath)
        {
            var fullResourcePath = Path.Combine(ResourceRoot, nonContextResourcePath);
            if (!PathHelper.IsSubPath(ResourceRoot, fullResourcePath)) throw new ArgumentException($"Resource Path \"{fullResourcePath}\" must be a subpath of resource root \"{ResourceRoot}\"");
            return Path.GetRelativePath(ResourceRoot, fullResourcePath);
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
        private object Load(NonContextResourcePath nonContextResourcePath, Type ty, out ResourceInfo infoOut)
        {
            ResourcePath resourcePath = nonContextResourcePath.CheckedBy(this);
            object? resObj;

            if (ContentSupportedTypes.Contains(ty))
            {
                resObj = LoadContentNonGeneric(resourcePath.ToString(), ty);
            }
            else
            {
                resObj = LoadResource(nonContextResourcePath, ty);
            }


        LOAD_FINISHED:
            infoOut = InitResourceHost(resObj, nonContextResourcePath);
            return resObj;
        }

        private object LoadResource(NonContextResourcePath nonContextResourcePath, Type ty)
        {
            ResourcePath resourcePath = nonContextResourcePath.CheckedBy(this);
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

        private T LoadContent<T>(NonContextResourcePath nonContextResourcePath)
        {
            ResourcePath resourcePath = nonContextResourcePath.CheckedBy(this);
            T resultObject;

            try
            {
                if (typeof(T) == typeof(SpriteSheet))
                {
                    resultObject = GameApplication.GetContent().Load<T>(
                        ContentOutputDir + "\\" + resourcePath, new JsonContentLoader());
                }
                else
                {
                    resultObject = GameApplication.GetContent().Load<T>(
                        ContentOutputDir + "\\" + resourcePath);
                }
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

        private object LoadContentNonGeneric(NonContextResourcePath nonContextResourcePath, Type ty)
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
                throw new ContentLoadException($"Cannot load content {nonContextResourcePath.CheckedBy(this)}", e);
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

        private ResourceInfo InitResourceHost(object resObj, NonContextResourcePath nonContextResourcePath)
        {
            var info = CreateResourceInfo(nonContextResourcePath);
            if (resObj is IHostResource host)
                host.ResourceInfo = info;
            return info;
        }

        private ResourceInfo CreateResourceInfo(NonContextResourcePath nonContextResourcePath)
        {
            ResourcePath resourcePath = nonContextResourcePath.CheckedBy(this);

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

        #region Private Variables
        Serializer _serializer;
        ResourceImporter _importer;
        string _resourceRoot = "res";
        string ContentOutputDir => $"{_resourceRoot}\\.content\\bin";

        private static readonly Type[] ContentSupportedTypes =
        {
            typeof(Texture2D),
            typeof(SpriteSheet),
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