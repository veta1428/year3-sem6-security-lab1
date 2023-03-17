using System;
using System.Linq;
using System.Reflection;

namespace SecurityReport.Enums;
internal static class EnumExtension
{
    public static TAttribute? GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
    {
        return value
            .GetType()
            .GetMember(value.ToString())
            .First()
            .GetCustomAttribute<TAttribute>();
    }
}
