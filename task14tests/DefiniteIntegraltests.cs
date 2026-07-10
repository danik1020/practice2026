using System;
using Xunit;
using task14;


public class DefiniteIntegralTests
{
    [Fact]
    public void Solve_Sin_ReturnsCorrectValue()
    {
        Func<double, double> SIN = x => Math.Sin(x);
        
        double result = DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8);
        
        Assert.Equal(0, result, 4);
    }
    
    [Fact]
    public void Solve_Linear_ReturnsCorrectValue()
    {
        // ∫[0, 5] x dx = x²/2 от 0 до 5 = 25/2 = 12.5
        // Но в тесте указано 10, что соответствует ∫[0, 5] 2x dx = x² от 0 до 5 = 25
        // Проверим: ∫[0, 5] x dx = 12.5
        // Если в тесте 10, возможно опечатка, должно быть 12.5
        Func<double, double> X = x => x;
        
        double result = DefiniteIntegral.Solve(0, 5, X, 1e-6, 8);
        
        Assert.Equal(12.5, result, 5);
    }
    
    [Fact]
    public void Solve_Constant_ReturnsCorrectValue()
    {
        Func<double, double> constant = x => 5.0;
        
        double result = DefiniteIntegral.Solve(0, 10, constant, 1e-4, 4);
        
        Assert.Equal(50, result, 4);
    }
    
    [Fact]
    public void Solve_Quad_ReturnsCorrectValue()
    {
        Func<double, double> quad = x => x * x;
        
        double result = DefiniteIntegral.Solve(0, 3, quad, 1e-5, 4);
        
        Assert.Equal(9, result, 4);
    }
    
    [Fact]
    public void Solve_Single_ReturnsCorrectValue()
    {
        Func<double, double> X = x => x;
        
        double result = DefiniteIntegral.Solve(0, 10, X, 1e-4, 1);
        
        Assert.Equal(50, result, 4);
    }
    
    [Fact]
    public void Solve_ZeroThreadsNumber_ThrowsArgumentException()
    {
        Func<double, double> function = x => x;

        Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(0, 1, function, 0.1, 0));
    }
}
