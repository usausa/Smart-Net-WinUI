namespace Smart.WinUI;

using Microsoft.UI.Xaml.Controls;

public sealed class AncestorTests
{
    [Fact]
    public void ValueIsNullWithoutVisualParent()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var element = new TextBlock();

            // Act
            Ancestor.SetType(element, typeof(Border));

            // Assert
            Assert.Null(Ancestor.GetValue(element));
        });
    }

    [Fact]
    public void ClearsValueWhenTypeIsCleared()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var element = new TextBlock();
            Ancestor.SetType(element, typeof(Border));
            Ancestor.SetValue(element, new Border());

            // Act
            Ancestor.SetType(element, null);

            // Assert
            Assert.Null(Ancestor.GetValue(element));
        });
    }
}
