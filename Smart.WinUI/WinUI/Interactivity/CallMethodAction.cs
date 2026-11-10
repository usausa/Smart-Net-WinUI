namespace Smart.WinUI.Interactivity;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.Xaml.Interactivity;

public sealed class CallMethodAction : DependencyObject, IAction
{
    public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
        nameof(TargetObject),
        typeof(object),
        typeof(CallMethodAction),
        new PropertyMetadata(null));

    public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(
        nameof(MethodName),
        typeof(string),
        typeof(CallMethodAction),
        new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty MethodParameterProperty = DependencyProperty.Register(
        nameof(MethodParameter),
        typeof(object),
        typeof(CallMethodAction),
        new PropertyMetadata(null));

    public static readonly DependencyProperty ConverterProperty = DependencyProperty.Register(
        nameof(Converter),
        typeof(IValueConverter),
        typeof(CallMethodAction),
        new PropertyMetadata(null));

    public static readonly DependencyProperty ConverterParameterProperty = DependencyProperty.Register(
        nameof(ConverterParameter),
        typeof(object),
        typeof(CallMethodAction),
        new PropertyMetadata(null));

    public object? TargetObject
    {
        get => GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    public string MethodName
    {
        get => (string)GetValue(MethodNameProperty);
        set => SetValue(MethodNameProperty, value);
    }

    public object? MethodParameter
    {
        get => GetValue(MethodParameterProperty);
        set => SetValue(MethodParameterProperty, value);
    }

    public IValueConverter? Converter
    {
        get => (IValueConverter?)GetValue(ConverterProperty);
        set => SetValue(ConverterProperty, value);
    }

    public object? ConverterParameter
    {
        get => GetValue(ConverterParameterProperty);
        set => SetValue(ConverterParameterProperty, value);
    }

    private MethodInfo? cachedMethod;

    private Type? cachedType;

    private Type? cachedParameterType;

    [RequiresUnreferencedCode("Method is resolved by reflection")]
    public CallMethodAction()
    {
    }

    [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Target type is determined at runtime via XAML; callers must ensure the type is preserved")]
    public object Execute(object? sender, object? parameter)
    {
        var target = TargetObject ?? sender;
        var methodName = MethodName;
        if ((target is null) || String.IsNullOrEmpty(methodName))
        {
            return false;
        }

        var parameterType = MethodParameter?.GetType();
        if ((cachedMethod is null) ||
            (cachedType != target.GetType()) ||
            (cachedMethod.Name != methodName) ||
            (cachedParameterType != parameterType))
        {
            cachedMethod = target.GetType().GetRuntimeMethods().FirstOrDefault(m =>
                (m.Name == methodName) &&
                ((m.GetParameters().Length == 0) ||
                 ((m.GetParameters().Length == 1) &&
                  ((MethodParameter is null) ||
                   m.GetParameters()[0].ParameterType.GetTypeInfo().IsAssignableFrom(MethodParameter.GetType().GetTypeInfo())))));
            if (cachedMethod is null)
            {
                return false;
            }

            cachedType = target.GetType();
            cachedParameterType = parameterType;
        }

        if (cachedMethod.GetParameters().Length > 0)
        {
            var methodParameter = MethodParameter;
            var argument = (methodParameter is not null) || (ReadLocalValue(MethodParameterProperty) != DependencyProperty.UnsetValue)
                ? methodParameter
                : Converter?.Convert(parameter, typeof(object), ConverterParameter, null) ?? parameter;
            cachedMethod.Invoke(target, [argument]);
        }
        else
        {
            cachedMethod.Invoke(target, null);
        }

        return true;
    }
}
