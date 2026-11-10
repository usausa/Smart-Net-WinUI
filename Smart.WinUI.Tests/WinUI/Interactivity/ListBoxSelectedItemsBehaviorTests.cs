namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

public sealed class ListBoxSelectedItemsBehaviorTests
{
    [Fact]
    public void SetsSelectedItemsOfListBoxWhenAttached()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var listBox = new ListBox { SelectionMode = SelectionMode.Multiple, Items = { "a", "b", "c" } };
            listBox.SelectedItems.Add("b");
            var behavior = new ListBoxSelectedItemsBehavior();

            // Act
            Interaction.GetBehaviors(listBox).Add(behavior);

            // Assert
            Assert.Equal(["b"], behavior.SelectedItems);
        });
    }

    [Fact]
    public void UpdatesSelectedItemsWhenSelectionChanged()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var listView = new ListView { SelectionMode = ListViewSelectionMode.Multiple, Items = { "a", "b", "c" } };
            var behavior = new ListBoxSelectedItemsBehavior();
            Interaction.GetBehaviors(listView).Add(behavior);

            // Act
            listView.SelectedItems.Add("a");
            listView.SelectedItems.Add("c");

            // Assert
            Assert.Equal(["a", "c"], behavior.SelectedItems);
        });
    }

    [Fact]
    public void DoesNothingForOtherSelector()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var comboBox = new ComboBox { Items = { "a" }, SelectedIndex = 0 };
            var behavior = new ListBoxSelectedItemsBehavior();

            // Act
            Interaction.GetBehaviors(comboBox).Add(behavior);

            // Assert
            Assert.Null(behavior.SelectedItems);
        });
    }
}
