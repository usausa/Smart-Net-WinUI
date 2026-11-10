namespace Smart.WinUI.Data;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

using Windows.UI;

public sealed class BoolToObjectConverterTests
{
    private const string Language = "";

    [Fact]
    public void TrueReturnsConfiguredTrueValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("yes", converter.Convert(true, typeof(string), null, Language));
    }

    [Fact]
    public void FalseReturnsConfiguredFalseValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("no", converter.Convert(false, typeof(string), null, Language));
    }

    [Fact]
    public void NullReturnsFalseValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal("no", converter.Convert(null, typeof(string), null, Language));
    }

    [Fact]
    public void ConvertBackTrueValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal(true, converter.ConvertBack("yes", typeof(bool), null, Language));
    }

    [Fact]
    public void ConvertBackFalseValue()
    {
        // Arrange
        var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

        // Act & Assert
        Assert.Equal(false, converter.ConvertBack("no", typeof(bool), null, Language));
    }

    [Fact]
    public void ConvertBackUnknownReturnsUnsetValue()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var converter = new BoolToTextConverter { TrueValue = "yes", FalseValue = "no" };

            // Act & Assert
            Assert.Equal(DependencyProperty.UnsetValue, converter.ConvertBack("maybe", typeof(bool), null, Language));
        });
    }

    [Fact]
    public void BoolToVisibilityConverter()
    {
        // Arrange
        var converter = new BoolToVisibilityConverter
        {
            TrueValue = Visibility.Visible,
            FalseValue = Visibility.Collapsed
        };

        // Act & Assert
        Assert.Equal(Visibility.Visible, converter.Convert(true, typeof(Visibility), null, Language));
        Assert.Equal(Visibility.Collapsed, converter.Convert(false, typeof(Visibility), null, Language));
    }

    [Fact]
    public void BoolToBrushConverterDefaultsToTransparent()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var converter = new BoolToBrushConverter();

            // Act & Assert
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.Convert(true, typeof(Brush), null, Language)).Color);
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.Convert(false, typeof(Brush), null, Language)).Color);
        });
    }

    [Fact]
    public void BoolToColorConverter()
    {
        // Arrange
        var converter = new BoolToColorConverter
        {
            TrueValue = Colors.Red,
            FalseValue = Colors.Blue
        };

        // Act & Assert
        Assert.Equal(Colors.Red, converter.Convert(true, typeof(Color), null, Language));
        Assert.Equal(Colors.Blue, converter.Convert(false, typeof(Color), null, Language));
    }
}
