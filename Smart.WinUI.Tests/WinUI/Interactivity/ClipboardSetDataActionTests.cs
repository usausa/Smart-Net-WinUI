namespace Smart.WinUI.Interactivity;

public sealed class ClipboardSetDataActionTests
{
    private sealed class DataSource
    {
        public string? Text { get; set; }

        public string? GetText() => Text;
    }

    [Fact]
    public void ReturnsFalseWhenMethodIsNotFound()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ClipboardSetDataAction
            {
                TargetObject = new DataSource { Text = "abc" },
                MethodName = "Unknown",
                Format = "Text"
            };

            // Act
            var result = action.Execute(null, null);

            // Assert
            Assert.False((bool)result);
        });
    }

    [Fact]
    public void ReturnsFalseWhenResultIsNull()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ClipboardSetDataAction
            {
                TargetObject = new DataSource { Text = null },
                MethodName = nameof(DataSource.GetText),
                Format = "Text"
            };

            // Act
            var result = action.Execute(null, null);

            // Assert
            Assert.False((bool)result);
        });
    }
}
