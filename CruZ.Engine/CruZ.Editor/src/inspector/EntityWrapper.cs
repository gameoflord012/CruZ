using CruZ.Components;
using System.ComponentModel;

namespace CruZ.Editor
{
    public class EntityWrapper
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TransformEntity TransformEntity { get; }

        public EntityWrapper(TransformEntity e)
        {
            Components = new(e.Components);
            TransformEntity = e;
        }

        public ComponentsWrapper Components { get; set; }
    }

    [TypeConverter(typeof(ComponentsTypeConverter))]
    public class ComponentsWrapper
    {
        public ComponentsWrapper(Components.IComponent[] comps)
        {
            Components = comps;
        }

        public Components.IComponent[] Components { get; set; }
    }
}