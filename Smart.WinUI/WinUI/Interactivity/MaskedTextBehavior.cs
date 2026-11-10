namespace Smart.WinUI.Interactivity;

using System.ComponentModel;
using System.Globalization;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

[TypeConstraint(typeof(TextBox))]
public sealed class MaskedTextBehavior : Behavior<TextBox>
{
    public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(
        nameof(Mask),
        typeof(string),
        typeof(MaskedTextBehavior),
        new PropertyMetadata(null, HandleProviderPropertyChanged));

    public static readonly DependencyProperty PromptCharProperty = DependencyProperty.Register(
        nameof(PromptChar),
        typeof(char),
        typeof(MaskedTextBehavior),
        new PropertyMetadata('_', HandlePromptCharPropertyChanged));

    public static readonly DependencyProperty IncludeLiteralsProperty = DependencyProperty.Register(
        nameof(IncludeLiterals),
        typeof(bool),
        typeof(MaskedTextBehavior),
        new PropertyMetadata(true));

    public static readonly DependencyProperty NormalizerProperty = DependencyProperty.Register(
        nameof(Normalizer),
        typeof(Func<string, string>),
        typeof(MaskedTextBehavior),
        new PropertyMetadata(null, HandleProviderPropertyChanged));

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value),
        typeof(string),
        typeof(MaskedTextBehavior),
        new PropertyMetadata(null, HandleValuePropertyChanged));

    public string? Mask
    {
        get => (string?)GetValue(MaskProperty);
        set => SetValue(MaskProperty, value);
    }

    public char PromptChar
    {
        get => (char)GetValue(PromptCharProperty);
        set => SetValue(PromptCharProperty, value);
    }

    public bool IncludeLiterals
    {
        get => (bool)GetValue(IncludeLiteralsProperty);
        set => SetValue(IncludeLiteralsProperty, value);
    }

    public Func<string, string>? Normalizer
    {
        get => (Func<string, string>?)GetValue(NormalizerProperty);
        set => SetValue(NormalizerProperty, value);
    }

