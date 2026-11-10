namespace Smart.WinUI.Interactivity;

using System.Runtime.CompilerServices;

using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

public sealed class ChangeCursorAction : DependencyObject, IAction
{
    public static readonly DependencyProperty CursorProperty = DependencyProperty.Register(
        nameof(Cursor),
        typeof(InputSystemCursorShape?),
        typeof(ChangeCursorAction),
        new PropertyMetadata(null));

    public InputSystemCursorShape? Cursor
    {
        get => (InputSystemCursorShape?)GetValue(CursorProperty);
        set => SetValue(CursorProperty, value);
    }

    public object Execute(object? sender, object? parameter)
    {
        if (sender is not UIElement element)
        {
            return false;
        }

#pragma warning disable CA2000
        SetProtectedCursor(element, Cursor is { } shape ? InputSystemCursor.Create(shape) : null);
#pragma warning restore CA2000
        return true;
    }

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "set_ProtectedCursor")]
    private static extern void SetProtectedCursor(UIElement element, InputCursor? value);
}
