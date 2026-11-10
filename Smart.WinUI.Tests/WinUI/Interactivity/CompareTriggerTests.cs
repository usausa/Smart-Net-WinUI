namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

using Smart.WinUI.Expressions;

public sealed class CompareTriggerTests
{
    private sealed class RecordAction : DependencyObject, IAction
    {
        public List<(object? Sender, object? Parameter)> Calls { get; } = [];

        public object Execute(object? sender, object? parameter)
        {
            Calls.Add((sender, parameter));
            return true;
        }
    }

    private static (Border Element, CompareTrigger Trigger, RecordAction Action) Create()
    {
        var element = new Border();
        var trigger = new CompareTrigger();
        var action = new RecordAction();
        trigger.Actions.Add(action);
        Interaction.GetBehaviors(element).Add(trigger);
        return (element, trigger, action);
    }

    [Fact]
    public void ExecutesActionsWhenBindingEqualsParameter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var (element, trigger, action) = Create();
            trigger.Parameter = 5;

            // Act
            trigger.Binding = 5;

            // Assert
            var (sender, parameter) = Assert.Single(action.Calls);
            Assert.Same(element, sender);
            Assert.Null(parameter);
        });
    }

    [Fact]
    public void DoesNotExecuteActionsWhenBindingDiffersFromParameter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var (_, trigger, action) = Create();
            trigger.Parameter = 5;

            // Act
            trigger.Binding = 6;

            // Assert
            Assert.Empty(action.Calls);
        });
    }

    [Fact]
    public void UsesExpression()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var (_, trigger, action) = Create();
            trigger.Expression = CompareExpressions.GreaterThan;
            trigger.Parameter = 5;

            // Act
            trigger.Binding = 6;

            // Assert
            Assert.Single(action.Calls);
        });
    }

    [Fact]
    public void DoesNotExecuteActionsWhenDetached()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var (element, trigger, action) = Create();
            Interaction.GetBehaviors(element).Remove(trigger);
            trigger.Parameter = 5;

            // Act
            trigger.Binding = 5;

            // Assert
            Assert.Empty(action.Calls);
        });
    }

    [Fact]
    public void ExpressionIsEqualByDefault()
    {
        XamlThread.Run(static () =>
        {
            // Arrange & Act
            var trigger = new CompareTrigger();

            // Assert
            Assert.Same(CompareExpressions.Equal, trigger.Expression);
        });
    }
}
