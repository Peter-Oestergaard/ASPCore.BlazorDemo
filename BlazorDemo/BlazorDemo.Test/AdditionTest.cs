namespace BlazorDemo.Test;

using System.Globalization;
using AngleSharp.Dom;
using Pages;
using Bunit;

public sealed class AdditionTest : TestContext
{
    private readonly IElement _inputNum1;
    private readonly IElement _inputNum2;
    private readonly IElement _addButton;
    private readonly IElement _result;
    
    public AdditionTest()
    {
        // Arrange
        
        // Dealing with decimal separators we need to lock the test to a specific culture
        // in order to have stable string conversions of doubles
        CultureInfo.CurrentCulture = new CultureInfo("da-DK");
        
        IRenderedComponent<Calculator> calculatorComponent = RenderComponent<Calculator>();
        
        _inputNum1 = calculatorComponent.Find("input[placeholder='Enter First Number']");
        _inputNum2 = calculatorComponent.Find("input[placeholder='Enter Second Number']");
        _addButton = calculatorComponent.Find("button:contains('Add (+)')");
        _result = calculatorComponent.Find("input[readonly]");
    }
    
    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(0, -1)]
    [InlineData(1, -1)]
    public void AddingTwoIntegers_GivesCorrectResult(int a, int b)
    {
        // Act
        _inputNum1.Change(a.ToString());
        _inputNum2.Change(b.ToString());
        _addButton.Click();

        // Assert
        Assert.Equal((a + b).ToString(), _result.GetAttribute("value"));
    }
    
    [Theory]
    [InlineData(0, 0.0)]
    [InlineData(0, 0.1)]
    [InlineData(1, 0.1)]
    [InlineData(-1, -1.0)]
    public void AddingIntegersAndDoubles_GivesCorrectResult(int a, double b)
    {
        // Arrange
        string expected = (a + b).ToString(CultureInfo.CurrentCulture);
        
        // Act
        _inputNum1.Change(a.ToString());
        _inputNum2.Change(b.ToString(CultureInfo.CurrentCulture));
        _addButton.Click();

        // Assert
        string result = _result.GetAttribute("value");
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(0.0, 0.1)]
    [InlineData(1.0, 0.1)]
    [InlineData(-1.0, -1.0)]
    [InlineData(2.0, 3.0)]
    public void AddingDoublesAndDoubles_GivesCorrectResult(double a, double b)
    {
        // Arrange
        string expected = (a + b).ToString(CultureInfo.CurrentCulture);
        
        // Act
        _inputNum1.Change(a.ToString(CultureInfo.CurrentCulture));
        _inputNum2.Change(b.ToString(CultureInfo.CurrentCulture));
        _addButton.Click();

        // Assert
        string result = _result.GetAttribute("value");
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void AddingIntMaxToIntMax_GivesTwiceIntMax()
    {
        // Act
        _inputNum1.Change(int.MaxValue.ToString());
        _inputNum2.Change(int.MaxValue.ToString());
        _addButton.Click();

        // Assert
        double expected = (double)int.MaxValue + (double)int.MaxValue;
        Assert.Equal(expected.ToString(CultureInfo.CurrentCulture), _result.GetAttribute("value"));
    }
    
    [Fact]
    public void AddingDoubleMaxToDoubleMax_GivesPositiveInfinity()
    {
        // double.MaxValue is particularly problematic because even though this unit test is correct and stable,
        // it does not reflect what can actually be entered into the number input fields on the web page when
        // running the program. Sometimes it is possible to enter the max value of 1,7976931348623157E+308, but
        // other times it gives an overflow error in the JavaScript console. In those cases the max value allowed
        // to be entered is 1,7976931348623158E+292.
        
        double maxDouble = double.MaxValue;
        
        // Act
        _inputNum1.Change(maxDouble.ToString(CultureInfo.CurrentCulture));
        _inputNum2.Change(maxDouble.ToString(CultureInfo.CurrentCulture));
        _addButton.Click();

        // Assert
        double expected = double.PositiveInfinity;
        Assert.Equal(expected.ToString(CultureInfo.CurrentCulture), _result.GetAttribute("value"));
    }
}