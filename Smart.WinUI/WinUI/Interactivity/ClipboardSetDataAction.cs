namespace Smart.WinUI.Interactivity;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

using Windows.ApplicationModel.DataTransfer;

[RequiresUnreferencedCode("Uses GetMethod to find method by name; method metadata may be trimmed.")]
public sealed class ClipboardSetDataAction : DependencyObject, IAction
{
    public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
        nameof(TargetObject),
        typeof(object),
        typeof(ClipboardSetDataAction),
        new PropertyMetadata(null));

    public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(
        nameof(MethodName),
        typeof(string),
        typeof(ClipboardSetDataAction),
        new PropertyMetadata(null));

    public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(
        nameof(Format),
        typeof(string),
        typeof(ClipboardSetDataAction),
        new PropertyMetadata(null));

    public object TargetObject
    {
        get => GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    public string MethodName
    {
        get => (string)GetValue(MethodNameProperty);
        set => SetValue(MethodNameProperty, value);
    }

    public string Format
    {
        get => (string)GetValue(FormatProperty);
        set => SetValue(FormatProperty, value);
    }

    private MethodInvoker? cachedInvoker;

    private string cachedMethodName = string.Empty;

    private Type? cachedType;

    public object Execute(object? sender, object? parameter)
    {
        var target = TargetObject;
        var methodName = MethodName;
        if (String.IsNullOrEmpty(methodName))
        {
            return false;
        }

        if ((cachedInvoker is null) ||
            (cachedType != target.GetType()) ||
            (cachedMethodName != methodName))
        {
            var methodInfo = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
            if (methodInfo is null)
            {
                return false;
            }

            cachedInvoker = MethodInvoker.Create(methodInfo);
            cachedMethodName = methodName;
            cachedType = target.GetType();
        }

        var result = cachedInvoker.Invoke(target);
        if (result is null)
        {
            return false;
        }

        var package = new DataPackage();
        package.SetData(Format, result);
        Clipboard.SetContent(package);
        return true;
    }
}
