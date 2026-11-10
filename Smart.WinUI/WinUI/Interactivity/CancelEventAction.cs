namespace Smart.WinUI.Interactivity;

using System.ComponentModel;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

public sealed class CancelEventAction : DependencyObject, IAction
{
    public static readonly DependencyProperty CancelProperty = DependencyProperty.Register(
        nameof(Cancel),
        typeof(bool),
        typeof(CancelEventAction),
        new PropertyMetadata(false));

    public bool Cancel
    {
        get => (bool)GetValue(CancelProperty);
        set => SetValue(CancelProperty, value);
    }

    public object Execute(object? sender, object? parameter)
    {
        switch (parameter)
        {
            case CancelEventArgs cancelArgs:
                cancelArgs.Cancel = Cancel;
                return true;
            case AppWindowClosingEventArgs closingArgs:
                closingArgs.Cancel = Cancel;
                return true;
            default:
                return false;
        }
    }
}
