namespace Smart.WinUI.Validation;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Xaml.Interactivity;

using Smart.Mvvm.ViewModels;
using Smart.WinUI.Messaging;
using Smart.WinUI.ViewModels;

[TypeConstraint(typeof(FrameworkElement))]
public sealed class ValidationFocusBehavior : Behavior<FrameworkElement>
{
    public static readonly DependencyProperty RequestProperty = DependencyProperty.Register(
        nameof(Request),
        typeof(ValidationFocusRequest),
        typeof(ValidationFocusBehavior),
        new PropertyMetadata(null, HandleRequestPropertyChanged));

    public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
        nameof(Target),
        typeof(ErrorInfo),
        typeof(ValidationFocusBehavior),
        new PropertyMetadata(null));

    public ValidationFocusRequest? Request
    {
        get => (ValidationFocusRequest?)GetValue(RequestProperty);
        set => SetValue(RequestProperty, value);
    }

    public ErrorInfo? Target
    {
        get => (ErrorInfo?)GetValue(TargetProperty);
        set => SetValue(TargetProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        AddRequested(Request);
    }

    protected override void OnDetaching()
    {
        RemoveRequested(Request);

        base.OnDetaching();
    }

    private void AddRequested(ValidationFocusRequest? request)
    {
        if (request is not null)
        {
            request.FocusRequested += OnFocusRequested;
        }
    }

    private void RemoveRequested(ValidationFocusRequest? request)
    {
        if (request is not null)
        {
            request.FocusRequested -= OnFocusRequested;
        }
    }

    private static void HandleRequestPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var behavior = (ValidationFocusBehavior)d;
        if (behavior.AssociatedObject is null)
        {
            return;
        }

        behavior.RemoveRequested(e.OldValue as ValidationFocusRequest);
        behavior.AddRequested(e.NewValue as ValidationFocusRequest);
    }

    private void OnFocusRequested(object? sender, EventArgs e)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            var root = AssociatedObject;
            if (root is null)
            {
                return;
            }

            var errorInfo = Target ?? (root.DataContext is ExtendViewModelBase vm ? vm.Errors : null);
            if (errorInfo is null)
            {
                return;
            }

            var target = FindTarget(root, errorInfo);
            target?.Focus(FocusState.Programmatic);
        });
    }

    public static UIElement? FindTarget(UIElement parent, ErrorInfo errorInfo)
    {
        if (!IsAvailable(parent))
        {
            return null;
        }

        var count = VisualTreeHelper.GetChildrenCount(parent);
        for (var i = 0; i < count; i++)
        {
            if ((VisualTreeHelper.GetChild(parent, i) is not UIElement child) || !IsAvailable(child))
            {
                continue;
            }

            var key = Smart.WinUI.Behavior.GetKey(child);
            if (!String.IsNullOrEmpty(key) && errorInfo.Contains(key))
            {
                return child;
            }

            var result = FindTarget(child, errorInfo);
            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }

    private static bool IsAvailable(UIElement element) =>
        (element.Visibility == Visibility.Visible) && (element is not Control { IsEnabled: false });
}
