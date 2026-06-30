using System;
using System.Reflection;


namespace task07
{
    public class DisplayNameAttribute: Attribute
    {

        public string DisplayName { get; }
        public DisplayNameAttribute(string displayname) => DisplayName = displayname;
    }

    public class VersionAttribute: Attribute
    {
        public int Major { get; }
        public int Minor { get; }

    public VersionAttribute(int major, int minor)
    {
        Major = major;
        Minor = minor;
    }

    public string Print()
    {
        return $"{Major}.{Minor}";
    }
    }

    [Version(1, 0), DisplayName("Пример класса")]
    public class SampleClass
    {
        [DisplayName("Числовое свойство")]
        public int Number { get; }
        [DisplayName("Тестовый метод")] 
        public void TestMethod(){}
        public SampleClass(int num){Number = num;}
    }


}