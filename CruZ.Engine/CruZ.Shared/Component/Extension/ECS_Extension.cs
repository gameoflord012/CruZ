using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CruZ_Engine.Components;
using MonoGame.Extended.Entities.Systems;

namespace MonoGame.Extended.Entities
{
    public static class ECS_Extension
    {
        public static TransformEntity CreateTransformEntity(this World world)
        {
            var e = world.CreateEntity();
            TransformEntity t_e = new(e);
            return t_e;
        }

        public static int[] GetActiveEntities(this EntitySystem es)
        {
            return es.ActiveEntities.
                Where(e => TransformEntity.GetTransformEntity(e).IsActive).
                ToArray();
        }
    }
}
