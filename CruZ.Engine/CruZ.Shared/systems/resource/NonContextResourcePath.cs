namespace CruZ.Resource
{
    public struct NonContextResourcePath
    {
        private NonContextResourcePath(string resourcePath)
        {
            _resourcePath = resourcePath;
        }

        public static implicit operator NonContextResourcePath(string resourcePath)
        {
            return new NonContextResourcePath(resourcePath);
        }

        public ResourcePath CheckedBy(ICheckResourcePath checker)
        {
            return new ResourcePath(checker, _resourcePath);
        }

        string _resourcePath;
    }
}
