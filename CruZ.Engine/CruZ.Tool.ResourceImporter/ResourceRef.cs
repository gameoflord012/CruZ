using System;
using System.CodeDom.Compiler;

namespace CruZ.Tools.ResourceImporter
{
    struct ResourceRef : IEquatable<ResourceRef>
    {
        public ResourceRef(Guid guid, string label)
        {
            Guid = guid;
            Label = label;
        }

        public bool Equals(ResourceRef other)
        {
            return Guid == other.Guid;
        }

        public override string ToString()
        {
            return $"{Guid}:{Label}";
        }

        public static ResourceRef Prase(string str)
        {
            var split = str.Split(':');
            var guid = Guid.Parse(split[0]);
            var label = split[1];
            return new ResourceRef(guid, label);
        }

        public string Label { get; private set; }
        public Guid Guid { get; private set; }
    }
}
