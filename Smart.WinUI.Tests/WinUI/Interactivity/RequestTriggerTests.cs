namespace Smart.WinUI.Interactivity;

using System.ComponentModel;
using System.Reflection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

using Smart.Mvvm.Messaging;

public sealed class RequestTriggerTests
{
    private sealed class RecordingAction : DependencyObject, IAction
    {
        public object? LastParameter { get; private set; }

        public object Execute(object? sender, object? parameter)
        {
            LastParameter = parameter;
            if (parameter is CancelEventArgs cancel)
            {
                cancel.Cancel = true;
            }
            else if (parameter is ResolveEventArgs resolve)
            {
                resolve.Result = "resolved";
            }

            return true;
        }
    }

    private static RecordingAction Attach<TEventArgs>(RequestTriggerBase<TEventArgs> trigger)
        where TEventArgs : EventArgs
    {
        var action = new RecordingAction();
        trigger.Actions.Add(action);
        Interaction.GetBehaviors(new Border()).Add(trigger);
        typeof(RequestTriggerBase<TEventArgs>).GetMethod("OnLoaded", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(trigger, [trigger.AssociatedObject, null]);
        return action;
    }

    [Fact]
    public void EventRequestTriggerPassesParameter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var request = new EventRequest<int>();
            var action = Attach(new EventRequestTrigger { Request = request });

            // Act
            request.Request(5);

            // Assert
            Assert.Equal(5, action.LastParameter);
        });
    }

    [Fact]
    public void CancelRequestTriggerPassesEventArgs()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var request = new CancelRequest();
            Attach(new CancelRequestTrigger { Request = request });

            // Act
            var cancel = request.IsCancel();

            // Assert
            Assert.True(cancel);
        });
    }

    [Fact]
    public void ResolveRequestTriggerPassesEventArgs()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var request = new ResolveRequest<string>();
            Attach(new ResolveRequestTrigger { Request = request });

            // Act
            var result = request.Resolve();

            // Assert
            Assert.Equal("resolved", result);
        });
    }
}
