namespace BlazorDemo.Test;

using System.Globalization;
using AngleSharp.Dom;
using Pages;
using Bunit;

public sealed class ModuloTest : TestContext
{
    private readonly IElement _inputNum1;
    private readonly IElement _inputNum2;
    private readonly IElement _addButton;
    private readonly IElement _result;

    public ModuloTest()
    {
        // Arrange

        // Dealing with decimal separators we need to lock the test to a specific culture
        // in order to have stable string conversions of doubles
        CultureInfo.CurrentCulture = new CultureInfo("da-DK");

        IRenderedComponent<Calculator> calculatorComponent = RenderComponent<Calculator>();

        _inputNum1 = calculatorComponent.Find("input[placeholder='Enter First Number']");
        _inputNum2 = calculatorComponent.Find("input[placeholder='Enter Second Number']");
        _addButton = calculatorComponent.Find("button:contains('Modulo (mod)')");
        _result = calculatorComponent.Find("input[readonly]");
    }

    [Theory]
    [InlineData(-1, 2)]
    [InlineData(-1, -2)]
    [InlineData(1, 2)]
    [InlineData(1, -2)]
    [InlineData(-1.5, 2)]
    [InlineData(-1.5, -2)]
    [InlineData(1.5, 2)]
    [InlineData(1.5, -2)]
    [InlineData(-1, 2.5)]
    [InlineData(-1, -2.5)]
    [InlineData(1, 2.5)]
    [InlineData(1, -2.5)]
    [InlineData(-1.5, 3.5)]
    [InlineData(-1.5, -3.5)]
    [InlineData(1.5, 3.5)]
    [InlineData(1.5, -3.5)]
    [InlineData(-1, double.MaxValue)]
    [InlineData(-1, double.MaxValue * -1)]
    [InlineData(1, double.MaxValue)]
    [InlineData(1, double.MaxValue * -1)]
    [InlineData(-1.5, double.MaxValue)]
    [InlineData(-1.5, double.MaxValue * -1)]
    [InlineData(1.5, double.MaxValue)]
    [InlineData(1.5, double.MaxValue * -1)]
    [InlineData(double.MaxValue, -1)]
    [InlineData(double.MaxValue * -1, -1)]
    [InlineData(double.MaxValue, 1)]
    [InlineData(double.MaxValue * -1, 1)]
    [InlineData(double.MaxValue, -1.5)]
    [InlineData(double.MaxValue * -1, -1.5)]
    [InlineData(double.MaxValue, 1.5)]
    [InlineData(double.MaxValue * -1, 1.5)]
    [InlineData(double.MaxValue, double.MaxValue)]
    [InlineData(double.MaxValue * -1, double.MaxValue)]
    [InlineData(double.MaxValue, double.MaxValue * -1)]
    [InlineData(double.MaxValue * -1, double.MaxValue * -1)]
    public void RealNumbersModuloRealNumbersExceptZero_GivesCorrectResult(double a, double b)
    {
        var x = a % b;
        // Act
        _inputNum1.Change(a.ToString(CultureInfo.CurrentCulture));
        _inputNum2.Change(b.ToString(CultureInfo.CurrentCulture));
        _addButton.Click();

        // Assert
        Assert.Equal((a % b).ToString(CultureInfo.CurrentCulture), _result.GetAttribute("value"));
    }

    [Theory]
    [InlineData(-1.5)]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(1.5)]
    public void SignedZeroModuloRealNumbersExceptZero_GivesZero(double a)
    {
        // Act
        _inputNum1.Change("0");
        _inputNum2.Change(a.ToString(CultureInfo.CurrentCulture));
        _addButton.Click();

        // Assert
        string actual = _result.GetAttribute("value");
        Assert.Equal("0", actual);

        // "-0" can be entered into first box so we need to test for it.
        // Act
        _inputNum1.Change("-0");
        _addButton.Click();

        // Assert
        actual = _result.GetAttribute("value");
        Assert.Equal("0", actual);
    }
    
    [Fact]
    public void RaisingSignedZeroToNegativeMaxDouble_GivesError()
    {
        // Act
        _inputNum1.Change("0");
        _inputNum2.Change((double.MaxValue * -1).ToString(CultureInfo.CurrentCulture));
        _addButton.Click();

        // Assert
        string actual = _result.GetAttribute("value");
        Assert.Equal("Cannot raise zero to negative exponent", actual);
        
        // "-0" can be entered into first box so we need to test for it.
        // Act
        _inputNum1.Change("-0");
        _addButton.Click();

        // Assert
        actual = _result.GetAttribute("value");
        Assert.Equal("Cannot raise zero to negative exponent", actual);
    }
    
    [Theory]
    [InlineData("0", "0")]
    [InlineData("0", "-0")]
    [InlineData("-0", "0")]
    [InlineData("-0", "-0")]
    public void RaisingSignedZeroToSignedZero_GivesOne(string a, string b)
    {
        // "-0" can be entered into either box so we need to test for it.

        // Act
        _inputNum1.Change(a);
        _inputNum2.Change(b);
        _addButton.Click();

        // Assert
        string actual = _result.GetAttribute("value");
        Assert.Equal("1", actual);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(1.5)]
    [InlineData(-1.5)]
    [InlineData(double.MaxValue)]
    [InlineData(double.MaxValue * -1)]
    public void RaisingRealNumbersExceptZeroToSignedZero_GivesOne(double a)
    {

        // Act
        _inputNum1.Change(a.ToString(CultureInfo.CurrentCulture));
        _inputNum2.Change("0");
        _addButton.Click();

        // Assert
        string actual = _result.GetAttribute("value");
        Assert.Equal("1", actual);
        
        // Act
        // "-0" can be entered into second box so we need to test for it.
        _inputNum2.Change("-0");
        _addButton.Click();

        // Assert
        actual = _result.GetAttribute("value");
        Assert.Equal("1", actual);
    }

    [Theory]
    [InlineData(0.1)]
    [InlineData(1)]
    [InlineData(1.5)]
    [InlineData(double.MaxValue)]
    public void RaisingSignedZeroToPositiveRealNumbers_GivesZero(double a)
    {

        // Act
        _inputNum1.Change("0");
        _inputNum2.Change(a.ToString(CultureInfo.CurrentCulture));
        _addButton.Click();

        // Assert
        string actualString = _result.GetAttribute("value");
        Assert.Equal("0", actualString);
        
        // Act
        // "-0" can be entered into first box so we need to test for it.
        _inputNum1.Change("-0");
        _addButton.Click();

        // Assert
        actualString = _result.GetAttribute("value");
        
        // Get rid of sign in front of zero
        double actual = Math.Abs(double.Parse(actualString));
        Assert.Equal(0, actual);
    }
}