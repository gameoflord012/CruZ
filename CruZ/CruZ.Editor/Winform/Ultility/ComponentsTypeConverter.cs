using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace CruZ.Editor.Winform.Ultility
{
    public class ComponentsTypeConverter : ExpandableObjectConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext? context, object value, Attribute[]? attributes)
        {
            var comps = (ComponentsWrapper)value;

            PropertyDescriptorCollection props = new([]);
            for (int i = 0; i < comps.Components.Count(); i++)
            {
                props.Add(
                    new ComponentsPropertyDescriptor(
                        comps.Components, i,
                        comps.Components[i].GetType().Name,
                        [new TypeConverterAttribute(typeof(ExpandableObjectConverter))]
                    )
                );
            }

            return props;
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var wrapper = (ComponentsWrapper)value;
                return $"{wrapper.Components.Count()} Components";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}