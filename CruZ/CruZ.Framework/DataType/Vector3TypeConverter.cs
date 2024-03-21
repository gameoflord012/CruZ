//using System;
//using System.ComponentModel;
//using System.Diagnostics.CodeAnalysis;
//using System.Globalization;

//namespace CruZ.Common.DataType
//{
//    /// <summary>
//    /// Convert Vector3 into string
//    /// </summary>
//    class Vector3TypeConverter : TypeConverter
//    {
//        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
//        {
//            return true;
//        }

//        public override object CreateInstance(ITypeDescriptorContext context, global::System.Collections.IDictionary propertyValues)
//        {
//            var valueTypeConverter = new StructToValueTypeConverter();
//            return valueTypeConverter.CreateInstance(context, propertyValues);
//        }

//        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
//        {
//            return sourceType == typeof(string);
//        }

//        public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
//        {
//            return destinationType == typeof(string);
//        }

//        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
//        {
//            var v3 = (Vector3)value;

//            if (destinationType == typeof(string))
//            {
//                return $"{v3.X}, {v3.Y}, {v3.Z}";
//            }

//            return base.ConvertTo(context, culture, value, destinationType);
//        }

//        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
//        {
//            if (value is string)
//            {
//                float x = 0, y = 0, z = 0;

//                var valStr = (string)value;
//                var split = valStr.Split(',');

//                try
//                {
//                    float.TryParse(split[0], out x);
//                    float.TryParse(split[1], out y);
//                    float.TryParse(split[2], out z);
//                }
//                catch (IndexOutOfRangeException)
//                {

//                }

//                return new Vector3(x, y, z);
//            }

//            return base.ConvertFrom(context, culture, value);
//        }
//    }
//}