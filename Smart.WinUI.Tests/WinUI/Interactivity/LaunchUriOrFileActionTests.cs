namespace Smart.WinUI.Interactivity;

public sealed class LaunchUriOrFileActionTests
{
    [Fact]
    public void ReturnsFalseWithoutPath()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new LaunchUriOrFileAction();

            // Act
            var result = action.Execute(null, null);

            // Assert
            Assert.False((bool)result);
        });
    }

    [Fact]
    public void ReturnsFalseWithEmptyPath()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new LaunchUriOrFileAction { Path = string.Empty };

            // Act
            var result = action.Execute(null, null);

            // Assert
            Assert.False((bool)result);
        });
    }
}
