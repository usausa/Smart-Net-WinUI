namespace Example.Views;

using System.Globalization;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;

using Smart.WinUI;

using Windows.Foundation;

internal sealed partial class UtilitiesView
{
    public UtilitiesView()
    {
        InitializeComponent();
    }

    public UtilitiesViewModel ViewModel { get; } = new();

    public static SolidColorBrush ToBrush(double hue) => new(new HslColor(hue, 1, 0.5, 1).ToColor());

    public static string ToHex(double hue)
    {
        var color = new HslColor(hue, 1, 0.5, 1).ToColor();
        return String.Create(CultureInfo.InvariantCulture, $"#{color.R:X2}{color.G:X2}{color.B:X2}");
    }

    private void OnFindClick(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        TreeResult.Text = String.Create(CultureInfo.InvariantCulture, $"{button.FindAscendant<Border>()?.Tag} {TreeRoot.FindDescendants<Button>().Count()} {ReferenceEquals(button.FindAscendantOrSelf<Button>(), button)}");
    }

    private void OnFlyoutParentClick(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        ParentResult.Text = String.Create(CultureInfo.InvariantCulture, $"flyout {button.FindParent<Border>(static x => x.Tag is not null)?.Tag ?? "none"} {button.IsDescendantOf(this)}");
    }

    private void OnHyperlinkClick(Hyperlink sender, HyperlinkClickEventArgs args)
    {
        ParentResult.Text = String.Create(CultureInfo.InvariantCulture, $"link {sender.FindParent<Border>()?.Tag} {sender.IsDescendantOf(this)}");
    }

    private void OnHitClick(object sender, RoutedEventArgs e)
    {
        var point = FindButton.TransformToVisual(TreeRoot).TransformPoint(new Point(FindButton.ActualWidth / 2, FindButton.ActualHeight / 2));
        HitResult.Text = String.Create(CultureInfo.InvariantCulture, $"{TreeRoot.FindFromPoint<Button>(point)?.Content} {TreeRoot.FindFromPoint<TextBlock>(point)?.Text}");
    }
}
