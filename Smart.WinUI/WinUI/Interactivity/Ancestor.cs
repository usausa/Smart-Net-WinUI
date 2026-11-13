namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml;

public static class Ancestor
{
    public static readonly DependencyProperty TypeProperty = DependencyProperty.RegisterAttached(
        "Type",
        typeof(Type),
        typeof(Ancestor),
        new PropertyMetadata(null, HandleTypePropertyChanged));

    public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
        "Value",
        typeof(object),
        typeof(Ancestor),
        new PropertyMetadata(null));

    public static Type? GetType(DependencyObject obj) => (Type?)obj.GetValue(TypeProperty);

    public static void SetType(DependencyObject obj, Type? value) => obj.SetValue(TypeProperty, value);

    public static object? GetValue(DependencyObject obj) => obj.GetValue(ValueProperty);

    public static void SetValue(DependencyObject obj, object? value) => obj.SetValue(ValueProperty, value);

    private static void HandleTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FrameworkElement element)
        {
            element.Loaded -= HandleLoaded;
            element.Unloaded -= HandleUnloaded;

            if (e.NewValue is null)
            {
                SetValue(element, null);
                return;
            }

            element.Loaded += HandleLoaded;
            element.Unloaded += HandleUnloaded;
            Update(element);
        }
    }

    private static void HandleLoaded(object sender, RoutedEventArgs e) => Update((FrameworkElement)sender);

    private static void HandleUnloaded(object sender, RoutedEventArgs e)
    {
        var element = (FrameworkElement)sender;
        if (!element.IsLoaded)
        {
            SetValue(element, null);
        }
    }

    private static void Update(FrameworkElement element)
    {
        if (GetType(element) is { } type)
        {
            SetValue(element, element.FindAscendant<DependencyObject, Type>(type, static (x, t) => t.IsInstanceOfType(x)));
        }
    }
}
