using System;
using CommandLib;
using task07;

namespace FileSystemCommands.Plugins
{
    [PluginLoad("LogPlugin", "ConfigPlugin")]
    public class DataPlugin : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("DataPlugin");
        }
    }
}
