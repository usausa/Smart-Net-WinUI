namespace Smart.WinUI.Interactivity;

using System.Diagnostics;

using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

public sealed class LaunchUriOrFileAction : DependencyObject, IAction
{
    public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
        nameof(Path),
        typeof(string),
        typeof(LaunchUriOrFileAction),
        new PropertyMetadata(null));

    public string? Path
    {
        get => (string?)GetValue(PathProperty);
        set => SetValue(PathProperty, value);
    }

    public object Execute(object? sender, object? parameter)
    {
        if (String.IsNullOrEmpty(Path))
        {
            return false;
        }

        using var process = Process.Start(new ProcessStartInfo(Path) { UseShellExecute = true });
        return true;
    }
}
