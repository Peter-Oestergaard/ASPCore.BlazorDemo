namespace BlazorDemo.Test;

using Pages;
using Bunit;

public class UnitTest1 : TestContext
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var counterComponent = RenderComponent<Calculator>();
        
        
        counterComponent.Find("p").MarkupMatches("<p>Current count: 0</p>");

        // Act
        var element = counterComponent.Find("button");
        element.Click();

        //Assert
        counterComponent.Find("p").MarkupMatches("<p>Current count: 1</p>");
    }
}