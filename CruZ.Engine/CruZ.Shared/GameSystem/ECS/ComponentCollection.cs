using System;
using System.Collections.Generic;

namespace CruZ.ECS
{
    public class ComponentCollection
    {
        public ComponentCollection(Dictionary<Type, Component> comp)
        {
            _comps = comp;
        }

        public void TryGetComponent<T>(ref T? component) where T : Component
        {
            component = (T)(_comps.ContainsKey(typeof(T)) ? _comps[typeof(T)] : null);
        }

        private readonly Dictionary<Type, Component> _comps;
    }
}
