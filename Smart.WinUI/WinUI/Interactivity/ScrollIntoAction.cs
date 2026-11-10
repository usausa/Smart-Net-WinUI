namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.Xaml.Interactivity;

using Smart.WinUI.Internal;

public sealed class ScrollIntoAction : DependencyObject, IAction
{
    public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
        nameof(Position),
        typeof(ScrollPosition),
        typeof(ScrollIntoAction),
        new PropertyMetadata(ScrollPosition.Last));

    public ScrollPosition Position
    {
        get => (ScrollPosition)GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    public object Execute(object? sender, object? parameter)
    {
        return (sender is Selector selector) && SelectorHelper.ScrollIntoView(selector, Position);
    }
}
