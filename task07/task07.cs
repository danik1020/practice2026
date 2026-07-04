﻿using System;
using System.Reflection;

namespace task07
{
    public class DisplayNameAttribute : Attribute
    {
        public string DisplayName { get; }
        public DisplayNameAttribute(string displayname) => DisplayName = displayname;
    }

    public class VersionAttribute : Attribute
    {
        public int Major { get; }
        public int Minor { get; }
        public VersionAttribute(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }
        public string Print() => $"{Major}.{Minor}";
    }

    [Version(1, 0), DisplayName("Пример класса")]
    public class SampleClass
    {
        [DisplayName("Числовое свойство")]
        public int Number { get; }

        [DisplayName("Тестовый метод")]
        public void TestMethod() { }

        public SampleClass(int num) { Number = num; }
    }

    public static class ReflectionHelper
    {
        public static void PrintTypeInfo(Type type)
        {
            var displayName = type.GetCustomAttribute<DisplayNameAttribute>();
            if (displayName != null)
                Console.WriteLine($"Имя: {displayName.DisplayName}");

            var version = type.GetCustomAttribute<VersionAttribute>();
            if (version != null)
                Console.WriteLine($"Версия: {version.Print()}");

            foreach (var prop in type.GetProperties())
            {
                var attr = prop.GetCustomAttribute<DisplayNameAttribute>();
                if (attr != null)
                    Console.WriteLine($"Свойство: {attr.DisplayName}");
            }

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var attr = method.GetCustomAttribute<DisplayNameAttribute>();
                if (attr != null)
                    Console.WriteLine($"Метод: {attr.DisplayName}");
            }
        }
    }
}