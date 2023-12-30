using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CruZ.Components
{
    static class EntityExtension
    {
        public static IEnumerable<T> GetAllComponents<T>(this EntitySystem system, ComponentMapper<T> mapper) where T : class
        {
            foreach(var e in system.GetActiveEntities())
            {
                yield return mapper.Get(e);
            }

            yield break;
        }

        public static void Attach(this Entity e, object component, Type ty)
        {
            var mapper = e.ComponentManager.GetMapper(ty);
            mapper.Put(e.Id, component);
        }
    }
}