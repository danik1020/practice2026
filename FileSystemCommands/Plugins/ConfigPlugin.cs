using System;
using CommandLib;
using task07;

namespace FileSystemCommands.Plugins
{
    [PluginLoad("LogPlugin")]
    public class ConfigPlugin : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("ConfigPlugin");
        }
    }
}
