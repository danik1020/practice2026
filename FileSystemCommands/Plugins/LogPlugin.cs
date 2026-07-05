using System;
using CommandLib;
using task07;

namespace FileSystemCommands.Plugins
{
    [PluginLoad]
    public class LogPlugin : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("LogPlugin");
        }
    }
}
