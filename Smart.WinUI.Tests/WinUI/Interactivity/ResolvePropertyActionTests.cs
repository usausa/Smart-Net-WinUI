namespace Smart.WinUI.Interactivity;

using System.Reflection;

using Microsoft.UI.Xaml.Controls;

using Smart.Mvvm.Messaging;

public sealed class ResolvePropertyActionTests
{
    private sealed class PropertyTarget
    {
        public string Name { get; } = "target";

        public string Broken => throw new InvalidOperationException(Name);
    }

    [Fact]
    public void ResolvesPropertyOfTargetObject()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ResolvePropertyAction { TargetObject = new PropertyTarget(), PropertyName = nameof(PropertyTarget.Name) };
            var args = new ResolveEventArgs();

            // Act
            var result = action.Execute(null, args);

            // Assert
            Assert.True((bool)result);
            Assert.Equal("target", args.Result);
        });
    }

    [Fact]
    public void ResolvesPropertyOfSenderWhenTargetObjectIsNotSet()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ResolvePropertyAction { PropertyName = nameof(TextBox.Text) };
            var args = new ResolveEventArgs();

            // Act
            action.Execute(new TextBox { Text = "abc" }, args);

            // Assert
            Assert.Equal("abc", args.Result);
        });
    }

    [Fact]
    public void ReturnsFalseWhenParameterIsNotResolveEventArgs()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ResolvePropertyAction { TargetObject = new PropertyTarget(), PropertyName = nameof(PropertyTarget.Name) };

            // Act
            var result = action.Execute(null, new object());

            // Assert
            Assert.False((bool)result);
        });
    }

    [Fact]
    public void ReturnsFalseWhenPropertyNameIsNull()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ResolvePropertyAction { TargetObject = new PropertyTarget(), PropertyName = null! };
            var args = new ResolveEventArgs();

            // Act
            var result = action.Execute(null, args);

            // Assert
            Assert.False((bool)result);
            Assert.Null(args.Result);
        });
    }

    [Fact]
    public void ThrowsWrappedExceptionWhenGetterThrows()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var action = new ResolvePropertyAction { TargetObject = new PropertyTarget(), PropertyName = nameof(PropertyTarget.Broken) };
            var args = new ResolveEventArgs();

            // Act
            var exception = Assert.Throws<TargetInvocationException>(() => action.Execute(null, args));

            // Assert
            var inner = Assert.IsType<InvalidOperationException>(exception.InnerException);
            Assert.Equal("target", inner.Message);
        });
    }
}
