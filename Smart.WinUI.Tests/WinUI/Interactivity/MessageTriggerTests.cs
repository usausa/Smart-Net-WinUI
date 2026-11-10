namespace Smart.WinUI.Interactivity;

using System.Reflection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

using Smart.Mvvm.Messaging;

public sealed class MessageTriggerTests
{
    private static readonly MethodInfo LoadedMethod =
        typeof(MessageTrigger).GetMethod("OnLoaded", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly MethodInfo UnloadedMethod =
        typeof(MessageTrigger).GetMethod("OnUnloaded", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private sealed class RecordingAction : DependencyObject, IAction
    {
        public int Count { get; private set; }

        public object? LastParameter { get; private set; }

        public object Execute(object? sender, object? parameter)
        {
            Count++;
            LastParameter = parameter;
            return true;
        }
    }

    private sealed class CustomMessenger : IMessenger
    {
        public event EventHandler<MessengerEventArgs>? Received;

        public void Send(string label) => Received?.Invoke(this, new MessengerEventArgs(label, typeof(object), null));

        public void Send<T>(T message) => Received?.Invoke(this, new MessengerEventArgs(string.Empty, typeof(T), message));

        public void Send<T>(string label, T parameter) => Received?.Invoke(this, new MessengerEventArgs(label, typeof(T), parameter));
    }

    private static (MessageTrigger Trigger, RecordingAction Action) Attach(IMessenger messenger)
    {
        var element = new Border();
        var action = new RecordingAction();
        var trigger = new MessageTrigger { Messenger = messenger };
        trigger.Actions.Add(action);
        Interaction.GetBehaviors(element).Add(trigger);
        return (trigger, action);
    }

    private static void RaiseLoaded(MessageTrigger trigger) => LoadedMethod.Invoke(trigger, [trigger.AssociatedObject, null]);

    private static void RaiseUnloaded(MessageTrigger trigger) => UnloadedMethod.Invoke(trigger, [trigger.AssociatedObject, null]);

    [Fact]
    public void ReactsAfterUnloadAndReload()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var messenger = new Messenger();
            var (trigger, action) = Attach(messenger);

            // Act
            RaiseLoaded(trigger);
            RaiseUnloaded(trigger);
            RaiseLoaded(trigger);
            messenger.Send("test");

            // Assert
            Assert.Equal(1, action.Count);
        });
    }

    [Fact]
    public void DoesNotReactWhileUnloaded()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var messenger = new Messenger();
            var (trigger, action) = Attach(messenger);

            // Act
            RaiseLoaded(trigger);
            RaiseUnloaded(trigger);
            messenger.Send("test");

            // Assert
            Assert.Equal(0, action.Count);
        });
    }

    [Fact]
    public void ReactsWithCustomMessengerImplementation()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var messenger = new CustomMessenger();
            var (trigger, action) = Attach(messenger);

            // Act
            RaiseLoaded(trigger);
            messenger.Send("test");

            // Assert
            Assert.Equal(1, action.Count);
        });
    }
}
