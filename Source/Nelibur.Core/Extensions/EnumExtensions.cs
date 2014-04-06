using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Nelibur.Core.DataStructures;

namespace Nelibur.Core.Extensions
{
    public static class EnumExtensions
    {
        public static Bag<TAttribute> GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            return GetAttributes<TAttribute>(value).SingleOrDefault()
                .ToBag();
        }

        public static string ToDescription(this Enum value)
        {
            DescriptionAttribute attribute = GetAttributes<DescriptionAttribute>(value).SingleOrDefault();
            if (attribute != null)
            {
                return attribute.Description;
            }
            return value.ToString();
        }

        private static TAttribute[] GetAttributes<TAttribute>(Enum value)
            where TAttribute : Attribute
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(Enum.GetName(type, value));
            return (TAttribute[])Attribute.GetCustomAttributes(fieldInfo, typeof(TAttribute));
        }
    }
}
