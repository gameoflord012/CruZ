using System.IO;

namespace CruZ.Resource
{
    public struct ResourcePath
    {
        public ResourcePath(ICheckResourcePath checker, string NonContextResourcePath)
        {
            _resourcePath = checker.CheckedResourcePath(NonContextResourcePath);
        }

        public static implicit operator string(ResourcePath resourcePath)
        {
            return resourcePath._resourcePath;
        }

        public override string ToString()
        {
            return _resourcePath;
        }

        string _resourcePath;
    }
}
