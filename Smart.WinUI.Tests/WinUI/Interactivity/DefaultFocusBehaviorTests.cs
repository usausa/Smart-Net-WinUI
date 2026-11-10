namespace Smart.WinUI.Interactivity;

using System.Reflection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

public sealed class DefaultFocusBehaviorTests
{
    private static readonly MethodInfo FindTargetMethod =
        typeof(DefaultFocusBehavior).GetMethod("FindTarget", BindingFlags.Static | BindingFlags.NonPublic)!;

    private static UIElement? FindTarget(DependencyObject parent) =>
        (UIElement?)FindTargetMethod.Invoke(null, [parent]);

    private static TextBox Target(bool isTarget = true)
    {
        var textBox = new TextBox();
        DefaultFocus.SetIsTarget(textBox, isTarget);
        return textBox;
    }

    [Fact]
    public void FindsFirstTarget()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var first = Target();
            var panel = new StackPanel { Children = { new TextBox(), Target(false), first, Target() } };

            // Act
            var target = FindTarget(panel);

            // Assert
            Assert.Same(first, target);
        });
    }

    [Fact]
    public void SkipsDisabledAndHiddenTargets()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var disabled = Target();
            disabled.IsEnabled = false;
            var hidden = Target();
            hidden.Visibility = Visibility.Collapsed;
            var expected = Target();
            var panel = new StackPanel { Children = { disabled, hidden, expected } };

            // Act
            var target = FindTarget(panel);

            // Assert
            Assert.Same(expected, target);
        });
    }

    [Fact]
    public void SkipsTargetsInsideHiddenContainer()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var hidden = new StackPanel { Visibility = Visibility.Collapsed, Children = { Target() } };
            var expected = Target();
            var panel = new StackPanel { Children = { hidden, new StackPanel { Children = { expected } } } };

            // Act
            var target = FindTarget(panel);

            // Assert
            Assert.Same(expected, target);
        });
    }

    [Fact]
    public void UsesBoundTarget()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var bound = new TextBox();
            BindingOperations.SetBinding(bound, DefaultFocus.IsTargetProperty, new Binding { Path = new PropertyPath(nameof(Model.IsNew)), Source = new Model(false) });
            var expected = Target();
            var panel = new StackPanel { Children = { bound, expected } };

            // Act
            var target = FindTarget(panel);

            // Assert
            Assert.Same(expected, target);
        });
    }

    [Fact]
    public void ReturnsNullWithoutTarget()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var panel = new StackPanel { Children = { new TextBox(), Target(false) } };

            // Act
            var target = FindTarget(panel);

            // Assert
            Assert.Null(target);
        });
    }

    private sealed class Model(bool isNew)
    {
        public bool IsNew { get; } = isNew;
    }
}
