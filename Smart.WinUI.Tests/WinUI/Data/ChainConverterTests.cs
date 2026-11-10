namespace Smart.WinUI.Data;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

public sealed class ChainConverterTests
{
    private const string Language = "";

    private sealed class UnsetValueConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, string? language) => DependencyProperty.UnsetValue;

        public object ConvertBack(object? value, Type targetType, object? parameter, string? language) => DependencyProperty.UnsetValue;
    }

    [Fact]
    public void EmptyChainReturnsOriginalValue()
    {
        // Arrange
        var converter = new ChainConverter();

        // Act & Assert
        Assert.Equal("hello", converter.Convert("hello", typeof(string), null, Language));
    }

    [Fact]
    public void ChainAppliesConvertersInOrder()
    {
        // Arrange
        var converter = new ChainConverter();
        converter.Converters.Add(new ToUpperConverter());
        converter.Converters.Add(new ReverseConverter());

        // Act
        var result = converter.Convert("hello", typeof(string), null, Language);

        // Assert
        Assert.Equal("HELLO", result);
    }

    [Fact]
    public void ConvertStopsAtUnsetValue()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var converter = new ChainConverter();
            converter.Converters.Add(new UnsetValueConverter());
            converter.Converters.Add(new NullToTextConverter { NullValue = "null", NonNullValue = "set" });

            // Act
            var result = converter.Convert("hello", typeof(string), null, Language);

            // Assert
            Assert.Equal(DependencyProperty.UnsetValue, result);
        });
    }

    [Fact]
    public void ConvertBackStopsAtUnsetValue()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var converter = new ChainConverter();
            converter.Converters.Add(new ObjectConvertConverter());
            converter.Converters.Add(new BoolToTextConverter { TrueValue = "ON", FalseValue = "OFF" });

            // Act
            var result = converter.ConvertBack("unknown", typeof(int), null, Language);

            // Assert
            Assert.Equal(DependencyProperty.UnsetValue, result);
        });
    }
}
