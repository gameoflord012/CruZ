using System;

namespace CruZ.Resource
{
    public interface IResourceInjection
    {
        public void GetInjectTarget(out Type type, out object resourceRef);
    }
}