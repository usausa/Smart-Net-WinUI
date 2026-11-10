namespace Smart.WinUI.Interactivity;

using System.ComponentModel;

public sealed class CancelEventActionTests
{
    [Fact]
    public void SetsCancelOfCancelEventArgs()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var args = new CancelEventArgs();
            var action = new CancelEventAction { Cancel = true };

            // Act
            var result = action.Execute(null, args);

            // Assert
            Assert.True((bool)result);
            Assert.True(args.Cancel);
        });
    }

    [Fact]
    public void ReturnsFalseForOtherParameter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new CancelEventAction { Cancel = true };

            // Act
            var result = action.Execute(null, EventArgs.Empty);

            // Assert
            Assert.False((bool)result);
        });
    }
}
