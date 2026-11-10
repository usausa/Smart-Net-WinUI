namespace Smart.WinUI.Validation;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Smart.Mvvm.ViewModels;

public sealed class ValidationFocusBehaviorTests
{
    private static TextBox Keyed(string key)
    {
        var textBox = new TextBox();
        Behavior.SetKey(textBox, key);
        return textBox;
    }

    [Fact]
    public void FindTargetReturnsFirstElementWithError()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var second = Keyed("Second");
            var root = new StackPanel { Children = { Keyed("First"), new Border { Child = second }, Keyed("Third") } };
            using var errors = new ErrorInfo();
            errors.AddError("Second", "error");
            errors.AddError("Third", "error");

            // Act
            var target = ValidationFocusBehavior.FindTarget(root, errors);

            // Assert
            Assert.Same(second, target);
        });
    }

    [Fact]
    public void FindTargetSkipsDisabledAndCollapsedElements()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var disabled = Keyed("A");
            disabled.IsEnabled = false;
            var collapsed = Keyed("B");
            collapsed.Visibility = Visibility.Collapsed;
            var enabled = Keyed("D");
            var root = new StackPanel { Children = { disabled, collapsed, new Border { Visibility = Visibility.Collapsed, Child = Keyed("C") }, enabled } };
            using var errors = new ErrorInfo();
            foreach (var key in (string[])["A", "B", "C", "D"])
            {
                errors.AddError(key, "error");
            }

            // Act
            var target = ValidationFocusBehavior.FindTarget(root, errors);

            // Assert
            Assert.Same(enabled, target);
        });
    }

    [Fact]
    public void FindTargetReturnsNullWithoutError()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var root = new StackPanel { Children = { Keyed("First") } };
            using var errors = new ErrorInfo();

            // Act
            var target = ValidationFocusBehavior.FindTarget(root, errors);

            // Assert
            Assert.Null(target);
        });
    }
}
