namespace Example.Views;

using System.Globalization;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;

using Smart.WinUI;

internal sealed partial class ExtensionsView
{
    public ExtensionsView()
    {
        InitializeComponent();

        var pressed = 0;
        var pressSubscription = PressButton.RoutedEventAsObservable<PointerEventHandler, PointerRoutedEventArgs>(UIElement.PointerPressedEvent, static h => (_, e) => h(e), true)
            .Subscribe(_ => PressCount.Text = (++pressed).ToString(CultureInfo.InvariantCulture));
        var sliderSubscription = ObservedSlider.PropertyChangedAsObservable<double>(RangeBase.ValueProperty)
            .Subscribe(x => SliderValue.Text = x.ToString(CultureInfo.InvariantCulture));
        Unloaded += (_, _) =>
        {
            pressSubscription.Dispose();
            sliderSubscription.Dispose();
            ViewModel.Dispose();
        };
    }

    public ExtensionsViewModel ViewModel { get; } = new();
}
