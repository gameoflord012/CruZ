using CruZ.ECS;

using System.ComponentModel;

namespace CruZ.Editor.Winform.Ultility
{
    public class EntityWrapper
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TransformEntity TransformEntity { get; }

        public EntityWrapper(TransformEntity e)
        {
            TransformEntity = e;
            RefreshComponents();
        }

        public void RefreshComponents()
        {
            Components = new(TransformEntity.Components);
        }

        public ComponentsWrapper Components { get; set; }
    }

    [TypeConverter(typeof(ComponentsTypeConverter))]
    public class ComponentsWrapper
    {
        public ComponentsWrapper(ECS.Component[] comps)
        {
            Components = comps;
        }

        public ECS.Component[] Components { get; set; }
    }
}