namespace Smart.WinUI.Interactivity;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

using Smart.Linq;
using Smart.Mvvm.Messaging;

[RequiresUnreferencedCode("Uses GetRuntimeMethods to find method by name; method metadata may be trimmed.")]
public sealed class ResolveMethodAction : DependencyObject, IAction
{
    public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
        nameof(TargetObject),
        typeof(object),
        typeof(ResolveMethodAction),
        new PropertyMetadata(null));

    public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(
        nameof(MethodName),
        typeof(string),
        typeof(ResolveMethodAction),
        new PropertyMetadata(string.Empty));

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

    private MethodInvoker? cachedInvoker;

    private string cachedMethodName = string.Empty;

    private Type? cachedType;

    public object Execute(object? sender, object? parameter)
    {
        if (parameter is not ResolveEventArgs args)
        {
            return false;
        }

        var target = TargetObject ?? sender;
        var methodName = MethodName;
        if ((target is null) || String.IsNullOrEmpty(methodName))
        {
            return false;
        }

        if ((cachedInvoker is null) ||
            (cachedType != target.GetType()) ||
            (cachedMethodName != methodName))
        {
            var methodInfo = target.GetType().GetRuntimeMethods()
                .FirstOrDefault(methodName, static (m, s) => (m.Name == s) && (m.GetParameters().Length == 0));
            if (methodInfo is null)
            {
                return false;
            }

            cachedInvoker = MethodInvoker.Create(methodInfo);
            cachedMethodName = methodName;
            cachedType = target.GetType();
        }

        args.Result = cachedInvoker.Invoke(target);
        return true;
    }
}
