namespace Smart.WinUI.Interactivity;

using System.Reflection;

using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

using Smart.WinUI.Input;

public sealed class TypingStoppedBehaviorTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);

    private static readonly TimeSpan Wait = TimeSpan.FromMilliseconds(200);

    private static readonly MethodInfo TextChangedMethod =
        typeof(TypingStoppedBehavior).GetMethod("OnTextChanged", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static void ChangeText(TypingStoppedBehavior behavior, TextBox textBox, string text)
    {
        textBox.Text = text;
        TextChangedMethod.Invoke(behavior, [textBox, null]);
    }

    [Fact]
    public void ExecutesCommandWithTextAfterDelay()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var textBox = new TextBox();
            object? received = null;
            var behavior = new TypingStoppedBehavior
            {
                Command = new DelegateCommand<object?>(x => received = x),
                Delay = TimeSpan.FromMilliseconds(10)
            };
            Interaction.GetBehaviors(textBox).Add(behavior);

            // Act
            ChangeText(behavior, textBox, "abc");
            XamlThread.ProcessUntil(() => received is not null, Timeout);

            // Assert
            Assert.Equal("abc", received);
        });
    }

    [Fact]
    public void ExecutesCommandOnceWithLatestText()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var textBox = new TextBox();
            var received = new List<object?>();
            var behavior = new TypingStoppedBehavior
            {
                Command = new DelegateCommand<object?>(received.Add),
                Delay = TimeSpan.FromMilliseconds(50)
            };
            Interaction.GetBehaviors(textBox).Add(behavior);

            // Act
            ChangeText(behavior, textBox, "a");
            ChangeText(behavior, textBox, "ab");
            XamlThread.ProcessUntil(() => received.Count > 0, Timeout);
            XamlThread.ProcessUntil(static () => false, Wait);

            // Assert
            Assert.Equal(["ab"], received);
        });
    }

    [Fact]
    public void UsesCommandParameterWhenSet()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var textBox = new TextBox();
            object? received = null;
            var behavior = new TypingStoppedBehavior
            {
                Command = new DelegateCommand<object?>(x => received = x),
                CommandParameter = "parameter",
                Delay = TimeSpan.FromMilliseconds(10)
            };
            Interaction.GetBehaviors(textBox).Add(behavior);

            // Act
            ChangeText(behavior, textBox, "abc");
            XamlThread.ProcessUntil(() => received is not null, Timeout);

            // Assert
            Assert.Equal("parameter", received);
        });
    }

    [Fact]
    public void DoesNotExecuteCommandAfterDetached()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var textBox = new TextBox();
            var count = 0;
            var behavior = new TypingStoppedBehavior
            {
                Command = new DelegateCommand(() => count++),
                Delay = TimeSpan.FromMilliseconds(50)
            };
            Interaction.GetBehaviors(textBox).Add(behavior);
            ChangeText(behavior, textBox, "abc");

            // Act
            Interaction.GetBehaviors(textBox).Remove(behavior);
            XamlThread.ProcessUntil(static () => false, Wait);

            // Assert
            Assert.Equal(0, count);
        });
    }
}
