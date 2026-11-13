namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml;

public static class ActualSize
{
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(ActualSize),
        new PropertyMetadata(false, HandleIsEnabledPropertyChanged));

    public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached(
        "Width",
        typeof(double),
        typeof(ActualSize),
        new PropertyMetadata(Double.NaN));

    public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached(
        "Height",
        typeof(double),
        typeof(ActualSize),
        new PropertyMetadata(Double.NaN));

    public static bool GetIsEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEnabledProperty);

    public static void SetIsEnabled(DependencyObject obj, bool value) => obj.SetValue(IsEnabledProperty, value);

    public static double GetWidth(DependencyObject obj) => (double)obj.GetValue(WidthProperty);

    public static void SetWidth(DependencyObject obj, double value) => obj.SetValue(WidthProperty, value);

    public static double GetHeight(DependencyObject obj) => (double)obj.GetValue(HeightProperty);

    public static void SetHeight(DependencyObject obj, double value) => obj.SetValue(HeightProperty, value);

    private static void HandleIsEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FrameworkElement element)
        {
            element.SizeChanged -= HandleSizeChanged;

            if ((bool)e.NewValue)
            {
                element.SizeChanged += HandleSizeChanged;
                Update(element);
            }
        }
    }

    private static void HandleSizeChanged(object sender, SizeChangedEventArgs e) => Update((FrameworkElement)sender);

    private static void Update(FrameworkElement element)
    {
        SetWidth(element, element.ActualWidth);
        SetHeight(element, element.ActualHeight);
    }
}