#pragma warning disable CA1721
    public string? Value
    {
        get => (string?)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
#pragma warning restore CA1721

    private MaskedTextProvider? provider;

    private bool updating;

    private bool composing;

    private int selectionStart;

    private int selectionLength;

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.TextChanging += OnTextChanging;
        AssociatedObject.SelectionChanged += OnSelectionChanged;
        AssociatedObject.TextCompositionStarted += OnTextCompositionStarted;
        AssociatedObject.TextCompositionEnded += OnTextCompositionEnded;

        UpdateProvider();
    }

    protected override void OnDetaching()
    {
        AssociatedObject.TextChanging -= OnTextChanging;
        AssociatedObject.SelectionChanged -= OnSelectionChanged;
        AssociatedObject.TextCompositionStarted -= OnTextCompositionStarted;
        AssociatedObject.TextCompositionEnded -= OnTextCompositionEnded;

        provider = null;
        composing = false;

        base.OnDetaching();
    }

    private static void HandleProviderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (MaskedTextBehavior)d;
        if (behavior.AssociatedObject is not null)
        {
            behavior.UpdateProvider();
        }
    }

    private static void HandlePromptCharPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (MaskedTextBehavior)d;
        if (behavior.provider is not null)
        {
            behavior.provider.PromptChar = (char)e.NewValue;
            behavior.Render(behavior.provider, behavior.AssociatedObject.SelectionStart, 0);
        }
    }

    private static void HandleValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (MaskedTextBehavior)d;
        if (behavior.updating || (behavior.provider is null))
        {
            return;
        }

        behavior.Load(behavior.provider, (string?)e.NewValue ?? string.Empty);
    }

    private void UpdateProvider()
    {
        var mask = Mask;
        if (String.IsNullOrEmpty(mask))
        {
            provider = null;
            return;
        }

        provider = new MaskedTextProvider(mask, CultureInfo.CurrentCulture) { PromptChar = PromptChar };
        Load(provider, Value ?? AssociatedObject.Text);
    }

    private void Load(MaskedTextProvider current, string text)
    {
        if (!current.Set(Normalize(text)))
        {
            current.Clear();
        }

        Render(current, current.LastAssignedPosition + 1, 0);
    }

    private string Normalize(string text) => Normalizer?.Invoke(text) ?? text;

    private void OnSelectionChanged(object sender, RoutedEventArgs e)
    {
        if (updating || composing)
        {
            return;
        }

        selectionStart = AssociatedObject.SelectionStart;
        selectionLength = AssociatedObject.SelectionLength;
    }

    private void OnTextCompositionStarted(TextBox sender, TextCompositionStartedEventArgs args)
    {
        composing = true;
    }

    private void OnTextCompositionEnded(TextBox sender, TextCompositionEndedEventArgs args)
    {
        composing = false;
        ApplyText();
    }

    private void OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        if (!composing)
        {
            ApplyText();
        }
    }

    private void ApplyText()
    {
        if (updating || (provider is null))
        {
            return;
        }

        var oldText = provider.ToDisplayString();
        var newText = AssociatedObject.Text;
        if (newText == oldText)
        {
            return;
        }

        var start = Math.Min(selectionStart, oldText.Length);
        var length = Math.Min(selectionLength, oldText.Length - start);
        var inserted = newText.Length - (oldText.Length - length);
        if ((length > 0) || (inserted > 0))
        {
            if ((inserted >= 0) && IsEdit(oldText, newText, start, start + length, start + inserted))
            {
                if (inserted > 0)
                {
                    Insert(provider, newText.Substring(start, inserted), start, length);
                }
                else
                {
                    Remove(provider, true, start, length);
                }

                return;
            }
        }
        else if (inserted < 0)
        {
            var removed = -inserted;
            var backward = (start >= removed) && IsEdit(oldText, newText, start - removed, start, start - removed);
            var forward = (start + removed <= oldText.Length) && IsEdit(oldText, newText, start, start + removed, start);
            if (backward || forward)
            {
                Remove(provider, !backward || (forward && (AssociatedObject.SelectionStart >= start)), start, 0);
                return;
            }
        }

        var work = (MaskedTextProvider)provider.Clone();
        if (work.Set(Normalize(newText)))
        {
            provider = work;
            Commit(work, work.LastAssignedPosition + 1);
        }
        else
        {
            Render(provider, start, length);
        }
    }

    private static bool IsEdit(string oldText, string newText, int removeStart, int removeEnd, int insertEnd) =>
        (String.CompareOrdinal(oldText, 0, newText, 0, removeStart) == 0) &&
        (String.CompareOrdinal(oldText, removeEnd, newText, insertEnd, oldText.Length - removeEnd) == 0);

    private void Insert(MaskedTextProvider current, string text, int start, int length)
    {
        var input = Normalize(text);
        var work = (MaskedTextProvider)current.Clone();
        if (length > 0)
        {
            work.RemoveAt(start, start + length - 1);
        }

        if ((input.Length == 0) || !work.InsertAt(input, Math.Min(start, work.LastAssignedPosition + 1), out var position, out _))
        {
            Render(current, start, length);
            return;
        }

        provider = work;
        var next = work.FindEditPositionFrom(position + 1, true);
        Commit(work, next >= 0 ? next : work.Length);
    }

    private void Remove(MaskedTextProvider current, bool forward, int start, int length)
    {
        if (length > 0)
        {
            current.RemoveAt(start, start + length - 1);
            Commit(current, start);
            return;
        }

        var position = forward ? current.FindEditPositionFrom(start, true) : start > 0 ? current.FindEditPositionFrom(start - 1, false) : -1;
        if (position < 0)
        {
            Render(current, start, 0);
            return;
        }

        current.RemoveAt(position);
        Commit(current, forward ? start : position);
    }

    private void Commit(MaskedTextProvider current, int caret)
    {
        Render(current, caret, 0);

        updating = true;
        Value = current.AssignedEditPositionCount == 0 ? string.Empty : current.ToString(false, IncludeLiterals);
        updating = false;
    }

    private void Render(MaskedTextProvider current, int caret, int length)
    {
        updating = true;
        AssociatedObject.Text = current.ToDisplayString();
        selectionStart = Math.Min(caret, current.Length);
        selectionLength = Math.Min(length, current.Length - selectionStart);
        AssociatedObject.Select(selectionStart, selectionLength);
        updating = false;
    }
}
