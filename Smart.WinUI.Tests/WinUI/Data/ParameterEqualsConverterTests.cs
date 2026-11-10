namespace Smart.WinUI.Data;

using Microsoft.UI.Xaml;

public sealed class ParameterEqualsConverterTests
{
    private const string Language = "";

    [Fact]
    public void EqualReturnsTrue()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act & Assert
        Assert.Equal(true, converter.Convert("a", typeof(bool), "a", Language));
    }

    [Fact]
    public void NotEqualReturnsFalse()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act & Assert
        Assert.Equal(false, converter.Convert("a", typeof(bool), "b", Language));
    }

    [Fact]
    public void ConvertBackTrueReturnsParameter()
    {
        // Arrange
        var converter = new ParameterEqualsConverter();

        // Act & Assert
        Assert.Equal("param", converter.ConvertBack(true, typeof(string), "param", Language));
    }

    [Fact]
    public void ConvertBackFalseReturnsUnsetValue()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var converter = new ParameterEqualsConverter();

            // Act & Assert
            Assert.Equal(DependencyProperty.UnsetValue, converter.ConvertBack(false, typeof(string), "param", Language));
        });
    }
}
