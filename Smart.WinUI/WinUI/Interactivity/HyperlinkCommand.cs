namespace Smart.WinUI.Interactivity;

using System.Windows.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Documents;

public static class HyperlinkCommand
{
    public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
        "Command",
        typeof(ICommand),
        typeof(HyperlinkCommand),
        new PropertyMetadata(null, HandleCommandPropertyChanged));

    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
        "CommandParameter",
        typeof(object),
        typeof(HyperlinkCommand),
        new PropertyMetadata(null));

    public static ICommand? GetCommand(DependencyObject obj) => (ICommand?)obj.GetValue(CommandProperty);

    public static void SetCommand(DependencyObject obj, ICommand? value) => obj.SetValue(CommandProperty, value);

    public static object? GetCommandParameter(DependencyObject obj) => obj.GetValue(CommandParameterProperty);

    public static void SetCommandParameter(DependencyObject obj, object? value) => obj.SetValue(CommandParameterProperty, value);

    private static void HandleCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Hyperlink hyperlink)
        {
            hyperlink.Click -= HandleClick;

            if (e.NewValue is not null)
            {
                hyperlink.Click += HandleClick;
            }
        }
    }

    private static void HandleClick(Hyperlink sender, HyperlinkClickEventArgs args)
    {
        var command = GetCommand(sender);
        var parameter = GetCommandParameter(sender);
        if ((command is not null) && command.CanExecute(parameter))
        {
            command.Execute(parameter);
        }
    }
}
