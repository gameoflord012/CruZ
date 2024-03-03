using System;
using System.IO;

namespace CruZ.Resource
{
    public struct ResourcePath
    {
        private ResourcePath(Guid guid, string formatedResourcePath)
        {
            Formated = formatedResourcePath;
            Guid = guid;
        }

        public static ResourcePath Create(Guid guid, string formatedResourcePath)
        {
            return new ResourcePath(guid, formatedResourcePath);
        }

        public static implicit operator string(ResourcePath resourcePath)
        {
            return resourcePath.Formated;
        }

        public override string ToString()
        {
            return Formated;
        }

        public string Formated { get; private set; }
        public Guid Guid { get; private set; }
    }
}
