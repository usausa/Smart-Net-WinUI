namespace Smart.WinUI.Interactivity;

using Microsoft.Xaml.Interactivity;

using Smart.Mvvm.Messaging;

public sealed class EventRequestTrigger : RequestTriggerBase<ParameterEventArgs>
{
    protected override void OnEventRequest(object? sender, ParameterEventArgs e)
    {
        Interaction.ExecuteActions(AssociatedObject, Actions, e.Parameter);
    }
}
