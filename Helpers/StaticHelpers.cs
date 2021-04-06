using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WBot2.Helpers
{
    public static class StaticHelpers
    {
        public static List<T> GetModules<T>()
        {
            return GetModules<T>(null);
        }

        public static Dictionary<Type, T> GetModulesWithType<T>()
        {
            var a = GetModules<T>();
            Dictionary<Type, T> dict = new();
            dict = a.ToDictionary(x => x.GetType().GetInterfaces().FirstOrDefault(i => i.IsGenericType).GetGenericArguments().FirstOrDefault(), x => x);
            return dict;
        }
        public static List<T> GetModules<T>(object[] constructorArgs)
        {
            List<T> list = new();
            foreach (Type type in GetModuleTypes<T>())
            {
                list.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            return list;
        }
        
        public static List<Type> GetModuleTypes<T>()
        {
            switch (typeof(T).IsInterface)
            {
                case true:
                    return Assembly.GetAssembly(typeof(T)).GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(T)) && !t.IsInterface).ToList();
                case false:
                    return Assembly.GetAssembly(typeof(T)).GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(T))).ToList();
            }
        }
    }
}
