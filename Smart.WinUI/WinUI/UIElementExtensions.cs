namespace Smart.WinUI;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

using Windows.Foundation;

public static class UIElementExtensions
{
    public static T? FindFromPoint<T>(this UIElement reference, Point point)
        where T : DependencyObject
    {
        var hostPoint = reference.TransformToVisual(null).TransformPoint(point);
        using var enumerator = VisualTreeHelper.FindElementsInHostCoordinates(hostPoint, reference).GetEnumerator();
        return enumerator.MoveNext() ? enumerator.Current?.FindParentOrSelf<T>() : null;
    }
}
