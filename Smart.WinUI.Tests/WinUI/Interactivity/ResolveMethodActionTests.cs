namespace Smart.WinUI.Interactivity;

using Smart.Mvvm.Messaging;

public sealed class ResolveMethodActionTests
{
    private sealed class MethodTarget
    {
        private readonly string text = "described";

        private readonly string summary = "summarized";

        public string Describe() => text;

        public string Format(string value) => text + value;

        public string Summarize() => summary;

        public string Fail() => throw new InvalidOperationException(text);
    }

    private sealed class OtherTarget
    {
        private readonly string text = "other";

        public string Describe() => text;
    }

    [Fact]
    public void ResolvesMethodOfTargetObject()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ResolveMethodAction { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Describe) };
            var args = new ResolveEventArgs();

            // Act
            var result = action.Execute(null, args);

            // Assert
            Assert.True((bool)result);
            Assert.Equal("described", args.Result);
        });
    }

    [Fact]
    public void ResolvesMethodOfSenderWhenTargetObjectIsNotSet()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ResolveMethodAction { MethodName = nameof(MethodTarget.Describe) };
            var args = new ResolveEventArgs();

            // Act
            var result = action.Execute(new MethodTarget(), args);

            // Assert
            Assert.True((bool)result);
            Assert.Equal("described", args.Result);
        });
    }

    [Fact]
    public void ReturnsFalseWhenParameterIsNotResolveEventArgs()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ResolveMethodAction { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Describe) };

            // Act
            var result = action.Execute(null, new object());

            // Assert
            Assert.False((bool)result);
        });
    }

    [Fact]
    public void ReturnsFalseWhenMethodHasParameters()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ResolveMethodAction { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Format) };
            var args = new ResolveEventArgs();

            // Act
            var result = action.Execute(null, args);

            // Assert
            Assert.False((bool)result);
            Assert.Null(args.Result);
        });
    }

    [Fact]
    public void ResolvesAgainWhenMethodNameChanged()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ResolveMethodAction { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Describe) };
            var first = new ResolveEventArgs();
            var second = new ResolveEventArgs();
            action.Execute(null, first);

            // Act
            action.MethodName = nameof(MethodTarget.Summarize);
            action.Execute(null, second);

            // Assert
            Assert.Equal("described", first.Result);
            Assert.Equal("summarized", second.Result);
        });
    }

    [Fact]
    public void ResolvesAgainWhenTargetTypeChanged()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ResolveMethodAction { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Describe) };
            var first = new ResolveEventArgs();
            var second = new ResolveEventArgs();
            action.Execute(null, first);

            // Act
            action.TargetObject = new OtherTarget();
            action.Execute(null, second);

            // Assert
            Assert.Equal("described", first.Result);
            Assert.Equal("other", second.Result);
        });
    }

    [Fact]
    public void ThrowsOriginalExceptionWhenMethodThrows()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ResolveMethodAction { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Fail) };
            var args = new ResolveEventArgs();

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => action.Execute(null, args));

            // Assert
            Assert.Equal("described", exception.Message);
        });
    }
}
