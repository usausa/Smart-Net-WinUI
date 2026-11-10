namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

using Smart.WinUI.Internal;

public sealed class WindowPlacementAction : DependencyObject, IAction
{
    public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(
        nameof(Placement),
        typeof(WindowPlacement),
        typeof(WindowPlacementAction),
        new PropertyMetadata(WindowPlacement.Center));

    public WindowPlacement Placement
    {
        get => (WindowPlacement)GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    public static readonly DependencyProperty MarginProperty = DependencyProperty.Register(
        nameof(Margin),
        typeof(Thickness),
        typeof(WindowPlacementAction),
        new PropertyMetadata(new Thickness(0)));

    public Thickness Margin
    {
        get => (Thickness)GetValue(MarginProperty);
        set => SetValue(MarginProperty, value);
    }

    public object Execute(object? sender, object? parameter)
    {
        if (((sender as UIElement)?.XamlRoot is not { } root) || (WindowHelper.GetAppWindow(sender) is not { } window))
        {
            return false;
        }

        var area = DisplayArea.GetFromWindowId(window.Id, DisplayAreaFallback.Nearest).WorkArea;
        return WindowPlacementHelper.UpdatePlacement(window, area, Placement, Margin, root.RasterizationScale);
    }
}
