using CruZ.Resource;
using CruZ.Utility;
using CurZ.Serialization;
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
            ValidateURI(uri);
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
        public static readonly string TEMPLATE_ROOT = "Template";
        public static readonly string CONTENT_ROOT = "Content\\bin";

        public static T LoadContent<T>(URI uri)
        {
            return Core.Instance.Content.Load<T>(uri);
        }

        public static T? LoadResource<T>(URI uri) where T : class
        {   
            return LoadResource(uri, typeof(T)) as T;
        }

        public static object? LoadResource(URI uri, Type ty)
        {
            if (string.IsNullOrWhiteSpace(File.ReadAllText(uri))) return null;
            return GlobalSerializer.DeserializeFromFile(uri, ty);
        }

        public static void CreateResource(URI uri, object res, bool renew = false)
        {
            if (LoadResource(uri, res.GetType()) != null && !renew) return;
            GlobalSerializer.SerializeToFile(res, uri);
        }

        //public static EntityTemplate? LoadTemplate(string filePath)
        //{
        //    filePath = GetTemplateFullPath(filePath);
        //    if (!File.Exists(filePath)) return null;

        //    return
        //        GlobalSerializer.DeserializeFromFile(filePath, typeof(EntityTemplate))
        //        as EntityTemplate;
        //}

        //public static void SaveTemplate(EntityTemplate et, string uri)
        //{
        //    uri = GetTemplateFullPath(uri);
        //    GlobalSerializer.SerializeToFile(et, uri);
        //}

        //private static string GetTemplateFullPath(string uri)
        //{
        //    return TEMPLATE_ROOT + "\\" + uri;
        //}
    }
}