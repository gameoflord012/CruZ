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

        public static T LoadContent<T>(URI uri)
        {
            try
            {
                if (typeof(T) == typeof(SpriteSheet))
                {
                    return ApplicationContext.Content.Load<T>(
                        CONTENT_ROOT + "\\" + uri, new JsonContentLoader());
                }

                return ApplicationContext.Content.Load<T>(
                    CONTENT_ROOT + "\\" + uri);
            }
            catch(FileNotFoundException)
            {
                throw new ContentLoadException();
            }
            catch(ContentLoadException)
            {
                throw;
            }
            
        }

        public static object LoadResource(URI uri, Type ty)
        {
            try
            {
                return LoadContent(uri, ty);
            }
            catch(ContentLoadException)
            {
                try
                {
                    return _Serializer.DeserializeFromFile(uri.GetFullPath(RESOURCE_ROOT), ty);
                }
                catch(FileNotFoundException)
                {
                    throw new(string.Format("Can't find resource file with uri {0}", uri));
                }
            }
        }

        public static void InitResource(URI uri, object res, bool renew = false)
        {
            object? existedResource = null;

            if (!renew)
            {
                try
                {
                    existedResource = LoadResource(uri, res.GetType());
                }
                catch
                {
                    Logging.PushMsg("Failed to load resource \"{0}\", new one will be created", uri);
                }
            }

            if (existedResource == null)
            {
                _Serializer.SerializeToFile(res, uri.GetFullPath(RESOURCE_ROOT));
            }
        }

        private static object LoadContent(URI uri, Type ty)
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.
                return typeof(ResourceManager).
                    GetMethod("LoadContent", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).
                    MakeGenericMethod(ty).
                    Invoke(null, [uri]);
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException == null) throw;
                throw e.InnerException;
            }
        }

        public static string GetFullPath()
        {
            return Path.GetFullPath(RESOURCE_ROOT);
        }

        public static T LoadResource<T>(URI uri)
        {
            return (T)LoadResource(uri, typeof(T));
        }

        private static Serializer _Serializer;
        private static ResourceImporterObject _ResourceImporterObject;
        private static Dictionary<string, string> _GetResourcePathFromGuid = [];
    }
}