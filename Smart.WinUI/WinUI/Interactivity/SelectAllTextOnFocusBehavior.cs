namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

[TypeConstraint(typeof(TextBox))]
public sealed class SelectAllTextOnFocusBehavior : Behavior<TextBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.GotFocus += SelectAllText;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.GotFocus -= SelectAllText;
    }

    private void SelectAllText(object sender, RoutedEventArgs e)
    {
        AssociatedObject.SelectAll();
    }
}
