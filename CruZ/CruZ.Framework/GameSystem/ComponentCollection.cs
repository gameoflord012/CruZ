using System;
using System.Collections.Generic;
using System.Linq;

using CruZ.Framework.GameSystem.ECS;

namespace CruZ.Framework.GameSystem
{
    public class ComponentCollection
    {
        public ComponentCollection(Dictionary<Type, Component> comp)
        {
            _comps = comp;
        }

        public void TryGetComponent<T>(out T? component) where T : Component
        {
            component = _comps.ContainsKey(typeof(T)) ? (T)_comps[typeof(T)] : null;
        }

        public Component[] GetAllComponents()
        {
            return _comps.Values.ToArray();
        }

        private readonly Dictionary<Type, Component> _comps;
    }
}
