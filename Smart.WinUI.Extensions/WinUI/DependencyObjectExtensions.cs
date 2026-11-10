namespace Smart.WinUI;

using System.Reactive.Linq;

using Microsoft.UI.Xaml;

public static class DependencyObjectExtensions
{
    public static IObservable<TValue> PropertyChangedAsObservable<TValue>(
        this DependencyObject source,
        DependencyProperty property)
    {
        return Observable.Create<TValue>(observer =>
        {
            var token = source.RegisterPropertyChangedCallback(property, (_, _) => observer.OnNext((TValue)source.GetValue(property)));
            return () => source.UnregisterPropertyChangedCallback(property, token);
        });
    }
}
