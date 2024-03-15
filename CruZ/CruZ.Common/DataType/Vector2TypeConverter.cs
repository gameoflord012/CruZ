using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Numerics;

namespace CruZ.Common.DataType
{
    /// <summary>
    /// Convert <see cref="Vector2"/> to <see cref="string"/> and vice-versa
    /// </summary>
    class Vector2TypeConverter : TypeConverter
    {
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
                float x = 0, y = 0;

                var valStr = (string)value;
                var split = valStr.Split(',');

                try
                {
                    float.TryParse(split[0], out x);
                    float.TryParse(split[1], out y);
                }
                catch (IndexOutOfRangeException)
                {

                }

                return new Vector2(x, y);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}