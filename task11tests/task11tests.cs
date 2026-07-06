using Xunit;
using task11;

namespace task11tests
{
    public class CalculatorTests
    {
        string CalculatorSource = @"
using task11;

public class Calculator : ICalculator
{
    public int Add(int a, int b) => a + b;
    public int Minus(int a, int b) => a - b;
    public int Mul(int a, int b) => a * b;
    public int Div(int a, int b) => a / b;
}";

        [Fact]
        public void Add_ShouldWork()
        {
            var generator = new CalculatorGenerator();
            ICalculator calc = generator.CreateCalculator(CalculatorSource);

            Assert.Equal(8, calc.Add(5, 3));
        }

        [Fact]
        public void Minus_ShouldWork()
        {
            var generator = new CalculatorGenerator();
            ICalculator calc = generator.CreateCalculator(CalculatorSource);

            Assert.Equal(6, calc.Minus(10, 4));
        }

        [Fact]
        public void Mul_ShouldWork()
        {
            var generator = new CalculatorGenerator();
            ICalculator calc = generator.CreateCalculator(CalculatorSource);

            Assert.Equal(42, calc.Mul(6, 7));
        }

        [Fact]
        public void Div_ShouldWork()
        {
            var generator = new CalculatorGenerator();
            ICalculator calc = generator.CreateCalculator(CalculatorSource);

            Assert.Equal(4, calc.Div(20, 5));
        }
    }
}
