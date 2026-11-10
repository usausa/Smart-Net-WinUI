namespace Smart.WinUI.Interactivity;

using System.Windows.Input;

using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.Xaml.Interactivity;

[TypeConstraint(typeof(UIElement))]
public sealed class LongPressBehavior : Behavior<UIElement>
{
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command),
        typeof(ICommand),
        typeof(LongPressBehavior),
        new PropertyMetadata(null));

    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        nameof(CommandParameter),
        typeof(object),
        typeof(LongPressBehavior),
        new PropertyMetadata(null));

    public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
        nameof(Duration),
        typeof(TimeSpan),
        typeof(LongPressBehavior),
        new PropertyMetadata(TimeSpan.FromMilliseconds(500)));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public TimeSpan Duration
    {
        get => (TimeSpan)GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    private readonly PointerEventHandler pressedHandler;

    private readonly PointerEventHandler releasedHandler;

    private DispatcherQueueTimer? timer;

    public LongPressBehavior()
    {
        pressedHandler = OnPressed;
        releasedHandler = OnReleased;
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.AddHandler(UIElement.PointerPressedEvent, pressedHandler, true);
        AssociatedObject.AddHandler(UIElement.PointerReleasedEvent, releasedHandler, true);
        AssociatedObject.AddHandler(UIElement.PointerCaptureLostEvent, releasedHandler, true);
        AssociatedObject.AddHandler(UIElement.PointerExitedEvent, releasedHandler, true);
    }

    protected override void OnDetaching()
    {
        AssociatedObject.RemoveHandler(UIElement.PointerPressedEvent, pressedHandler);
        AssociatedObject.RemoveHandler(UIElement.PointerReleasedEvent, releasedHandler);
        AssociatedObject.RemoveHandler(UIElement.PointerCaptureLostEvent, releasedHandler);
        AssociatedObject.RemoveHandler(UIElement.PointerExitedEvent, releasedHandler);
        if (timer is not null)
        {
            timer.Stop();
            timer.Tick -= OnTick;
            timer = null;
        }

        base.OnDetaching();
    }

    private void OnPressed(object sender, PointerRoutedEventArgs e)
    {
        if (!e.GetCurrentPoint(AssociatedObject).Properties.IsLeftButtonPressed)
        {
            return;
        }

        if (timer is null)
        {
            timer = DispatcherQueue.CreateTimer();
            timer.IsRepeating = false;
            timer.Tick += OnTick;
        }

        timer.Stop();
        timer.Interval = Duration;
        timer.Start();
    }

    private void OnReleased(object sender, PointerRoutedEventArgs e)
    {
        timer?.Stop();
    }

    private void OnTick(DispatcherQueueTimer sender, object args)
    {
        var command = Command;
        if (command is null)
        {
            return;
        }

        var parameter = CommandParameter;
        if (command.CanExecute(parameter))
        {
            command.Execute(parameter);
        }
    }
}
