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
using System.Linq.Expressions;
using System.Reflection;

namespace CruZ.Resource
{
    public static class ResourceManager
    {
        public const string RESOURCE_ROOT = "res";
        public const string CONTENT_ROOT = "res\\Content\\bin";

        static ResourceManager()
        {
            _Serializer = new Serializer();
            _Serializer.Converters.Add(new TextureAtlasJsonConverter());
            _Serializer.Converters.Add(new SerializableJsonConverter());

            _ResourceImporterObject = ResourceImporter.ReadImporterObject(RESOURCE_ROOT + "\\.resourceImporter");
            _GetResourcePathFromGuid = _ResourceImporterObject.BuildResult;
        }

        public static object LoadResource(string resourcePath, Type ty)
        {
            try
            {
                var dir = Path.GetDirectoryName(resourcePath);
                var file = Path.GetFileNameWithoutExtension(resourcePath);
                return LoadContent(Path.Combine(dir, file), ty);
            }
            catch(ContentLoadException)
            {
                try
                {
                    return _Serializer.DeserializeFromFile(Path.Combine(RESOURCE_ROOT, resourcePath), ty);
                }
                catch(FileNotFoundException)
                {
                    throw new(string.Format("Can't find resource file {0}", resourcePath));
                }
            }
        }

        public static void InitResource(string resourcePath, object res, bool renew = false)
        {
            object? existedResource = null;

            if (!renew)
            {
                try
                {
                    existedResource = LoadResource(resourcePath, res.GetType());
                }
                catch
                {
                    Logging.PushMsg("Failed to load resource \"{0}\", new one will be created", resourcePath);
                }
            }

            if (existedResource == null)
            {
                _Serializer.SerializeToFile(res, Path.Combine(RESOURCE_ROOT, resourcePath));
            }
        }

        public static T LoadResource<T>(Guid guid)
        {
            return LoadResource<T>(GetResourcePath(guid));
        }

        private static string GetResourcePath(Guid guid)
        {
            try { 
                return _GetResourcePathFromGuid[guid.ToString()];
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Can't find resource with guid " + guid);
            }
        }

        public static T LoadContent<T>(string resourcePath)
        {
            try
            {
                if (typeof(T) == typeof(SpriteSheet))
                {
                    return ApplicationContext.Content.Load<T>(
                        CONTENT_ROOT + "\\" + resourcePath, new JsonContentLoader());
                }

                return ApplicationContext.Content.Load<T>(
                    CONTENT_ROOT + "\\" + resourcePath);
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

        private static object LoadContent(string resourcePath, Type ty)
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.
                return typeof(ResourceManager).
                    GetMethod("LoadContent", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).
                    MakeGenericMethod(ty).
                    Invoke(null, BindingFlags.DoNotWrapExceptions, null,[resourcePath], null);
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            catch
            {
                throw;
            }
        }

        public static string GetFullPath()
        {
            return Path.GetFullPath(RESOURCE_ROOT);
        }

        public static T LoadResource<T>(string resourcePath)
        {
            return (T)LoadResource(resourcePath, typeof(T));
        }

        private static Serializer _Serializer;
        private static ResourceImporterObject _ResourceImporterObject;
        private static Dictionary<string, string> _GetResourcePathFromGuid = [];
    }
}