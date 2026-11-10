namespace Smart.WinUI;

using System.Reactive.Linq;

using Microsoft.UI.Xaml;

public static class UIElementExtensions
{
    public static IObservable<TEventArgs> RoutedEventAsObservable<TDelegate, TEventArgs>(
        this UIElement source,
        RoutedEvent routedEvent,
        Func<Action<TEventArgs>, TDelegate> conversion,
        bool handledEventsToo = false)
        where TDelegate : Delegate
        where TEventArgs : RoutedEventArgs
    {
        return Observable.Create<TEventArgs>(observer =>
        {
            var handler = conversion(observer.OnNext);
            source.AddHandler(routedEvent, handler, handledEventsToo);
            return () => source.RemoveHandler(routedEvent, handler);
        });
    }
}
