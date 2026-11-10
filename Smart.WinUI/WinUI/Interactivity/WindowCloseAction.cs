namespace Smart.WinUI.Interactivity;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

using Smart.WinUI.Internal;

public sealed class WindowCloseAction : DependencyObject, IAction
{
    public object Execute(object? sender, object? parameter)
    {
        var window = WindowHelper.GetAppWindow(sender);
        return (window is not null) && NativeMethods.PostMessage(Win32Interop.GetWindowFromWindowId(window.Id), NativeMethods.WmClose, IntPtr.Zero, IntPtr.Zero);
    }
}
