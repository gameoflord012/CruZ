using System;
using System.Collections.Generic;
using System.Linq;

namespace CruZ.Components
{
    public interface IBuildInstruction
    {
        public void SetTarget(EntityTemplate target);
        public void AddChildTemplate(EntityTemplate childTemplate);
        public void RequireComponent(Type ty);
    }

    class BuildInstruction : IBuildInstruction
    {
        public List<KeyValuePair<EntityTemplate, EntityTemplate>> Tree { get => _tree; set => _tree = value; }
        internal Dictionary<EntityTemplate, List<Type>> Comps { get => _comps; set => _comps = value; }

        public BuildInstruction(EntityTemplate instructionTarget)
        {
            SetTarget(instructionTarget);
        }

        public void AddChildTemplate(EntityTemplate child)
        {
            var parent = GetTarget();

            Tree.Add(new(parent, child));

            SetTarget(child);
            child.GetInstruction(this);
            SetTarget(parent);
        }

        public void RequireComponent(Type ty)
        {
            _comps[GetTarget()].Add(ty);
        }

        public void SetTarget(EntityTemplate target)
        {
            _target = target;

            if (!_comps.ContainsKey(target))
            {
                _comps[target] = [];
            }
        }

        private EntityTemplate GetTarget()
        {
            return _target;
        }

        EntityTemplate _target;
        List<KeyValuePair<EntityTemplate, EntityTemplate>> _tree = [];
        Dictionary<EntityTemplate, List<Type>> _comps = [];
    }
}