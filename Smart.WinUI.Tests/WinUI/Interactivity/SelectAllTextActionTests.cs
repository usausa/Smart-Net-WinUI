namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml.Controls;

public sealed class SelectAllTextActionTests
{
    [Fact]
    public void SelectsAllTextOfTextBox()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var textBox = new TextBox { Text = "abc" };
            var action = new SelectAllTextAction();

            // Act
            var result = action.Execute(textBox, null);

            // Assert
            Assert.True((bool)result);
            Assert.Equal("abc", textBox.SelectedText);
        });
    }

    [Fact]
    public void ReturnsFalseWhenSenderIsNotTextBox()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var passwordBox = new PasswordBox { Password = "abc" };
            var action = new SelectAllTextAction();

            // Act
            var result = action.Execute(passwordBox, null);

            // Assert
            Assert.False((bool)result);
        });
    }
}
