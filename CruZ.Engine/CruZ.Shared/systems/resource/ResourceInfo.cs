using CruZ.Serialization;

using Newtonsoft.Json;
using System;
using System.IO;

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

        private ResourceInfo(ResourceManager? manager, string resName)
        {
            ResourceName = resName;
            ResourceManager = manager;
        }

        public static ResourceInfo Create(ResourceManager? manager, string resourceName)
        {
            return new ResourceInfo(manager, resourceName);
        }

        public string ResourceName  { get; private set; }
        public ResourceManager ResourceManager { get; private set; }
    }
}