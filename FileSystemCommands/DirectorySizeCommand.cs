using System;
using System.IO;
using System.Linq;
using CommandLib;
using task07;

namespace FileSystemCommands
{
    [task07.Version(1, 0), task07.DisplayName("Вычисление размера каталога")]
    public class DirectorySizeCommand : ICommand
    {
        string _path;

        public DirectorySizeCommand(string path)
        {
            _path = path;
        }

        [task07.DisplayName("Выполнить расчет")]
        public void Execute()
        {
            if (!Directory.Exists(_path)) return;
            
            long size = CalculateDirectorySize(_path);
            Console.WriteLine(size);
        }

        long CalculateDirectorySize(string path)
        {
            long size = 0;
            size += Directory.GetFiles(path).Sum(f => new FileInfo(f).Length);

            foreach (var dir in Directory.GetDirectories(path))
            {
                size += CalculateDirectorySize(dir);
            }
            return size;
        }
    }
}
