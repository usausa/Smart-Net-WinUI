namespace Smart.WinUI.Data;

using Microsoft.UI.Xaml;

using Smart.Converter;

public sealed class ObjectConvertConverterTests
{
    private const string Language = "";

    [Fact]
    public void ConvertReturnsConvertedValue()
    {
        // Arrange
        var converter = new ObjectConvertConverter();

        // Act
        var result = converter.Convert(12, typeof(string), null, Language);

        // Assert
        Assert.Equal("12", result);
    }

    [Fact]
    public void ConvertBackReturnsParsedValue()
    {
        // Arrange
        var converter = new ObjectConvertConverter();

        // Act
        var result = converter.ConvertBack("12", typeof(int), null, Language);

        // Assert
        Assert.Equal(12, result);
    }

    [Fact]
    public void ConvertBackInvalidReturnsUnsetValue()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var converter = new ObjectConvertConverter();

            // Act
            var result = converter.ConvertBack("abc", typeof(int), null, Language);

            // Assert
            Assert.Equal(DependencyProperty.UnsetValue, result);
        });
    }

    [Fact]
    public void ConvertBackEmptyToValueTypeReturnsUnsetValue()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var converter = new ObjectConvertConverter();

            // Act
            var result = converter.ConvertBack(string.Empty, typeof(int), null, Language);

            // Assert
            Assert.Equal(DependencyProperty.UnsetValue, result);
        });
    }

    [Fact]
    public void ConvertBackEmptyToNullableReturnsNull()
    {
        // Arrange
        var converter = new ObjectConvertConverter();

        // Act
        var result = converter.ConvertBack(string.Empty, typeof(int?), null, Language);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertBackWithoutConverterThrows()
    {
        // Arrange
        var converter = new ObjectConvertConverter();

        // Act & Assert
        Assert.Throws<ObjectConverterException>(() => converter.ConvertBack(new object(), typeof(int), null, Language));
    }
}
