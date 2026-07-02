using System;
using System.IO;
using System.Linq;
using CommandLib;

namespace FileSystemCommands
{
    public class FindFilesCommand : ICommand
    {
        string _path;
        string _mask;

        public FindFilesCommand(string path, string mask)
        {
            _path = path;
            _mask = mask;
        }

        public void Execute()
        {
            if (!Directory.Exists(_path)) return;

            var files = Directory.GetFiles(_path, _mask, SearchOption.AllDirectories);
            
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
            
            Console.WriteLine(files.Length);
        }
    }
}
