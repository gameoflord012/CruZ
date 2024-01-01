using CruZ.Components;
using CruZ.Utility;
using CurZ.Serialization;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace CruZ
{
    public static class ResourceManager
    {
        public static readonly string TEMPLATE_ROOT = "Template";
        public static readonly string CONTENT_ROOT = "Content\\bin";

        public static T LoadContent<T>(string resourceName)
        {
            return Core.Instance.Content.Load<T>(resourceName);
        }

        public static EntityTemplate? LoadTemplate(string filePath)
        {
            filePath = GetTemplateFullPath(filePath);
            if (!File.Exists(filePath)) return null;

            return
                GlobalSerializer.DeserializeFromFile(filePath, typeof(EntityTemplate))
                as EntityTemplate;
        }

        public static void SaveTemplate(EntityTemplate et, string filePath)
        {
            filePath = GetTemplateFullPath(filePath);
            GlobalSerializer.SerializeToFile(et, filePath);
        }

        private static string GetTemplateFullPath(string templatePath)
        {
            return TEMPLATE_ROOT + "\\" + templatePath;
        }
    }
}