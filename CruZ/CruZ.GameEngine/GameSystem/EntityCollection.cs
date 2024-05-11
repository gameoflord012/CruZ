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
        public EntityCollection(IEnumerable<TransformEntity> transformEntities)
        {
            _entities = transformEntities;
        }

        public List<T> GetAllComponents<T>(GetComponentMode mode = GetComponentMode.ExactType) where T : Component
        {
            List<T> result = [];

            if (mode == GetComponentMode.ExactType)
            {
                foreach (var entity in _entities)
                {
                    if(entity.TryGetComponent<T>(out var com))
                    {
                        result.Add(com);
                    }
                }
            }
            else
            {
                foreach (var entity in _entities)
                {
                    foreach (var com in entity.GetAllComponents())
                    {
                        if (com is T t) result.Add(t);
                    }
                }
            }


            return result;
        }

        IEnumerable<TransformEntity> _entities;
    }

    enum GetComponentMode
    {
        ExactType,
        Inherit
    }
}
