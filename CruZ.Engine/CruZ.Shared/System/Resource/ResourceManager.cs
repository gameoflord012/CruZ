﻿using Assimp.Configs;
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

        /// <summary>
        /// Load resource with relative or full path, the resource file should within the .res folder
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <param name="ty"></param>
        /// <returns></returns>
        public static object LoadResource(string resourcePath, Type ty)
        {
            object resObj;

            try
            {
                var dir = Path.GetDirectoryName(resourcePath);
                var file = Path.GetFileNameWithoutExtension(resourcePath);

                resObj = LoadContentNonGeneric(Path.Combine(dir, file), ty);
            }
            catch(ContentLoadException)
            {
                try
                {
                    resObj = _Serializer.DeserializeFromFile(Path.Combine(RESOURCE_ROOT, resourcePath), ty);
                }
                catch(FileNotFoundException)
                {
                    throw new(string.Format("Can't find resource file {0}", resourcePath));
                }
            }

            var hasResourcePath = resObj as IHasResourcePath;
            if(hasResourcePath != null) hasResourcePath.ResourcePath = resourcePath;

            return resObj;
        }

        public static void CreateResource(string resourcePath, object res, bool renew = false)
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

        public static void CreateResource(IHasResourcePath res, bool renew = false)
        {
            CreateResource(res.ResourcePath, res, renew);
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

        private static T LoadContent<T>(string resourcePath)
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

        private static object LoadContentNonGeneric(string resourcePath, Type ty)
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.
                return typeof(ResourceManager).
                    GetMethod("LoadContent", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).
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