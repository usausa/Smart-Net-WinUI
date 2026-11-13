namespace Smart.WinUI.Markup;

using System.Reflection;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;

internal static class MarkupExtensionHelper
{
    private static readonly MethodInfo ProvideValueMethod = typeof(MarkupExtension).GetMethod("ProvideValue", BindingFlags.Instance | BindingFlags.NonPublic, Type.EmptyTypes)!;

    public static object? Provide(MarkupExtension extension) => ProvideValueMethod.Invoke(extension, null);
}

public sealed class BoolExtensionTests
{
    [Fact]
    public void ProvideValueReturnsTrue()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new BoolExtension { Value = true };

            // Act & Assert
            Assert.Equal(true, MarkupExtensionHelper.Provide(ext));
        });
    }

    [Fact]
    public void ProvideValueReturnsFalse()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new BoolExtension { Value = false };

            // Act & Assert
            Assert.Equal(false, MarkupExtensionHelper.Provide(ext));
        });
    }
}

public sealed class DoubleExtensionTests
{
    [Fact]
    public void ProvideValueReturnsDouble()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new DoubleExtension { Value = 3.14 };

            // Act & Assert
            Assert.Equal(3.14, MarkupExtensionHelper.Provide(ext));
        });
    }
}

public sealed class Int32ExtensionTests
{
    [Fact]
    public void ProvideValueReturnsInt()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new Int32Extension { Value = 42 };

            // Act & Assert
            Assert.Equal(42, MarkupExtensionHelper.Provide(ext));
        });
    }
}

public sealed class EnumValuesExtensionTests
{
    private enum SampleEnum
    {
        A,
        B,
        C
    }

    [Fact]
    public void ProvideValueReturnsEnumValues()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new EnumValuesExtension { Type = typeof(SampleEnum) };

            // Act
            var result = MarkupExtensionHelper.Provide(ext);

            // Assert
            var values = Assert.IsAssignableFrom<System.Collections.IEnumerable>(result);
            var list = values.Cast<object>().ToList();
            Assert.Equal([SampleEnum.A, SampleEnum.B, SampleEnum.C], list);
        });
    }
}

public sealed class BoolToExtensionTests
{
    [Fact]
    public void BoolToTextExtensionProvidesBoolToTextConverter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new BoolToTextExtension { True = "yes", False = "no" };

            // Act
            var converter = Assert.IsType<Data.BoolToTextConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.Equal("yes", converter.TrueValue);
            Assert.Equal("no", converter.FalseValue);
        });
    }

    [Fact]
    public void BoolToVisibilityExtensionProvidesConverter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new BoolToVisibilityExtension { True = Visibility.Visible, False = Visibility.Collapsed };

            // Act
            var converter = Assert.IsType<Data.BoolToVisibilityConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.Equal(Visibility.Visible, converter.TrueValue);
            Assert.Equal(Visibility.Collapsed, converter.FalseValue);
        });
    }

    [Fact]
    public void BoolToBrushExtensionDefaultsToTransparent()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new BoolToBrushExtension();

            // Act
            var converter = Assert.IsType<Data.BoolToBrushConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.TrueValue).Color);
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.FalseValue).Color);
        });
    }

    [Fact]
    public void BoolToColorExtensionProvidesConverter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new BoolToColorExtension { True = Colors.Red, False = Colors.Blue };

            // Act
            var converter = Assert.IsType<Data.BoolToColorConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.Equal(Colors.Red, converter.TrueValue);
            Assert.Equal(Colors.Blue, converter.FalseValue);
        });
    }
}

public sealed class ColorBlendExtensionTests
{
    [Fact]
    public void ProvideValueProvidesColorBlendConverter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new ColorBlendExtension { Color = Colors.Red, Raito = 0.5 };

            // Act
            var converter = Assert.IsType<Data.ColorBlendConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.Equal(Colors.Red, converter.Color);
            Assert.Equal(0.5, converter.Raito);
        });
    }
}

public sealed class CompareToExtensionTests
{
    [Fact]
    public void CompareToBoolExtensionProvidesConverter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new CompareToBoolExtension();

            // Act
            var converter = Assert.IsType<Data.CompareToBoolConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.NotNull(converter.Expression);
        });
    }

    [Fact]
    public void CompareToTextExtensionProvidesConverter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new CompareToTextExtension { True = "equal", False = "not equal" };

            // Act
            var converter = Assert.IsType<Data.CompareToTextConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.Equal("equal", converter.TrueValue);
            Assert.Equal("not equal", converter.FalseValue);
        });
    }

    [Fact]
    public void CompareToBrushExtensionDefaultsToTransparent()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new CompareToBrushExtension();

            // Act
            var converter = Assert.IsType<Data.CompareToBrushConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.TrueValue).Color);
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.FalseValue).Color);
        });
    }
}

