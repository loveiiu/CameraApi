using System;
using System.ComponentModel;
using System.Reflection;

namespace WebApi.Utilities
{
    public static class EnumHelper
    {
        public static string GetDescription<TEnum>(this TEnum value)
     where TEnum : struct, IConvertible
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null) return string.Empty;

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
