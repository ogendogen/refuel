using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Utils
{
    public static class EnumDescriptionHelper
    {
        public static string GetCustomDescription(object objEnum)
        {
            var fi = objEnum.GetType().GetField(objEnum.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : objEnum.ToString();
        }

        public static string GetDisplayName(object objEnum)
        {
            var fi = objEnum.GetType().GetField(objEnum.ToString());
            var displayName = fi.GetCustomAttributesData().FirstOrDefault()?.NamedArguments?.FirstOrDefault().TypedValue.ToString().Trim('"');
            return displayName ?? objEnum.ToString();
        }

        public static string DisplayName(this Enum value)
        {
            return GetDisplayName(value);
        }

        public static string Description(this Enum value)
        {
            return GetCustomDescription(value);
        }
    }
}
