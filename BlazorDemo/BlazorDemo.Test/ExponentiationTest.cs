namespace BlazorDemo.Test;

using System.Globalization;
using AngleSharp.Dom;
using Pages;
using Bunit;

public sealed class ExponentiationTest : TestContext
{
    private readonly IElement _inputNum1;
    private readonly IElement _inputNum2;
    private readonly IElement _addButton;
    private readonly IElement _result;

    public ExponentiationTest()
    {
        // Arrange

        // Dealing with decimal separators we need to lock the test to a specific culture
        // in order to have stable string conversions of doubles
        CultureInfo.CurrentCulture = new CultureInfo("da-DK");

        IRenderedComponent<Calculator> calculatorComponent = RenderComponent<Calculator>();

        _inputNum1 = calculatorComponent.Find("input[placeholder='Enter First Number']");
        _inputNum2 = calculatorComponent.Find("input[placeholder='Enter Second Number']");
        _addButton = calculatorComponent.Find("button:contains('Raise (^)')");
        _result = calculatorComponent.Find("input[readonly]");
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(1, -1)]
    [InlineData(4, 1)]
    [InlineData(4, 2)]
    [InlineData(4, 8)]
    public void RaisingTwoRealNumbers_GivesCorrectResult(double a, double b)
    {
        // Act
        _inputNum1.Change(a.ToString(CultureInfo.CurrentCulture));
        _inputNum2.Change(b.ToString(CultureInfo.CurrentCulture));
        _addButton.Click();

        // Assert
        Assert.Equal(Math.Pow(a, b).ToString(CultureInfo.CurrentCulture), _result.GetAttribute("value"));
    }

    [Theory]
    [InlineData("-0", "-1")]
    [InlineData("-0", "1")]
    [InlineData("-0", "-1,5")] // Decimal comma!
    [InlineData("-0", "1,5")]  // Decimal comma!
    public void DividingNegativeZeroWithRealNumbers_GivesZero(string a, string b)
    {
        // "-0" can be entered into first box so we need to test for it.

        // Act
        _inputNum1.Change(a);
        _inputNum2.Change(b);
        _addButton.Click();

        // Assert
        Assert.True(double.TryParse(a, out double ad));
        Assert.True(double.TryParse(b, out double bd));
        Assert.True(double.TryParse(_result.GetAttribute("value"), out double resultAsDouble));
        
        // Because of some magic and IEEE 754 some values results in negative zero. Negative zero and zero should be
        // indistinguishable from each other so we Math.Abs() it away to have our test succeed.
        string actual = Math.Abs(resultAsDouble).ToString(CultureInfo.CurrentCulture);
        Assert.Equal("0", actual);
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(0, 0.1)]
    [InlineData(1, 0.5)]
    [InlineData(-1, -1.5)]
    public void DividingRealNumbersWithRealNumbersExceptZero_GivesCorrectResult(int a, double b)
    {
        // Arrange
        string expected = (a / b).ToString(CultureInfo.CurrentCulture);

        // Act
        _inputNum1.Change(a.ToString());
        _inputNum2.Change(b.ToString(CultureInfo.CurrentCulture));
        _addButton.Click();

        // Assert
        string result = _result.GetAttribute("value");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DividingDoubleMaxWithDoubleMax_GivesOne()
    {
        double maxDouble = double.MaxValue;

        // Act
        _inputNum1.Change(maxDouble.ToString(CultureInfo.CurrentCulture));
        _inputNum2.Change(maxDouble.ToString(CultureInfo.CurrentCulture));
        _addButton.Click();

        // Assert
        double expected = 1;
        Assert.Equal(expected.ToString(CultureInfo.CurrentCulture), _result.GetAttribute("value"));
    }
    
    [Fact]
    public void DividingNegativeDoubleMaxWithNegativeDoubleMax_GivesOne()
    {
        double negativeMaxDouble = double.MaxValue * -1;

        // Act
        _inputNum1.Change(negativeMaxDouble.ToString(CultureInfo.CurrentCulture));
        _inputNum2.Change(negativeMaxDouble.ToString(CultureInfo.CurrentCulture));
        _addButton.Click();

        // Assert
        double expected = 1;
        Assert.Equal(expected.ToString(CultureInfo.CurrentCulture), _result.GetAttribute("value"));
    }
    
    [Fact]
    public void DividingDoubleMaxWithNegativeDoubleMax_GivesNegativeOne()
    {
        double maxDouble = double.MaxValue;
        double negativeMaxDouble = double.MaxValue * -1;

        // Act
        _inputNum1.Change(maxDouble.ToString(CultureInfo.CurrentCulture));
        _inputNum2.Change(negativeMaxDouble.ToString(CultureInfo.CurrentCulture));
        _addButton.Click();

        // Assert
        double expected = -1;
        Assert.Equal(expected.ToString(CultureInfo.CurrentCulture), _result.GetAttribute("value"));
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(1.5)]
    [InlineData(-1.5)]
    [InlineData(0)]
    [InlineData(double.MaxValue)]
    public void DividingRealNumbersWithZero_GivesError(double a)
    {
        // Act
        _inputNum1.Change(a.ToString(CultureInfo.CurrentCulture));
        _inputNum2.Change("0");
        _addButton.Click();

        // Assert
        string expected = "Cannot Divide by Zero";
        Assert.Equal(expected, _result.GetAttribute("value"));
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(1.5)]
    [InlineData(-1.5)]
    [InlineData(0)]
    [InlineData(double.MaxValue)]
    public void DividingRealNumbersWithNegativeZero_GivesError(double a)
    {
        // Act
        _inputNum1.Change(a.ToString(CultureInfo.CurrentCulture));
        _inputNum2.Change("-0");
        _addButton.Click();

        // Assert
        string expected = "Cannot Divide by Zero";
        Assert.Equal(expected, _result.GetAttribute("value"));
    }
}