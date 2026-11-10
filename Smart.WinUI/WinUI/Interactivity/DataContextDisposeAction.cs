namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

public sealed class DataContextDisposeAction : DependencyObject, IAction
{
    public object Execute(object? sender, object? parameter)
    {
        if ((sender as FrameworkElement)?.DataContext is not IDisposable disposable)
        {
            return false;
        }

        disposable.Dispose();
        return true;
    }
}
