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

        private ResourceInfo(string resName, bool isRuntime)
        {
            ResourceName = resName;
            IsRuntime = isRuntime;
        }

        public static ResourceInfo Create(string resName, bool isRuntime)
        {
            return new ResourceInfo(resName, isRuntime);
        }

        [JsonProperty]
        public string ResourceName  { get; private set; }
        [JsonProperty]
        public bool   IsRuntime     { get; private set; }
    }
}