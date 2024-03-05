using System;

namespace CruZ.Resource
{
    public interface IHostResource
    {
        ResourceInfo? ResourceInfo { get; set; }
    }

    public class ResourceHost : IHostResource
    {
        public ResourceInfo? ResourceInfo { get; set; }
    }

    public class ResourceInfo
    {
        private ResourceInfo() { }

        private ResourceInfo(Guid guid, string resourceName)
        {
            Guid = guid;
            ResourceName = resourceName;
        }

        public static ResourceInfo Create(Guid guid, string resourceName)
        {
            return new ResourceInfo(guid, resourceName);
        }

        public Guid Guid { get; private set; }
        public string ResourceName { get; private set; }
        //public ResourceManager ResourceManager { get; private set; }
    }
}