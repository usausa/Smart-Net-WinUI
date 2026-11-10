namespace Smart.WinUI.Interactivity;

using Smart.Mvvm.Messaging;

public sealed class ResolveMethodActionTests
{
    private sealed class MethodTarget
    {
        private readonly string text = "described";

        public string Describe() => text;

        public string Format(string value) => text + value;
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
}
