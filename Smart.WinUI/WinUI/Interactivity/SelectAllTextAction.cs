namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

public sealed class SelectAllTextAction : DependencyObject, IAction
{
    public object Execute(object? sender, object? parameter)
    {
        if (sender is not TextBox textBox)
        {
            return false;
        }

        textBox.SelectAll();
        return true;
    }
}
