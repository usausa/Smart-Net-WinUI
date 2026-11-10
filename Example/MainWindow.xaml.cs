namespace Example;

using Example.Helpers;
using Example.Views;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;

using Smart.WinUI.Interactivity;

using Windows.Graphics;

using WinRT.Interop;

internal sealed partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        var scale = NativeMethods.GetDpiForWindow(WindowNative.GetWindowHandle(this)) / 96d;
        AppWindow.Resize(new SizeInt32((int)(1024 * scale), (int)(768 * scale)));
        WindowPlacementHelper.UpdatePlacement(AppWindow, DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Nearest).WorkArea, WindowPlacement.Center, scale: scale);

        Navigation.SelectedItem = Navigation.MenuItems[0];
    }

    private void OnNavigationSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        var page = args.SelectedItemContainer?.Tag switch
        {
            "Data" => typeof(DataView),
            "Markup" => typeof(MarkupView),
            "Input" => typeof(InputView),
            "Interactivity" => typeof(InteractivityView),
            "Window" => typeof(WindowView),
            "Utilities" => typeof(UtilitiesView),
            "Extensions" => typeof(ExtensionsView),
            _ => null
        };
        if (page is not null)
        {
            ContentFrame.Navigate(page);
        }
    }
}
