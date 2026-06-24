using xUnit;
using task01;
public class StringExtensionsTests
{
    [Fact]
    public void IsPalindrome_ValidPalindrome_ReturnsTrue()
    {
        string input = "А роза упала на лапу Азора";
        Assert.True(input.IsPalindrome());
    }

    [Fact]
    public void IsPalindrome_NotPalindrome_ReturnsFalse()
    {
        string input = "Hello, world!";
        Assert.False(input.IsPalindrome());
    }

    [Fact]
    public void IsPalindrome_EmptyString_ReturnsFalse()
    {
        string input = "";
        Assert.False(input.IsPalindrome());
    }

    [Fact]
    public void IsPalindrome_WithPunctuation_IgnoresPunctuation_ReturnsTrue()
    {
        string input = "Was it a car or a cat I saw?";
        Assert.True(input.IsPalindrome());
    }

    [Fact]
    public void IsPalindrome_WithPunctuation_OnlySpace__ReturnsFalse()
    {
        string input = "  ";
        Assert.False(input.IsPalindrome());
    }

    [Fact]
    public void IsPalindrome_WithPunctuation_EasyWordManyspace()
    {
        string input = "За   к а   з";
        Assert.True(input.IsPalindrome());
    }
    [Fact]
    public void IsPalindrome_TwoChars_ReturnsTrue()
        => Assert.True("аа".IsPalindrome());

}