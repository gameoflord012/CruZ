using System.ComponentModel;

namespace CruZ.Editor.Winform.Ultility
{
    using CruZ.Framework.GameSystem;

    internal class EntityWrapper
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
            Components = new ComponentsWrapper(TransformEntity.GetAllComponents());
        }

        public ComponentsWrapper Components { get; private set; } = null!;
    }

    [TypeConverter(typeof(ComponentsWrapperTypeConverter))]
    public class ComponentsWrapper
    {
        public ComponentsWrapper(Component[] comps)
        {
            Components = comps;
        }

        public Component[] Components { get; set; }

        public override string ToString()
        {
            return $"{Components.Length} Components";
        }
    }
}