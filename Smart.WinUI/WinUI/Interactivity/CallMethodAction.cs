namespace Smart.WinUI.Interactivity;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.Xaml.Interactivity;

using Smart.Linq;

[RequiresUnreferencedCode("Uses GetRuntimeMethods to find method by name; method metadata may be trimmed.")]
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

    private MethodInvoker? cachedInvoker;

    private string cachedMethodName = string.Empty;

    private Type? cachedType;

    private Type? cachedParameterType;

    private bool cachedHasParameter;

    public object Execute(object? sender, object? parameter)
    {
        var target = TargetObject ?? sender;
        var methodName = MethodName;
        if ((target is null) || String.IsNullOrEmpty(methodName))
        {
            return false;
        }

        var methodParameter = MethodParameter;
        var parameterType = methodParameter?.GetType();
        if ((cachedInvoker is null) ||
            (cachedType != target.GetType()) ||
            (cachedMethodName != methodName) ||
            (cachedParameterType != parameterType))
        {
            var methodInfo = target.GetType().GetRuntimeMethods().FirstOrDefault((methodName, methodParameter), static (m, s) =>
                (m.Name == s.methodName) &&
                ((m.GetParameters().Length == 0) ||
                 ((m.GetParameters().Length == 1) &&
                  ((s.methodParameter is null) ||
                   m.GetParameters()[0].ParameterType.GetTypeInfo().IsAssignableFrom(s.methodParameter.GetType().GetTypeInfo())))));
            if (methodInfo is null)
            {
                return false;
            }

            cachedInvoker = MethodInvoker.Create(methodInfo);
            cachedMethodName = methodName;
            cachedType = target.GetType();
            cachedParameterType = parameterType;
            cachedHasParameter = methodInfo.GetParameters().Length > 0;
        }

        if (cachedHasParameter)
        {
            var converter = Converter;
            var argument = (methodParameter is not null) || this.IsSet(MethodParameterProperty)
                ? methodParameter
                : converter is not null ? converter.Convert(parameter, typeof(object), ConverterParameter, null) : parameter;
            cachedInvoker.Invoke(target, argument);
        }
        else
        {
            cachedInvoker.Invoke(target);
        }

        return true;
    }
}
