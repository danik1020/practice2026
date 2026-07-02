using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLib;

namespace CommandRunner
{
    class Program
    {
        static void Main()
        {
            string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileSystemCommands.dll");

            if (!File.Exists(dllPath)) return;

            Assembly assembly = Assembly.LoadFrom(dllPath);

            var commandTypes = assembly.GetTypes()
                .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();

            foreach (var type in commandTypes)
            {
                ICommand command = CreateCommand(type);
                if (command != null)
                {
                    command.Execute();
                }
            }
        }

        static ICommand CreateCommand(Type type)
        {
            if (type.Name == "DirectorySizeCommand")
            {
                return (ICommand)Activator.CreateInstance(type, new object[] { Directory.GetCurrentDirectory() });
            }
            else if (type.Name == "FindFilesCommand")
            {
                return (ICommand)Activator.CreateInstance(type, new object[] { Directory.GetCurrentDirectory(), "*.cs" });
            }

            return null;
        }
    }
}
