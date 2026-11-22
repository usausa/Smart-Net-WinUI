namespace Smart.WinUI.Interactivity;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

using Smart.Mvvm.Messaging;

[RequiresUnreferencedCode("Uses GetRuntimeProperty to find property by name; property metadata may be trimmed.")]
public sealed class ResolvePropertyAction : DependencyObject, IAction
{
    public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
        nameof(TargetObject),
        typeof(object),
        typeof(ResolvePropertyAction),
        new PropertyMetadata(null));

    public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
        nameof(PropertyName),
        typeof(string),
        typeof(ResolvePropertyAction),
        new PropertyMetadata(string.Empty));

    public object? TargetObject
    {
        get => GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    public string PropertyName
    {
        get => (string)GetValue(PropertyNameProperty);
        set => SetValue(PropertyNameProperty, value);
    }

    private PropertyInfo? cachedProperty;

    private Type? cachedType;

    public object Execute(object? sender, object? parameter)
    {
        if (parameter is not ResolveEventArgs args)
        {
            return false;
        }

        var target = TargetObject ?? sender;
        var propertyName = PropertyName;
        if ((target is null) || String.IsNullOrEmpty(propertyName))
        {
            return false;
        }

        if ((cachedProperty is null) ||
            (cachedType != target.GetType()) ||
            (cachedProperty.Name != propertyName))
        {
            cachedProperty = target.GetType().GetRuntimeProperty(propertyName);
            cachedType = target.GetType();
            if (cachedProperty is null)
            {
                return false;
            }
        }

        args.Result = cachedProperty.GetValue(target);
        return true;
    }
}
