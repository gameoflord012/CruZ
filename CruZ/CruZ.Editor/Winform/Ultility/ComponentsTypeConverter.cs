using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace CruZ.Editor.Winform.Ultility
{
    // TODO: subtitude this to use TransformEntityTypeConverter directly instead of using EntityWrapper
    public class ComponentsWrapperTypeConverter : ExpandableObjectConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext? context, object value, Attribute[]? attributes)
        {
            var comps = (ComponentsWrapper)value;

            PropertyDescriptorCollection props = new([]);
            for (int i = 0; i < comps.Components.Count(); i++)
            {
                props.Add(
                    new ComponentPropertyDescriptor(
                        comps.Components, i,
                        comps.Components[i].GetType().Name,
                        [new TypeConverterAttribute(typeof(ExpandableObjectConverter))]
                    )
                );
            }

            return props;
        }
    }
}