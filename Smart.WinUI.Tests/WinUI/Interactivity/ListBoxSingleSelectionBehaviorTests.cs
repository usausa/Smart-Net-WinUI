namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

public sealed class ListBoxSingleSelectionBehaviorTests
{
    [Fact]
    public void EnablesMultipleSelectionOfListBox()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var listBox = new ListBox();

            // Act
            Interaction.GetBehaviors(listBox).Add(new ListBoxSingleSelectionBehavior());

            // Assert
            Assert.Equal(SelectionMode.Multiple, listBox.SelectionMode);
        });
    }

    [Fact]
    public void EnablesMultipleSelectionOfListViewWithoutCheckBox()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var listView = new ListView();

            // Act
            Interaction.GetBehaviors(listView).Add(new ListBoxSingleSelectionBehavior());

            // Assert
            Assert.Equal(ListViewSelectionMode.Multiple, listView.SelectionMode);
            Assert.False(listView.IsMultiSelectCheckBoxEnabled);
        });
    }

    [Fact]
    public void KeepsLastSelectedItem()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var listBox = new ListBox { Items = { "a", "b", "c" } };
            var behavior = new ListBoxSingleSelectionBehavior();
            Interaction.GetBehaviors(listBox).Add(behavior);

            // Act
            listBox.SelectedItems.Add("a");
            listBox.SelectedItems.Add("c");

            // Assert
            Assert.Equal(["c"], listBox.SelectedItems);
            Assert.Equal("c", behavior.SelectedItem);
        });
    }

    [Fact]
    public void ClearsSelectedItemWhenDeselected()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var listView = new ListView { Items = { "a", "b" } };
            var behavior = new ListBoxSingleSelectionBehavior();
            Interaction.GetBehaviors(listView).Add(behavior);
            listView.SelectedItems.Add("b");

            // Act
            listView.SelectedItems.Remove("b");

            // Assert
            Assert.Null(behavior.SelectedItem);
        });
    }
}
