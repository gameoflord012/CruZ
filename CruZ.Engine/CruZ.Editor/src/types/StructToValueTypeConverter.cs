using System;

namespace CruZ.Editor;

/// <summary>
/// TypeConverter convert struct to value type 
/// so it can be modified in PropertyGrid/>
/// </summary>
public class StructToValueTypeConverter : System.ComponentModel.ExpandableObjectConverter
{
    public override bool GetCreateInstanceSupported(System.ComponentModel.ITypeDescriptorContext context)
    {
        return true;
    }

    public override object CreateInstance(System.ComponentModel.ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
    {
        if (propertyValues == null)
            throw new ArgumentNullException("propertyValues");

        object instance = Activator.CreateInstance(context.PropertyDescriptor.PropertyType);
        foreach (System.Collections.DictionaryEntry entry in propertyValues)
        {
            System.Reflection.PropertyInfo propInfo = context.PropertyDescriptor.PropertyType.GetProperty(entry.Key.ToString());
            if ((propInfo != null) && (propInfo.CanWrite))
            {
                propInfo.SetValue(instance, Convert.ChangeType(entry.Value, propInfo.PropertyType), null);
            }
        }
        return instance;
    }
}