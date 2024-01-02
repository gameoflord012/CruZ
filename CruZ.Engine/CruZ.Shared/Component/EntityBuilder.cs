//using CruZ.Components;
//using MonoGame.Extended.Entities;
//using System.Collections.Generic;
//using System.Linq;

//namespace CruZ.Components
//{
//    public class EntityBuilder
//    {
//        public EntityBuilder(World world)
//        {
//            _world = world;
//        }

//        public Dictionary<EntityTemplate, TransformEntity> BuildFrom(EntityTemplate root)
//        {
//            var instruction = new BuildInstruction(root);
//            root.GetInstruction(instruction);

//            var getEntity = new Dictionary<EntityTemplate, TransformEntity>();

//            Build(root, null);

//            foreach (var node in instruction.Tree)
//            {
//                var parent = node.Key;
//                var child = node.Value;
//                Build(child, parent);
//            }

//            return getEntity;

//            void Build(EntityTemplate cTemplate, EntityTemplate? pTemplate)
//            {
//                var cEntity = _world.CreateTransformEntity();
//                getEntity[cTemplate] = cEntity;

//                cEntity.Parent = pTemplate != null ? getEntity[pTemplate] : null;

//                foreach (var compTy in instruction.Comps[cTemplate])
//                {
//                    cEntity.RequireComponent(compTy);
//                }

//                cTemplate.OnAttached(cEntity);
//            }
//        }

//        World _world;
//    }
//}
