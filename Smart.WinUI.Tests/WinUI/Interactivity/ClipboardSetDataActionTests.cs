namespace Smart.WinUI.Interactivity;

public sealed class ClipboardSetDataActionTests
{
    private sealed class DataSource
    {
        public string? Text { get; set; }

        public bool FirstCalled { get; private set; }

        public bool SecondCalled { get; private set; }

        public string? GetText() => Text;

        public string? GetFirst()
        {
            FirstCalled = true;
            return null;
        }

        public string? GetSecond()
        {
            SecondCalled = true;
            return null;
        }

        public string Fail() => throw new InvalidOperationException(Text);
    }

    private sealed class OtherSource
    {
        public bool FirstCalled { get; private set; }

        public string? GetFirst()
        {
            FirstCalled = true;
            return null;
        }
    }

    [Fact]
    public void ReturnsFalseWhenMethodNameIsNull()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ClipboardSetDataAction
            {
                TargetObject = new DataSource { Text = "abc" },
                MethodName = null!,
                Format = "Text"
            };

            // Act
            var result = action.Execute(null, null);

            // Assert
            Assert.False((bool)result);
        });
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

    [Fact]
    public void ResolvesAgainWhenMethodNameChanged()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var source = new DataSource();
            var action = new ClipboardSetDataAction
            {
                TargetObject = source,
                MethodName = nameof(DataSource.GetFirst),
                Format = "Text"
            };
            action.Execute(null, null);

            // Act
            action.MethodName = nameof(DataSource.GetSecond);
            action.Execute(null, null);

            // Assert
            Assert.True(source.FirstCalled);
            Assert.True(source.SecondCalled);
        });
    }

    [Fact]
    public void ResolvesAgainWhenTargetTypeChanged()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var first = new DataSource();
            var second = new OtherSource();
            var action = new ClipboardSetDataAction
            {
                TargetObject = first,
                MethodName = nameof(DataSource.GetFirst),
                Format = "Text"
            };
            action.Execute(null, null);

            // Act
            action.TargetObject = second;
            action.Execute(null, null);

            // Assert
            Assert.True(first.FirstCalled);
            Assert.True(second.FirstCalled);
        });
    }

    [Fact]
    public void ThrowsOriginalExceptionWhenMethodThrows()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ClipboardSetDataAction
            {
                TargetObject = new DataSource { Text = "failed" },
                MethodName = nameof(DataSource.Fail),
                Format = "Text"
            };

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => action.Execute(null, null));

            // Assert
            Assert.Equal("failed", exception.Message);
        });
    }
}
