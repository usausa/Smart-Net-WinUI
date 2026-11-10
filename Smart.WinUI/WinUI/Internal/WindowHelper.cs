namespace Smart.WinUI.Internal;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

internal static class WindowHelper
{
    public static AppWindow? GetAppWindow(object? element) =>
        (element as UIElement)?.XamlRoot?.ContentIslandEnvironment is { } environment ? AppWindow.GetFromWindowId(environment.AppWindowId) : null;
}
