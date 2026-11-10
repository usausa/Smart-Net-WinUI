namespace Smart.WinUI.Interactivity;

using System.ComponentModel;
using System.Reflection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.Xaml.Interactivity;

public sealed class MaskedTextBehaviorTests
{
    private sealed class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string? Value
        {
            get;
            set
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }
    }

    private static readonly Func<string, string> NarrowDigits = static x => String.Concat(x.Select(static c => c is >= '０' and <= '９' ? (char)(c - '０' + '0') : c));

    private static readonly MethodInfo SelectionChangedMethod =
        typeof(MaskedTextBehavior).GetMethod("OnSelectionChanged", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly MethodInfo CompositionStartedMethod =
        typeof(MaskedTextBehavior).GetMethod("OnTextCompositionStarted", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly MethodInfo CompositionEndedMethod =
        typeof(MaskedTextBehavior).GetMethod("OnTextCompositionEnded", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static TextBox Attach(MaskedTextBehavior behavior)
    {
        var textBox = new TextBox();
        Interaction.GetBehaviors(textBox).Add(behavior);
        return textBox;
    }

    private static void Select(MaskedTextBehavior behavior, TextBox textBox, int start, int length)
    {
        textBox.Select(start, length);
        SelectionChangedMethod.Invoke(behavior, [textBox, null]);
    }

    private static void Edit(TextBox textBox, int start, int length, string text)
    {
        var current = textBox.Text;
        textBox.Text = current[..start] + text + current[(start + length)..];
    }

    private static void Type(TextBox textBox, string text)
    {
        foreach (var c in text)
        {
            Edit(textBox, textBox.SelectionStart, textBox.SelectionLength, c.ToString());
        }
    }

    private static void Paste(TextBox textBox, string text)
    {
        Edit(textBox, textBox.SelectionStart, textBox.SelectionLength, text);
    }

    private static void Backspace(TextBox textBox)
    {
        var start = textBox.SelectionStart;
        var length = textBox.SelectionLength;
        if (length > 0)
        {
            Edit(textBox, start, length, string.Empty);
        }
        else
        {
            Edit(textBox, start - 1, 1, string.Empty);
        }
    }

    private static void Delete(TextBox textBox)
    {
        var length = textBox.SelectionLength;
        Edit(textBox, textBox.SelectionStart, length > 0 ? length : 1, string.Empty);
    }

    [Fact]
    public void ShowsPromptWhenAttached()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var textBox = Attach(new MaskedTextBehavior { Mask = "000-0000" });

            // Assert
            Assert.Equal("___-____", textBox.Text);
        });
    }

    [Fact]
    public void ChangesPromptChar()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "12" };
            var textBox = Attach(behavior);

            // Act
            behavior.PromptChar = '*';

            // Assert
            Assert.Equal("12*-****", textBox.Text);
        });
    }

    [Fact]
    public void ReappliesValueWhenMaskChanged()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" };
            var textBox = Attach(behavior);

            // Act
            behavior.Mask = "000 0000";

            // Assert
            Assert.Equal("123 4567", textBox.Text);
        });
    }

    [Fact]
    public void FillsPositionsAndSkipsLiteral()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000" };
            var textBox = Attach(behavior);

            // Act
            Type(textBox, "1234");

            // Assert
            Assert.Equal("123-4___", textBox.Text);
            Assert.Equal(5, textBox.SelectionStart);
            Assert.Equal("123-4", behavior.Value);
        });
    }

    [Fact]
    public void IgnoresCharacterNotMatchingMask()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000" };
            var textBox = Attach(behavior);

            // Act
            Type(textBox, "1a");

            // Assert
            Assert.Equal("1__-____", textBox.Text);
            Assert.Equal(1, textBox.SelectionStart);
        });
    }

    [Fact]
    public void FillsFromFirstEmptyPositionWhenCaretIsAfterInput()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "1" };
            var textBox = Attach(behavior);
            Select(behavior, textBox, 8, 0);

            // Act
            Type(textBox, "2");

            // Assert
            Assert.Equal("12_-____", textBox.Text);
        });
    }

    [Fact]
    public void ReplacesSelection()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" };
            var textBox = Attach(behavior);
            Select(behavior, textBox, 4, 2);

            // Act
            Type(textBox, "9");

            // Assert
            Assert.Equal("123-967_", textBox.Text);
            Assert.Equal(5, textBox.SelectionStart);
        });
    }

    [Fact]
    public void IgnoresTypingWhenFull()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" };
            var textBox = Attach(behavior);
            Select(behavior, textBox, 1, 0);

            // Act
            Type(textBox, "9");

            // Assert
            Assert.Equal("123-4567", textBox.Text);
        });
    }

    [Fact]
    public void BackspaceRemovesPreviousCharacterAndShiftsRest()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" };
            var textBox = Attach(behavior);
            Select(behavior, textBox, 5, 0);

            // Act
            Backspace(textBox);

            // Assert
            Assert.Equal("123-567_", textBox.Text);
            Assert.Equal(4, textBox.SelectionStart);
        });
    }

    [Fact]
    public void BackspaceSkipsLiteral()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" };
            var textBox = Attach(behavior);
            Select(behavior, textBox, 4, 0);

            // Act
            Backspace(textBox);

            // Assert
            Assert.Equal("124-567_", textBox.Text);
            Assert.Equal(2, textBox.SelectionStart);
        });
    }

    [Fact]
    public void DeleteRemovesNextCharacter()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" };
            var textBox = Attach(behavior);
            Select(behavior, textBox, 3, 0);

            // Act
            Delete(textBox);

            // Assert
            Assert.Equal("123-567_", textBox.Text);
            Assert.Equal(3, textBox.SelectionStart);
        });
    }

    [Fact]
    public void DeleteRemovesSelection()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" };
            var textBox = Attach(behavior);
            Select(behavior, textBox, 2, 3);

            // Act
            Delete(textBox);

            // Assert
            Assert.Equal("125-67__", textBox.Text);
            Assert.Equal(2, textBox.SelectionStart);
        });
    }

    [Fact]
    public void ValueIsEmptyWhenAllRemoved()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000" };
            var textBox = Attach(behavior);
            Type(textBox, "1");

            // Act
            Backspace(textBox);

            // Assert
            Assert.Equal("___-____", textBox.Text);
            Assert.Equal(string.Empty, behavior.Value);
        });
    }

    [Fact]
    public void PastesTextWithLiteral()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000" };
            var textBox = Attach(behavior);

            // Act
            Paste(textBox, "123-4567");

            // Assert
            Assert.Equal("123-4567", textBox.Text);
            Assert.Equal("123-4567", behavior.Value);
        });
    }

    [Fact]
    public void IgnoresPasteNotMatchingMask()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "12" };
            var textBox = Attach(behavior);

            // Act
            Paste(textBox, "abc");

            // Assert
            Assert.Equal("12_-____", textBox.Text);
        });
    }

    [Fact]
    public void UpdatesTextFromSource()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var model = new Model();
            var behavior = new MaskedTextBehavior { Mask = "000-0000" };
            BindingOperations.SetBinding(behavior, MaskedTextBehavior.ValueProperty, new Binding { Path = new PropertyPath(nameof(Model.Value)), Source = model });
            var textBox = Attach(behavior);

            // Act
            model.Value = "1234567";

            // Assert
            Assert.Equal("123-4567", textBox.Text);
        });
    }

    [Fact]
    public void UpdatesSourceWithoutLiteral()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var model = new Model();
            var behavior = new MaskedTextBehavior { Mask = "000-0000", IncludeLiterals = false };
            BindingOperations.SetBinding(behavior, MaskedTextBehavior.ValueProperty, new Binding { Path = new PropertyPath(nameof(Model.Value)), Source = model, Mode = BindingMode.TwoWay });
            var textBox = Attach(behavior);

            // Act
            Type(textBox, "1234567");

            // Assert
            Assert.Equal("1234567", model.Value);
            Assert.IsType<BindingExpression>(behavior.ReadLocalValue(MaskedTextBehavior.ValueProperty), false);
        });
    }

    [Fact]
    public void ClearsWhenValueNotMatchingMask()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" };
            var textBox = Attach(behavior);

            // Act
            behavior.Value = "abc";

            // Assert
            Assert.Equal("___-____", textBox.Text);
        });
    }

    [Fact]
    public void AcceptsFullWidthDigitsWithoutNormalizer()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000" };
            var textBox = Attach(behavior);

            // Act
            Paste(textBox, "１２３４５６７");

            // Assert
            Assert.Equal("１２３-４５６７", textBox.Text);
        });
    }

    [Fact]
    public void NormalizesTypedAndPastedText()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Normalizer = NarrowDigits };
            var textBox = Attach(behavior);

            // Act
            Type(textBox, "１２３");
            Paste(textBox, "４５６７");

            // Assert
            Assert.Equal("123-4567", textBox.Text);
            Assert.Equal("123-4567", behavior.Value);
        });
    }

    [Fact]
    public void NormalizesValueFromSource()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Normalizer = NarrowDigits };
            var textBox = Attach(behavior);

            // Act
            behavior.Value = "１２３４５６７";

            // Assert
            Assert.Equal("123-4567", textBox.Text);
        });
    }

    [Fact]
    public void NormalizesTextSetDirectly()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Normalizer = NarrowDigits };
            var textBox = Attach(behavior);

            // Act
            textBox.Text = "１２３４５６７";

            // Assert
            Assert.Equal("123-4567", textBox.Text);
        });
    }

    [Fact]
    public void ReappliesValueWhenNormalizerChanged()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "１２３４５６７" };
            var textBox = Attach(behavior);

            // Act
            behavior.Normalizer = NarrowDigits;

            // Assert
            Assert.Equal("123-4567", textBox.Text);
        });
    }

    [Fact]
    public void FormatsTextSetDirectly()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000" };
            var textBox = Attach(behavior);

            // Act
            textBox.Text = "1234567";

            // Assert
            Assert.Equal("123-4567", textBox.Text);
            Assert.Equal("123-4567", behavior.Value);
        });
    }

    [Fact]
    public void RevertsTextNotMatchingMask()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", Value = "1234567" };
            var textBox = Attach(behavior);

            // Act
            textBox.Text = "abc";

            // Assert
            Assert.Equal("123-4567", textBox.Text);
        });
    }

    [Fact]
    public void AppliesTextWhenCompositionEnded()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000", IncludeLiterals = false, Normalizer = NarrowDigits };
            var textBox = Attach(behavior);
            CompositionStartedMethod.Invoke(behavior, [textBox, null]);
            Edit(textBox, 0, 0, "１２");

            // Act
            var composing = textBox.Text;
            CompositionEndedMethod.Invoke(behavior, [textBox, null]);

            // Assert
            Assert.Equal("１２___-____", composing);
            Assert.Equal("12_-____", textBox.Text);
            Assert.Equal("12", behavior.Value);
        });
    }

    [Fact]
    public void DoesNothingWithoutMask()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior();
            var textBox = Attach(behavior);

            // Act
            textBox.Text = "a";

            // Assert
            Assert.Equal("a", textBox.Text);
        });
    }

    [Fact]
    public void StopsMaskingWhenDetached()
    {
        XamlThread.Run(static () =>
        {
            // Arrange
            var behavior = new MaskedTextBehavior { Mask = "000-0000" };
            var textBox = Attach(behavior);

            // Act
            Interaction.GetBehaviors(textBox).Remove(behavior);
            textBox.Text = "a";

            // Assert
            Assert.Equal("a", textBox.Text);
        });
    }
}
