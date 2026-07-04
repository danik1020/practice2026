using System;
using System.IO;
using Xunit;
using FileSystemCommands;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(testDir);
        command.Execute(); 

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(testDir, "*.txt");
        command.Execute(); 

        Directory.Delete(testDir, true);
    }


    [Fact]
    public void DirectorySizeCommand_ShouldOutputCorrectSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello"); 
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World"); 

        var output = new StringWriter();
        Console.SetOut(output);

        var command = new DirectorySizeCommand(testDir);
        command.Execute();

        var result = output.ToString();
        Assert.Contains("10", result);

        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldOutputFoundFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");
        File.WriteAllText(Path.Combine(testDir, "file3.txt"), "More");

        var output = new StringWriter();
        Console.SetOut(output);

        var command = new FindFilesCommand(testDir, "*.txt");
        command.Execute();

        var result = output.ToString();
        Assert.Contains("file1.txt", result);
        Assert.Contains("file3.txt", result);
        Assert.Contains("2", result);

        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        Directory.Delete(testDir, true);
    }

    [Fact]
    public void DirectorySizeCommand_ShouldHandleNonExistentDirectory()
    {
        var fakeDir = Path.Combine(Path.GetTempPath(), "NonExistentDir_" + DateTime.Now.Ticks);
        
        var command = new DirectorySizeCommand(fakeDir);
        
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);
    }

    [Fact]
    public void FindFilesCommand_ShouldHandleNonExistentDirectory()
    {
        var fakeDir = Path.Combine(Path.GetTempPath(), "NonExistentDir_" + DateTime.Now.Ticks);
        
        var command = new FindFilesCommand(fakeDir, "*.txt");
    
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);
    }
}