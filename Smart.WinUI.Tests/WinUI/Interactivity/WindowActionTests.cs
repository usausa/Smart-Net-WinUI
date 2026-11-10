namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml.Controls;

public sealed class WindowActionTests
{
    [Fact]
    public void WindowCloseActionReturnsFalseWithoutWindow()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new WindowCloseAction();

            // Act
            var result = action.Execute(new Border(), null);

            // Assert
            Assert.False((bool)result);
        });
    }

    [Fact]
    public void WindowPlacementActionReturnsFalseWithoutWindow()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new WindowPlacementAction();

            // Act
            var result = action.Execute(new Border(), null);

            // Assert
            Assert.False((bool)result);
        });
    }

    [Fact]
    public void WindowCloseToHideActionReturnsFalseForOtherParameter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new WindowCloseToHideAction();

            // Act
            var result = action.Execute(new Border(), EventArgs.Empty);

            // Assert
            Assert.False((bool)result);
        });
    }
}
