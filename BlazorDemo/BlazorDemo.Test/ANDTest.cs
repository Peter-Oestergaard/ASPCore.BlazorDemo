using System.Numerics;

namespace BlazorDemo.Test;

using System.Globalization;
using AngleSharp.Dom;
using Pages;
using Bunit;

public sealed class ANDTest : TestContext
{
    private readonly IElement _inputNum1;
    private readonly IElement _inputNum2;
    private readonly IElement _addButton;
    private readonly IElement _result;

    public ANDTest()
    {
        // Arrange

        // Dealing with decimal separators we need to lock the test to a specific culture
        // in order to have stable string conversions of doubles
        CultureInfo.CurrentCulture = new CultureInfo("da-DK");

        IRenderedComponent<Calculator> calculatorComponent = RenderComponent<Calculator>();

        _inputNum1 = calculatorComponent.Find("input[placeholder='Enter First Number']");
        _inputNum2 = calculatorComponent.Find("input[placeholder='Enter Second Number']");
        _addButton = calculatorComponent.Find("button:contains('AND (&)')");
        _result = calculatorComponent.Find("input[readonly]");
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(3, 0)]
    [InlineData(-1, 2)]
    [InlineData(-1, -2)]
    [InlineData(1, 2)]
    [InlineData(1, -2)]
    [InlineData(-1, long.MaxValue)]
    [InlineData(-1, long.MaxValue * -1)]
    [InlineData(1, long.MaxValue)]
    [InlineData(1, long.MaxValue * -1)]
    [InlineData(long.MaxValue, -1)]
    [InlineData(long.MaxValue * -1, -1)]
    [InlineData(long.MaxValue, 1)]
    [InlineData(long.MaxValue * -1, 1)]
    [InlineData(long.MaxValue, long.MaxValue)]
    [InlineData(long.MaxValue * -1, long.MaxValue)]
    [InlineData(long.MaxValue, long.MaxValue * -1)]
    [InlineData(long.MaxValue * -1, long.MaxValue * -1)]
    public void ANDingTwoIntegers_GivesCorrectResult(long a, long b)
    {
        // Act
        _inputNum1.Change(a.ToString(CultureInfo.CurrentCulture));
        _inputNum2.Change(b.ToString(CultureInfo.CurrentCulture));
        _addButton.Click();

        // Assert
        Assert.Equal((a & b).ToString(CultureInfo.CurrentCulture), _result.GetAttribute("value"));
    }

    [Theory]
    [InlineData(long.MaxValue)]
    [InlineData(2)]
    [InlineData(1)]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(long.MaxValue * -1)]
    public void ANDingNegativeZero_GivesZero(long a)
    {
        // Act
        _inputNum1.Change("-0");
        _inputNum2.Change(a.ToString(CultureInfo.CurrentCulture));
        _addButton.Click();

        // Assert
        string actual = _result.GetAttribute("value");
        Assert.Equal("0", actual);
        
        // Act
        _inputNum1.Change(a.ToString(CultureInfo.CurrentCulture));
        _inputNum2.Change("-0");
        _addButton.Click();

        // Assert
        actual = _result.GetAttribute("value");
        Assert.Equal("0", actual);
    }
    
    [Fact]
    public void ANDingNegativeZeroWithNegativeZero_GivesZero()
    {
        // Act
        _inputNum1.Change("-0");
        _inputNum2.Change("-0");
        _addButton.Click();
        
        // Assert
        Assert.Equal("0", _result.GetAttribute("value"));
    }
    
    [Theory]
    [InlineData(1.5, 4)]
    [InlineData(2, 2.4)]
    [InlineData(0.7, 10.9)]
    public void ANDingDoubles_GivesError(double a, double b)
    {
        // Act
        _inputNum1.Change(a.ToString(CultureInfo.CurrentCulture));
        _inputNum2.Change(b.ToString(CultureInfo.CurrentCulture));
        _addButton.Click();
        
        // Assert
        Assert.Equal("Cannot AND non-integers", _result.GetAttribute("value"));
    }
    
}