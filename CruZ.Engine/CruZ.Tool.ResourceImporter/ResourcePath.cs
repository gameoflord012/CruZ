using System.IO;

namespace CruZ.Tools.ResourceImporter
{
    /// <summary>
    /// Unify formated resource path
    /// </summary>
    internal struct ResourcePath
    {
        private ResourcePath(string value)
        {
            Value = value;
        }

        static internal ResourcePath Create(string resourcePath)
        {
            return new ResourcePath(resourcePath);
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(ResourcePath resourcePath)
        {
            return resourcePath.Value;
        }

        public string Value { get; private set; }
    }
}
