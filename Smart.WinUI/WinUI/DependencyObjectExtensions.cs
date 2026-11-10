namespace Smart.WinUI;

using System.Runtime.InteropServices;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;

public static class DependencyObjectExtensions
{
    public static bool IsSet(this DependencyObject obj, DependencyProperty dp)
    {
        return obj.ReadLocalValue(dp) != DependencyProperty.UnsetValue;
    }

    public static DependencyObject? Parent(this DependencyObject obj)
    {
        if (obj is TextElement textElement)
        {
            try
            {
                return textElement.ElementStart.Parent;
            }
            catch (COMException)
            {
                return null;
            }
        }

        if (obj is FrameworkElement frameworkElement)
        {
            var parent = frameworkElement.Parent;
            if (parent is not null)
            {
                return parent;
            }
        }

        return VisualTreeHelper.GetParent(obj);
    }

    public static T? FindParent<T>(this DependencyObject obj)
        where T : DependencyObject
    {
        while (true)
        {
            var parent = obj.Parent();
            if (parent is null)
            {
                return null;
            }

            if (parent is T typedParent)
            {
                return typedParent;
            }

            obj = parent;
        }
    }

    public static T? FindParent<T>(this DependencyObject obj, Func<T, bool> predicate)
        where T : DependencyObject =>
        obj.FindParent<T, Func<T, bool>>(predicate, static (x, p) => p(x));

    public static T? FindParent<T, TState>(this DependencyObject obj, TState state, Func<T, TState, bool> predicate)
        where T : DependencyObject
    {
        var parent = obj.Parent();
        while (parent is not null)
        {
            if ((parent is T typedParent) && predicate(typedParent, state))
            {
                return typedParent;
            }

            parent = parent.Parent();
        }

        return null;
    }

    public static T? FindParentOrSelf<T>(this DependencyObject obj)
        where T : DependencyObject =>
        obj as T ?? obj.FindParent<T>();

    public static T? FindParentOrSelf<T>(this DependencyObject obj, Func<T, bool> predicate)
        where T : DependencyObject =>
        (obj is T self) && predicate(self) ? self : obj.FindParent(predicate);

    public static T? FindParentOrSelf<T, TState>(this DependencyObject obj, TState state, Func<T, TState, bool> predicate)
        where T : DependencyObject =>
        (obj is T self) && predicate(self, state) ? self : obj.FindParent(state, predicate);

    public static IEnumerable<T> FindDescendants<T>(this DependencyObject source)
        where T : DependencyObject
    {
        var count = VisualTreeHelper.GetChildrenCount(source);
        for (var i = 0; i < count; i++)
        {
            var child = VisualTreeHelper.GetChild(source, i);
            if (child is T typedChild)
            {
                yield return typedChild;
            }

            foreach (var descendant in child.FindDescendants<T>())
            {
                yield return descendant;
            }
        }
    }

    public static IEnumerable<T> FindDescendants<T>(this DependencyObject source, Func<T, bool> predicate)
        where T : DependencyObject
    {
        foreach (var descendant in source.FindDescendants<T>())
        {
            if (predicate(descendant))
            {
                yield return descendant;
            }
        }
    }

    public static IEnumerable<T> FindDescendants<T, TState>(this DependencyObject source, TState state, Func<T, TState, bool> predicate)
        where T : DependencyObject
    {
        foreach (var descendant in source.FindDescendants<T>())
        {
            if (predicate(descendant, state))
            {
                yield return descendant;
            }
        }
    }

    public static T? FindDescendant<T>(this DependencyObject source)
        where T : DependencyObject =>
        source.FindDescendants<T>().FirstOrDefault();

    public static T? FindDescendant<T>(this DependencyObject source, Func<T, bool> predicate)
        where T : DependencyObject =>
        source.FindDescendants<T>().FirstOrDefault(predicate);

    public static T? FindDescendant<T, TState>(this DependencyObject source, TState state, Func<T, TState, bool> predicate)
        where T : DependencyObject
    {
        foreach (var descendant in source.FindDescendants<T>())
        {
            if (predicate(descendant, state))
            {
                return descendant;
            }
        }

        return null;
    }

    public static T? FindDescendantOrSelf<T>(this DependencyObject source)
        where T : DependencyObject =>
        source as T ?? source.FindDescendant<T>();

    public static T? FindDescendantOrSelf<T>(this DependencyObject source, Func<T, bool> predicate)
        where T : DependencyObject =>
        (source is T self) && predicate(self) ? self : source.FindDescendant(predicate);

    public static T? FindDescendantOrSelf<T, TState>(this DependencyObject source, TState state, Func<T, TState, bool> predicate)
        where T : DependencyObject =>
        (source is T self) && predicate(self, state) ? self : source.FindDescendant(state, predicate);

    public static T? FindAscendant<T>(this DependencyObject source)
        where T : DependencyObject
    {
        var parent = VisualTreeHelper.GetParent(source);
        while (parent is not null)
        {
            if (parent is T typedParent)
            {
                return typedParent;
            }

            parent = VisualTreeHelper.GetParent(parent);
        }

        return null;
    }

    public static T? FindAscendant<T>(this DependencyObject source, Func<T, bool> predicate)
        where T : DependencyObject =>
        source.FindAscendant<T, Func<T, bool>>(predicate, static (x, p) => p(x));

    public static T? FindAscendant<T, TState>(this DependencyObject source, TState state, Func<T, TState, bool> predicate)
        where T : DependencyObject
    {
        var parent = VisualTreeHelper.GetParent(source);
        while (parent is not null)
        {
            if ((parent is T typedParent) && predicate(typedParent, state))
            {
                return typedParent;
            }

            parent = VisualTreeHelper.GetParent(parent);
        }

        return null;
    }

    public static T? FindAscendantOrSelf<T>(this DependencyObject source)
        where T : DependencyObject =>
        source as T ?? source.FindAscendant<T>();

    public static T? FindAscendantOrSelf<T>(this DependencyObject source, Func<T, bool> predicate)
        where T : DependencyObject =>
        (source is T self) && predicate(self) ? self : source.FindAscendant(predicate);

    public static T? FindAscendantOrSelf<T, TState>(this DependencyObject source, TState state, Func<T, TState, bool> predicate)
        where T : DependencyObject =>
        (source is T self) && predicate(self, state) ? self : source.FindAscendant(state, predicate);

    public static bool IsDescendantOf(this DependencyObject element, DependencyObject ancestor)
    {
        var current = element.Parent();
        while (current is not null)
        {
            if (current == ancestor)
            {
                return true;
            }

            current = current.Parent();
        }

        return false;
    }
}
