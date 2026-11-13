namespace Smart.WinUI.Interactivity;

using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.Xaml.Interactivity;

using Windows.System;
using Windows.UI.Core;

[TypeConstraint(typeof(FrameworkElement))]
public sealed class KeyTrigger : Trigger<FrameworkElement>
{
    public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
        nameof(Key),
        typeof(VirtualKey),
        typeof(KeyTrigger),
        new PropertyMetadata(VirtualKey.None));

    public static readonly DependencyProperty ModifiersProperty = DependencyProperty.Register(
        nameof(Modifiers),
        typeof(VirtualKeyModifiers),
        typeof(KeyTrigger),
        new PropertyMetadata(VirtualKeyModifiers.None));

    public static readonly DependencyProperty ActiveOnFocusProperty = DependencyProperty.Register(
        nameof(ActiveOnFocus),
        typeof(bool),
        typeof(KeyTrigger),
        new PropertyMetadata(false));

    public static readonly DependencyProperty FiredOnProperty = DependencyProperty.Register(
        nameof(FiredOn),
        typeof(KeyTriggerFiredOn),
        typeof(KeyTrigger),
        new PropertyMetadata(KeyTriggerFiredOn.KeyDown));

    private UIElement? target;

    private KeyTriggerFiredOn targetFiredOn;

    public VirtualKey Key
    {
        get => (VirtualKey)GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    public VirtualKeyModifiers Modifiers
    {
        get => (VirtualKeyModifiers)GetValue(ModifiersProperty);
        set => SetValue(ModifiersProperty, value);
    }

    public bool ActiveOnFocus
    {
        get => (bool)GetValue(ActiveOnFocusProperty);
        set => SetValue(ActiveOnFocusProperty, value);
    }

    public KeyTriggerFiredOn FiredOn
    {
        get => (KeyTriggerFiredOn)GetValue(FiredOnProperty);
        set => SetValue(FiredOnProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject.IsLoaded)
        {
            Subscribe();
        }
        else
        {
            AssociatedObject.Loaded += OnLoaded;
        }
    }

    protected override void OnDetaching()
    {
        AssociatedObject.Loaded -= OnLoaded;
        Unsubscribe();

        base.OnDetaching();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        AssociatedObject.Loaded -= OnLoaded;
        Subscribe();
    }

    private void Subscribe()
    {
        target = ActiveOnFocus ? AssociatedObject : GetRoot(AssociatedObject);
        targetFiredOn = FiredOn;
        if (targetFiredOn == KeyTriggerFiredOn.KeyDown)
        {
            target.KeyDown += OnKey;
        }
        else
        {
            target.KeyUp += OnKey;
        }
    }

    private void Unsubscribe()
    {
        if (target is null)
        {
            return;
        }

        if (targetFiredOn == KeyTriggerFiredOn.KeyDown)
        {
            target.KeyDown -= OnKey;
        }
        else
        {
            target.KeyUp -= OnKey;
        }

        target = null;
    }

    private void OnKey(object sender, KeyRoutedEventArgs e)
    {
        if ((e.Key == Key) && (GetModifiers() == Modifiers))
        {
            Interaction.ExecuteActions(AssociatedObject, Actions, e);
        }
    }

    private static UIElement GetRoot(UIElement element)
    {
        var root = element;
        var parent = VisualTreeHelper.GetParent(element);
        while (parent is not null)
        {
            if (parent is UIElement uiElement)
            {
                root = uiElement;
            }

            parent = VisualTreeHelper.GetParent(parent);
        }

        return root;
    }

    private static VirtualKeyModifiers GetModifiers()
    {
        var modifiers = VirtualKeyModifiers.None;
        if (IsDown(VirtualKey.Control))
        {
            modifiers |= VirtualKeyModifiers.Control;
        }

        if (IsDown(VirtualKey.Shift))
        {
            modifiers |= VirtualKeyModifiers.Shift;
        }

        if (IsDown(VirtualKey.Menu))
        {
            modifiers |= VirtualKeyModifiers.Menu;
        }

        return modifiers;
    }

    private static bool IsDown(VirtualKey key) =>
        (InputKeyboardSource.GetKeyStateForCurrentThread(key) & CoreVirtualKeyStates.Down) != 0;
}
