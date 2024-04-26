//using System;

//namespace CruZ.GameEngine.Resource
//{
//    public class ResourceInfo
//    {
//        private ResourceInfo() { } // default constructor for serialization

//        private ResourceInfo(Guid guid, string resourceName, string referencePath) =>
//            (Guid, ResourceName, ReferencePath) = (guid, resourceName, referencePath);

//        internal static ResourceInfo Create(Guid guid, string resourceName, string referencePath)
//        {
//            return new ResourceInfo(guid, resourceName, referencePath);
//        }

//        public Guid Guid { get; private set; }
//        public string ResourceName { get; private set; }

//        /// <summary>
//        /// Resource reference Id seperates by '\' for example ".resourceref\editor\.resourceref\editor2\", empty if it is in the base
//        /// </summary>
//        public string ReferencePath { get; private set; }
//    }
//}