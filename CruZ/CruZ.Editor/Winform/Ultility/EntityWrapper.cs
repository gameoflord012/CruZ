using System.ComponentModel;

namespace CruZ.Editor.Winform.Ultility
{
    using CruZ.Framework.GameSystem.ECS;

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
            Components = new(TransformEntity.GetAllComponents());
        }

        public ComponentsWrapper Components { get; set; }
    }

    [TypeConverter(typeof(ComponentsTypeConverter))]
    public class ComponentsWrapper
    {
        public ComponentsWrapper(Component[] comps)
        {
            Components = comps;
        }

        public Component[] Components { get; set; }
    }
}