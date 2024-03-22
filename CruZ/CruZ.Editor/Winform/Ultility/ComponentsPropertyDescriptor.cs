using System;
using System.ComponentModel;

namespace CruZ.Editor.Winform.Ultility
{
    using CruZ.Framework.GameSystem.ECS;

    public class ComponentsPropertyDescriptor : PropertyDescriptor
    {
        public ComponentsPropertyDescriptor(
            Component[] components,
            int index, string name, Attribute[]? attrs) : base(name, attrs)
        {
            _index = index;
            _components = components;

        }

        public override Type ComponentType => typeof(ComponentsWrapper);
        public override Type PropertyType => typeof(Component);

        public override bool CanResetValue(object component) => false;
        public override bool ShouldSerializeValue(object component) => true;
        public override bool IsReadOnly => false;


        public override object? GetValue(object? component)
        {
            return _components[_index];
        }


        public override void ResetValue(object component) { }
        public override void SetValue(object? component, object? value) { }


        int _index;
        Component[] _components;
    }
}