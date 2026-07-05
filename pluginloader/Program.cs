using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CommandLib;
using task07;

namespace PluginLoader
{
    class Program
    {
        static List<Type> foundPlugins = new List<Type>();
        static List<Type> executionOrder = new List<Type>();
        static HashSet<string> processed = new HashSet<string>();

        static void BuildExecutionChain(Type plugin)
        {
            if (processed.Contains(plugin.Name)) return;

            var dependencies = plugin.GetCustomAttribute<PluginLoadAttribute>();
            
            if (dependencies?.DependsOn != null)
            {
                foreach (var dependency in dependencies.DependsOn)
                {
                    foreach (var candidate in foundPlugins)
                    {
                        if (candidate.Name == dependency)
                        {
                            BuildExecutionChain(candidate);
                            break;
                        }
                    }
                }
            }

            executionOrder.Add(plugin);
            processed.Add(plugin.Name);
        }

        public static void Main(string[] args)
        {
            string path = args[0];
            var files = Directory.GetFiles(path, "*.dll");

            foreach (var file in files)
            {
                var asm = Assembly.LoadFrom(file);
                foreach (var type in asm.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract)
                    {
                        var hasAttribute = type.GetCustomAttribute<PluginLoadAttribute>() != null;
                        var implementsCommand = typeof(ICommand).IsAssignableFrom(type);
                        
                        if (hasAttribute && implementsCommand)
                        {
                            foundPlugins.Add(type);
                        }
                    }
                }
            }

            foreach (var plugin in foundPlugins)
            {
                BuildExecutionChain(plugin);
            }

            foreach (var plugin in executionOrder)
            {
                var cmd = (ICommand)Activator.CreateInstance(plugin);
                cmd.Execute();
            }
        }
    }
}
