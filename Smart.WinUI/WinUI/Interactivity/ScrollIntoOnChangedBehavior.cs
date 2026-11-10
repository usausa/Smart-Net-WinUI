namespace Smart.WinUI.Interactivity;

using System.Collections.Specialized;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.Xaml.Interactivity;

using Smart.WinUI.Internal;

[TypeConstraint(typeof(Selector))]
public sealed class ScrollIntoOnChangedBehavior : Behavior<Selector>
{
    public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register(
        nameof(Enabled),
        typeof(bool),
        typeof(ScrollIntoOnChangedBehavior),
        new PropertyMetadata(true));

    public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
        nameof(Position),
        typeof(ScrollPosition),
        typeof(ScrollIntoOnChangedBehavior),
        new PropertyMetadata(ScrollPosition.Last));

    public bool Enabled
    {
        get => (bool)GetValue(EnabledProperty);
        set => SetValue(EnabledProperty, value);
    }

    public ScrollPosition Position
    {
        get => (ScrollPosition)GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    private INotifyCollectionChanged? subscribedCollection;

    private long itemsSourceToken;

    private bool loaded;

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.Loaded += OnLoaded;
        AssociatedObject.Unloaded += OnUnloaded;
        itemsSourceToken = AssociatedObject.RegisterPropertyChangedCallback(ItemsControl.ItemsSourceProperty, OnItemsSourceChanged);

        if (AssociatedObject.IsLoaded)
        {
            loaded = true;
            Subscribe(AssociatedObject.ItemsSource as INotifyCollectionChanged);
        }
    }

    protected override void OnDetaching()
    {
        loaded = false;
        Subscribe(null);

        AssociatedObject.UnregisterPropertyChangedCallback(ItemsControl.ItemsSourceProperty, itemsSourceToken);

        AssociatedObject.Loaded -= OnLoaded;
        AssociatedObject.Unloaded -= OnUnloaded;

        base.OnDetaching();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        loaded = true;
        Subscribe(AssociatedObject.ItemsSource as INotifyCollectionChanged);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        loaded = false;
        Subscribe(null);
    }

    private void OnItemsSourceChanged(DependencyObject sender, DependencyProperty dp)
    {
        if (loaded)
        {
            Subscribe(AssociatedObject.ItemsSource as INotifyCollectionChanged);
        }
    }

    private void Subscribe(INotifyCollectionChanged? collection)
    {
        if (ReferenceEquals(subscribedCollection, collection))
        {
            return;
        }

        if (subscribedCollection is not null)
        {
            subscribedCollection.CollectionChanged -= OnCollectionChanged;
        }

        subscribedCollection = collection;

        if (subscribedCollection is not null)
        {
            subscribedCollection.CollectionChanged += OnCollectionChanged;
        }
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var selector = AssociatedObject;
        if (!Enabled || (selector is null))
        {
            return;
        }

        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            SelectorHelper.ScrollIntoView(selector, Position);
        }
    }
}
