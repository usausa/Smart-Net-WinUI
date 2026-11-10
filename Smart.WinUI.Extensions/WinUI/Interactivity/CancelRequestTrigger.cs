namespace Smart.WinUI.Interactivity;

using System.ComponentModel;

using Microsoft.Xaml.Interactivity;

public sealed class CancelRequestTrigger : RequestTriggerBase<CancelEventArgs>
{
    protected override void OnEventRequest(object? sender, CancelEventArgs e)
    {
        Interaction.ExecuteActions(AssociatedObject, Actions, e);
    }
}
