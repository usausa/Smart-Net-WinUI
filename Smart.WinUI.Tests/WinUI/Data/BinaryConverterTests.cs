namespace Smart.WinUI.Data;

public sealed class BinaryConverterTests
{
    private const string Language = "";

    [Fact]
    public void EvalDelegatesToExpression()
    {
        // Arrange
        var called = false;
        object? leftReceived = null;
        object? rightReceived = null;
        var expr = new DelegateExpression((l, r) =>
        {
            called = true;
            leftReceived = l;
            rightReceived = r;
            return "result";
        });
        var converter = new BinaryConverter { Expression = expr };

        // Act
        var result = converter.Convert("left", typeof(object), "right", Language);

        // Assert
        Assert.True(called);
        Assert.Equal("left", leftReceived);
        Assert.Equal("right", rightReceived);
        Assert.Equal("result", result);
    }

    [Fact]
    public void ConvertBackThrows()
    {
        // Arrange
        var converter = new BinaryConverter { Expression = new DelegateExpression((_, _) => null) };

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack("test", typeof(object), null, Language));
    }

    private sealed class DelegateExpression : Expressions.IBinaryExpression
    {
        private readonly Func<object?, object?, object?> func;

        public DelegateExpression(Func<object?, object?, object?> func)
        {
            this.func = func;
        }

        public object? Eval(object? left, object? right) => func(left, right);
    }
}
