using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruZ.GameEngine.GameSystem
{
    internal class EntityCollection
    {
        public EntityCollection(IImmutableList<TransformEntity> transformEntities)
        {
            _entities = transformEntities;
        }

        public List<T> GetAllComponents<T>(GetComponentMode mode = GetComponentMode.ExactType) where T : Component
        {
            List<T> result = [];

            if (mode == GetComponentMode.ExactType)
            {
                foreach (var e in _entities)
                {
                    e.TryGetComponent(out T? com);
                    if (com != null) result.Add(com);
                }
            }
            else
            {
                foreach (var e in _entities)
                {
                    foreach (var com in e.GetAllComponents())
                    {
                        if (com is T) result.Add((T)com);
                    }
                }
            }


            return result;
        }

        IImmutableList<TransformEntity> _entities;
    }

    enum GetComponentMode
    {
        ExactType,
        Inherit
    }
}
