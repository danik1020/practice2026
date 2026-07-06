using System;
using System.IO;
using System.Linq;
using System.Reflection;
using task07;

namespace metainfo
{
    class Program
    {
        static void Main(string[] args)
        {
            string dllPath = args[0];

            if (!File.Exists(dllPath)) return;

            Assembly assembly = Assembly.LoadFrom(dllPath);

            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(t => t != null).ToArray();
            }

            foreach (var type in types)
            {
                if (type.IsInterface || type.IsAbstract) continue;

                Console.WriteLine($"Класс: {type.Name}");

                Console.WriteLine("Атрибуты класса:");
                foreach (var attr in type.GetCustomAttributes())
                {
                    Console.WriteLine($"{attr.GetType().Name}: ");
                    if (attr is DisplayNameAttribute displayName)
                        Console.WriteLine(displayName.DisplayName);
                    else if (attr is VersionAttribute version)
                        Console.WriteLine(version.Print());
                }

                Console.WriteLine("Конструкторы:");
                foreach (var c in type.GetConstructors())
                {
                    Console.WriteLine(type.Name);
                    var parameters = c.GetParameters();
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        Console.WriteLine($"{parameters[i].ParameterType.Name} {parameters[i].Name}");
                    }
                    
                }

                Console.WriteLine("Методы:");
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    Console.WriteLine($"{method.ReturnType.Name} {method.Name}");
                    var parameters = method.GetParameters();
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        Console.WriteLine($"{parameters[i].ParameterType.Name} {parameters[i].Name}");
                    }

                    foreach (var attr in method.GetCustomAttributes())
                    {
                        Console.WriteLine($"Атрибут: {attr.GetType().Name}: ");
                        if (attr is DisplayNameAttribute displayName)
                            Console.WriteLine(displayName.DisplayName);
                    }
                }

                Console.WriteLine("Свойства:");
                foreach (var prop in type.GetProperties())
                {
                    Console.WriteLine($"{prop.PropertyType.Name} {prop.Name}");
                    foreach (var attr in prop.GetCustomAttributes())
                    {
                        Console.WriteLine($"{attr.GetType().Name}");
                        if (attr is DisplayNameAttribute displayName)
                            Console.WriteLine($": {displayName.DisplayName}");
                    }
                }
            }
        }
    }
}
