using System;
using System.Reflection;

namespace WBot2.Extensions
{
    public static class MemberInfoExtensions
    {
        public static bool HasCustomAttribute<T> (this MemberInfo info) where T : Attribute
        {
            return info.GetCustomAttribute<T>() != null;
        }
    }
}
