namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

using Smart.WinUI.Internal;

public sealed class WindowCloseToHideAction : DependencyObject, IAction
{
    public static readonly DependencyProperty ClosableProperty = DependencyProperty.Register(
        nameof(Closable),
        typeof(bool),
        typeof(WindowCloseToHideAction),
        new PropertyMetadata(false));

    public bool Closable
    {
        get => (bool)GetValue(ClosableProperty);
        set => SetValue(ClosableProperty, value);
    }

    public object Execute(object? sender, object? parameter)
    {
        if (Closable || (parameter is not AppWindowClosingEventArgs args))
        {
            return false;
        }

        args.Cancel = true;
        WindowHelper.GetAppWindow(sender)?.Hide();
        return true;
    }
}
