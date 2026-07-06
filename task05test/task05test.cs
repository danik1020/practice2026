using System;
using System.Collections.Generic;
using System.Linq; 
using Xunit;
using _ClassAnalyzer;

public interface ISpaceship
{
    void MoveForward();      // Движение вперед
    void Rotate(int angle);  // Поворот на угол (градусы)
    void Fire();             // Выстрел ракетой
    int Speed { get; }       // Скорость корабля
    int FirePower { get; }   // Мощность выстрела
}

public class TestClass
{
    public int PublicField;
    private string _privateField;
    public int Property { get; set; }

    public void Method() { }
}

public class Cruiser : ISpaceship
{
    public int Speed => 50;
    public int FirePower => 100;
    int _angle = 0;
    int _x = 0;

    public void Fire() { }
    public void Rotate(int angle) { }
    public void MoveForward() { }
}


[Serializable]
public class AttributedClass { }
public class SpacecraftAttribute : Attribute
{
    public string Class { get; }
    public SpacecraftAttribute(string spacecraftClass)
    {Class = spacecraftClass;}
}
[SpacecraftAttribute("Military")]
public class MilitaryCruiser : ISpaceship
{
    public int Speed => 50;
    public int FirePower => 100;
    public void Fire() { }
    public void Rotate(int angle) { }
    public void MoveForward() { }
}

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
    }

    [Fact]
    public void GetMethodParams_Rotate_ReturnsAngleParam()
    {
        var analyzer = new ClassAnalyzer(typeof(Cruiser));
        var result = analyzer.GetMethodParams("Rotate").ToList();
        Assert.Equal("параметры Int32 и angle", result[0]);
        Assert.Equal("тип Void", result[1]);
    }


    [Fact]
    public void GetAllFields_Cruiser_ReturnsPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(Cruiser));
        var fields = analyzer.GetAllFields().ToList();
        Assert.Equal(2, fields.Count);
    }

    [Fact]
    public void GetProperties_Cruiser_ReturnsCorrectProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(Cruiser));
        var properties = analyzer.GetProperties().ToList();
        Assert.Equal(2, properties.Count);
    }

    [Fact]
    public void HasAttribute_Serializable_ReturnsTrue()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        Assert.True(analyzer.HasAttribute<SerializableAttribute>());
    }

    [Fact]
    public void HasAttribute_CustomAttribute_ReturnsTrue()
    {
        var analyzer = new ClassAnalyzer(typeof(MilitaryCruiser));
        Assert.True(analyzer.HasAttribute<SpacecraftAttribute>());
    }

}