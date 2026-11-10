namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml.Controls;

public sealed class ScrollIntoActionTests
{
    [Fact]
    public void ReturnsTrueForListWithItems()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var listView = new ListView { Items = { "a", "b" } };
            var action = new ScrollIntoAction { Position = ScrollPosition.First };

            // Act
            var result = action.Execute(listView, null);

            // Assert
            Assert.True((bool)result);
        });
    }

    [Fact]
    public void ReturnsFalseForEmptyList()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var listBox = new ListBox();
            var action = new ScrollIntoAction();

            // Act
            var result = action.Execute(listBox, null);

            // Assert
            Assert.False((bool)result);
        });
    }

    [Fact]
    public void ReturnsFalseForOtherSelector()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var comboBox = new ComboBox { Items = { "a" } };
            var action = new ScrollIntoAction();

            // Act
            var result = action.Execute(comboBox, null);

            // Assert
            Assert.False((bool)result);
        });
    }

    [Fact]
    public void PositionIsLastByDefault()
    {
        XamlThread.Run(static () =>
        {
            // Arrange & Act
            var action = new ScrollIntoAction();

            // Assert
            Assert.Equal(ScrollPosition.Last, action.Position);
        });
    }
}
