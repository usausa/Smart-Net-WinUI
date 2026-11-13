namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Xaml.Data;

public sealed class CallMethodActionTests
{
    private interface IArgument;

    private sealed class Argument : IArgument;

    private sealed class MethodTarget
    {
        public bool InterfaceCalled { get; private set; }

        public object? InterfaceReceived { get; private set; }

        public bool ObjectCalled { get; private set; }

        public object? ObjectReceived { get; private set; }

        public void InvokeInterface(IArgument argument)
        {
            InterfaceCalled = true;
            InterfaceReceived = argument;
        }

        public void InvokeObject(object argument)
        {
            ObjectCalled = true;
            ObjectReceived = argument;
        }
    }

    private sealed class NullConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, string? language) => null;

        public object ConvertBack(object? value, Type targetType, object? parameter, string? language) => throw new NotSupportedException();
    }

    [Fact]
    public void ResolvesMethodWhenArgumentIsAssignableToParameterType()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var target = new MethodTarget();
            var argument = new Argument();
            var action = new CallMethodAction
            {
                TargetObject = target,
                MethodName = nameof(MethodTarget.InvokeInterface),
                MethodParameter = argument
            };

            // Act
            action.Execute(null, null);

            // Assert
            Assert.True(target.InterfaceCalled);
            Assert.Same(argument, target.InterfaceReceived);
        });
    }

    [Fact]
    public void PassesEventParameterWhenMethodParameterIsNotSet()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var target = new MethodTarget();
            var action = new CallMethodAction
            {
                TargetObject = target,
                MethodName = nameof(MethodTarget.InvokeObject)
            };

            // Act
            action.Execute(null, "eventArgument");

            // Assert
            Assert.True(target.ObjectCalled);
            Assert.Equal("eventArgument", target.ObjectReceived);
        });
    }

    [Fact]
    public void PassesMethodParameterWhenSet()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var target = new MethodTarget();
            var action = new CallMethodAction
            {
                TargetObject = target,
                MethodName = nameof(MethodTarget.InvokeObject),
                MethodParameter = "methodParameter"
            };

            // Act
            action.Execute(null, "eventArgument");

            // Assert
            Assert.True(target.ObjectCalled);
            Assert.Equal("methodParameter", target.ObjectReceived);
        });
    }

    [Fact]
    public void CallsMethodOfSenderWhenTargetObjectIsNotSet()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var sender = new MethodTarget();
            var action = new CallMethodAction
            {
                MethodName = nameof(MethodTarget.InvokeObject)
            };

            // Act
            var result = action.Execute(sender, "eventArgument");

            // Assert
            Assert.True((bool)result);
            Assert.True(sender.ObjectCalled);
        });
    }

    [Fact]
    public void PassesNullWhenConverterReturnsNull()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var target = new MethodTarget();
            var action = new CallMethodAction
            {
                TargetObject = target,
                MethodName = nameof(MethodTarget.InvokeObject),
                Converter = new NullConverter()
            };

            // Act
            action.Execute(null, "eventArgument");

            // Assert
            Assert.True(target.ObjectCalled);
            Assert.Null(target.ObjectReceived);
        });
    }
}
