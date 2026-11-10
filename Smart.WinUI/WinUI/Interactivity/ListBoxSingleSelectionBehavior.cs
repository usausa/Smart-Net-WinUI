namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.Xaml.Interactivity;

using Smart.WinUI.Internal;

public sealed class ListBoxSingleSelectionBehavior : Behavior<Selector>
{
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        nameof(SelectedItem),
        typeof(object),
        typeof(ListBoxSingleSelectionBehavior),
        new PropertyMetadata(null));

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        SelectorHelper.EnableMultipleSelection(AssociatedObject);
        AssociatedObject.SelectionChanged += OnSelectionChanged;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.SelectionChanged -= OnSelectionChanged;

        base.OnDetaching();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SelectorHelper.GetSelectedItems((Selector)sender) is { } selectedItems)
        {
            if (e.AddedItems.Count > 0)
            {
                var target = e.AddedItems[0];
                foreach (var item in selectedItems.ToArray())
                {
                    if (item != target)
                    {
                        selectedItems.Remove(item);
                    }
                }
            }

            SelectedItem = selectedItems.Count > 0 ? selectedItems[0] : null;
        }
    }
}
