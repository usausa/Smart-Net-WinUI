namespace Smart.WinUI.Internal;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

using Smart.WinUI.Interactivity;

internal static class SelectorHelper
{
    public static IList<object>? GetSelectedItems(Selector selector) => selector switch
    {
        ListBox listBox => listBox.SelectedItems,
        ListViewBase listView => listView.SelectedItems,
        _ => null
    };

    public static void EnableMultipleSelection(Selector selector)
    {
        if (selector is ListBox listBox)
        {
            listBox.SelectionMode = SelectionMode.Multiple;
        }
        else if (selector is ListViewBase listView)
        {
            listView.SelectionMode = ListViewSelectionMode.Multiple;
            listView.IsMultiSelectCheckBoxEnabled = false;
        }
    }

    public static bool ScrollIntoView(Selector selector, ScrollPosition position)
    {
        var count = selector.Items.Count;
        if (count == 0)
        {
            return false;
        }

        var item = position == ScrollPosition.First ? selector.Items[0] : selector.Items[count - 1];
        if (item is null)
        {
            return false;
        }

        if (selector is ListBox listBox)
        {
            listBox.ScrollIntoView(item);
            return true;
        }

        if (selector is ListViewBase listView)
        {
            listView.ScrollIntoView(item);
            return true;
        }

        return false;
    }
}
