using System;
using System.IO;
using System.Linq;
using CommandLib;
using task07;

namespace FileSystemCommands
{
    [task07.Version(1, 0), task07.DisplayName("Поиск файлов по маске")]
    public class FindFilesCommand : ICommand
    {
        string _path;
        string _mask;

        public FindFilesCommand(string path, string mask)
        {
            _path = path;
            _mask = mask;
        }

        [task07.DisplayName("Выполнить поиск")]
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