public sealed class CompareExpressionExtensionTests
{
    [Theory]
    [InlineData(CompareOperator.Equal)]
    [InlineData(CompareOperator.NotEqual)]
    [InlineData(CompareOperator.LessThan)]
    [InlineData(CompareOperator.LessThanOrEqual)]
    [InlineData(CompareOperator.GreaterThan)]
    [InlineData(CompareOperator.GreaterThanOrEqual)]
    public void ProvideValueReturnsExpressionOfSameName(CompareOperator value)
    {
        XamlThread.Run(() =>
        {
            // Arrange
            var ext = new CompareExpressionExtension { Value = value };
            var expected = typeof(Expressions.CompareExpressions).GetProperty(value.ToString())!.GetValue(null);

            // Act & Assert
            Assert.Same(expected, MarkupExtensionHelper.Provide(ext));
        });
    }
}

public sealed class BinaryExpressionExtensionTests
{
    [Theory]
    [InlineData(BinaryOperator.Max)]
    [InlineData(BinaryOperator.Min)]
    [InlineData(BinaryOperator.Add)]
    [InlineData(BinaryOperator.Sub)]
    public void ProvideValueReturnsExpressionOfSameName(BinaryOperator value)
    {
        XamlThread.Run(() =>
        {
            // Arrange
            var ext = new BinaryExpressionExtension { Value = value };
            var expected = typeof(Expressions.BinaryExpressions).GetProperty(value.ToString())!.GetValue(null);

            // Act & Assert
            Assert.Same(expected, MarkupExtensionHelper.Provide(ext));
        });
    }
}

public sealed class ContainsToExtensionTests
{
    [Fact]
    public void ContainsToBoolExtensionProvidesConverter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new ContainsToBoolExtension();

            // Act
            var converter = Assert.IsType<Data.ContainsToBoolConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.True(converter.TrueValue);
            Assert.False(converter.FalseValue);
        });
    }

    [Fact]
    public void ContainsToBoolExtensionInvertedProvidesConverter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new ContainsToBoolExtension { Invert = true };

            // Act
            var converter = Assert.IsType<Data.ContainsToBoolConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.False(converter.TrueValue);
            Assert.True(converter.FalseValue);
        });
    }

    [Fact]
    public void ContainsToBrushExtensionDefaultsToTransparent()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new ContainsToBrushExtension();

            // Act
            var converter = Assert.IsType<Data.ContainsToBrushConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.TrueValue).Color);
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.FalseValue).Color);
        });
    }
}

public sealed class NullToExtensionTests
{
    [Fact]
    public void NullToBoolExtensionProvidesConverter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new NullToBoolExtension();

            // Act
            var converter = Assert.IsType<Data.NullToBoolConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.False(converter.HandleEmptyString);
        });
    }

    [Fact]
    public void NullToTextExtensionProvidesConverter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new NullToTextExtension { Null = "(null)", NonNull = "value" };

            // Act
            var converter = Assert.IsType<Data.NullToTextConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.Equal("(null)", converter.NullValue);
            Assert.Equal("value", converter.NonNullValue);
        });
    }

    [Fact]
    public void NullToBrushExtensionDefaultsToTransparent()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new NullToBrushExtension();

            // Act
            var converter = Assert.IsType<Data.NullToBrushConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.NullValue).Color);
            Assert.Equal(Colors.Transparent, Assert.IsType<SolidColorBrush>(converter.NonNullValue).Color);
        });
    }
}

public sealed class TextReplaceExtensionTests
{
    [Fact]
    public void ProvideValueProvidesTextReplaceConverter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new TextReplaceExtension { Pattern = @"\d+", Replacement = "#" };

            // Act
            var converter = Assert.IsType<Data.TextReplaceConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.Equal(@"\d+", converter.Pattern);
            Assert.Equal("#", converter.Replacement);
        });
    }
}

public sealed class ToBoolExtensionTests
{
    [Fact]
    public void TextToBoolExtensionProvidesConverter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new TextToBoolExtension { True = "yes", False = "no" };

            // Act
            var converter = Assert.IsType<Data.TextToBoolConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.Equal("yes", converter.TrueValue);
            Assert.Equal("no", converter.FalseValue);
        });
    }

    [Fact]
    public void IntToBoolExtensionProvidesConverter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var ext = new IntToBoolExtension { True = 1, False = 0 };

            // Act
            var converter = Assert.IsType<Data.IntToBoolConverter>(MarkupExtensionHelper.Provide(ext));

            // Assert
            Assert.Equal(1, converter.TrueValue);
            Assert.Equal(0, converter.FalseValue);
        });
    }
}
