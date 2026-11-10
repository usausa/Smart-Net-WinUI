namespace Smart.WinUI.Interactivity;

using Microsoft.Xaml.Interactivity;

using Smart.Mvvm.Messaging;

public sealed class ResolveRequestTrigger : RequestTriggerBase<ResolveEventArgs>
{
    protected override void OnEventRequest(object? sender, ResolveEventArgs e)
    {
        Interaction.ExecuteActions(AssociatedObject, Actions, e);
    }
}
