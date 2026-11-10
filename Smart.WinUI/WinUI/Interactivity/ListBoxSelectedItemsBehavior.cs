namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.Xaml.Interactivity;

using Smart.WinUI.Internal;

[TypeConstraint(typeof(Selector))]
public sealed class ListBoxSelectedItemsBehavior : Behavior<Selector>
{
    public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
        nameof(SelectedItems),
        typeof(IList<object>),
        typeof(ListBoxSelectedItemsBehavior),
        new PropertyMetadata(null));

#pragma warning disable CA2227
    public IList<object> SelectedItems
    {
        get => (IList<object>)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }
#pragma warning restore CA2227

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.SelectionChanged += AssociatedObjectSelectionChanged;
        UpdateSelectedItems();
    }

    protected override void OnDetaching()
    {
        AssociatedObject.SelectionChanged -= AssociatedObjectSelectionChanged;

        base.OnDetaching();
    }

    private void AssociatedObjectSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateSelectedItems();
    }

    private void UpdateSelectedItems()
    {
        var items = SelectorHelper.GetSelectedItems(AssociatedObject);
        if (items is not null)
        {
            SelectedItems = items;
        }
    }
}
