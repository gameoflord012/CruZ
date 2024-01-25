using System;
using System.ComponentModel;
using System.Linq;

namespace CruZ.Editor
{
    public class ComponentsTypeConverter : ExpandableObjectConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext? context, object value, Attribute[]? attributes)
        {
            var comps = (ComponentsWrapper)value;

            PropertyDescriptorCollection props = new([]);
            for(int i = 0; i < comps.Components.Count(); i++)
            {
                props.Add(
                    new ComponentsPropertyDescriptor(
                        comps.Components, i, 
                        comps.Components[i].ComponentType.ToString(), 
                        [new TypeConverterAttribute(typeof(ExpandableObjectConverter))]
                    )
                );
            }

            return props;
        }
    }
}