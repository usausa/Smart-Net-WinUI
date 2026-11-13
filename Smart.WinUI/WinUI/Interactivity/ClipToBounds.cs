namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Hosting;

public static class ClipToBounds
{
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(ClipToBounds),
        new PropertyMetadata(false, HandleIsEnabledPropertyChanged));

    public static bool GetIsEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEnabledProperty);

    public static void SetIsEnabled(DependencyObject obj, bool value) => obj.SetValue(IsEnabledProperty, value);

    private static void HandleIsEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement element)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);
            visual.Clip = (bool)e.NewValue ? visual.Compositor.CreateInsetClip() : null;
        }
    }
}
