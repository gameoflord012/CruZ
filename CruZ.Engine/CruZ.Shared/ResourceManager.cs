using CruZ.Resource;
using CruZ.Utility;
using CurZ.Serialization;
using Newtonsoft.Json;
using System;
using System.IO;
namespace CruZ.Resource
{
    public class URI
    {
        public static implicit operator string(URI uri)
        {
            return uri._uri;
        }

        public static implicit operator URI(string uri)
        {
            return new URI(uri);
        }

        public URI(string uri)
        {
            _uri = uri;

            ValidateURI(this);
        }

        private string _uri = "";

        public static void ValidateURI(string uri)
        {
            bool isValid = !string.IsNullOrEmpty(uri);

            if (!isValid)
            {
                throw new(string.Format("Resource URI {0} is invalid", uri));
            }
        }
    }

    public static class ResourceManager
    {
        public const string RESOURCE_ROOT = "Resource\\bin";
        public const string CONTENT_ROOT = "Resource\\bin";

        public static T LoadContent<T>(URI uri)
        {
            return Core.Instance.Content.Load<T>(uri);
        }

        public static T LoadResource<T>(URI uri) where T : class
        {
            return (T)LoadResource(uri, typeof(T));
        }

        public static object LoadResource(URI uri, Type ty)
        {
            uri = RESOURCE_ROOT + "\\" + uri;
            return GlobalSerializer.DeserializeFromFile(uri, ty);
        }

        public static void CreateResource(URI uri, object res, bool renew = false)
        {
            try
            {
                if (LoadResource(uri, res.GetType()) != null && !renew) return;
            }
            catch
            {
                uri = RESOURCE_ROOT + "\\" + uri;
                GlobalSerializer.SerializeToFile(res, uri);
            }
        }
    }
}