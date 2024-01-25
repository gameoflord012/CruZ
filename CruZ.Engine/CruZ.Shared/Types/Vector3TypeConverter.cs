using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

namespace CruZ
{
    class Vector3TypeConverter : ExpandableObjectConverter
    {
        public override bool GetCreateInstanceSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(System.ComponentModel.ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
        {
            var valueTypeConverter = new ValueTypeTypeConverter();
            return valueTypeConverter.CreateInstance(context, propertyValues);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            var v3 = (Vector3)value;

            if(destinationType == typeof(string))
            {
                return $"{v3.X}, {v3.Y}, {v3.Z}";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if(value is string)
            {
                var valStr = (string)value;
                var split = valStr.Split(',');

                float.TryParse(split[0], out float x);
                float.TryParse(split[1], out float y);
                float.TryParse(split[2], out float z);

                return new Vector3(x, y, z);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}