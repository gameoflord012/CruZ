using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Numerics;

namespace CruZ
{
    class Vector2TypeConverter : ExpandableObjectConverter
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

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext? context, object value, Attribute[]? attributes)
        {
            return base.GetProperties(context, value, attributes);
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
            var v2 = (Vector2)value;

            if (destinationType == typeof(string))
            {
                return $"{v2.X}, {v2.Y}";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string)
            {
                var valStr = (string)value;
                var split = valStr.Split(',');

                float.TryParse(split[0], out float x);
                float.TryParse(split[1], out float y);

                return new Vector2(x, y);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}