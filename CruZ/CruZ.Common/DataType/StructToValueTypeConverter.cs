//using System;

//namespace CruZ.Common.DataType;

///// <summary>
///// TypeConverter convert struct to value type 
///// so it can be modified in PropertyGrid/>
///// </summary>
//public class StructToValueTypeConverter : global::System.ComponentModel.ExpandableObjectConverter
//{
//    public override bool GetCreateInstanceSupported(global::System.ComponentModel.ITypeDescriptorContext context)
//    {
//        return true;
//    }

//    public override object CreateInstance(global::System.ComponentModel.ITypeDescriptorContext context, global::System.Collections.IDictionary propertyValues)
//    {
//        if (propertyValues == null)
//            throw new ArgumentNullException("propertyValues");

//        object instance = Activator.CreateInstance(context.PropertyDescriptor.PropertyType);
//        foreach (global::System.Collections.DictionaryEntry entry in propertyValues)
//        {
//            global::System.Reflection.PropertyInfo propInfo = context.PropertyDescriptor.PropertyType.GetProperty(entry.Key.ToString());
//            if (propInfo != null && propInfo.CanWrite)
//            {
//                propInfo.SetValue(instance, Convert.ChangeType(entry.Value, propInfo.PropertyType), null);
//            }
//        }
//        return instance;
//    }
//}