namespace Smart.WinUI.Interactivity;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;

using Windows.ApplicationModel.DataTransfer;

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

    private MethodInfo? cachedMethod;

    private Type? cachedType;

    [RequiresUnreferencedCode("Method is resolved by reflection")]
    public ClipboardSetDataAction()
    {
    }

    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Target type is determined at runtime via XAML; callers must ensure the type is preserved")]
    public object Execute(object? sender, object? parameter)
    {
        var target = TargetObject;
        var methodName = MethodName;
        if (String.IsNullOrEmpty(methodName))
        {
            return false;
        }

        if ((cachedMethod is null) ||
            (cachedType != target.GetType()) ||
            (cachedMethod.Name != methodName))
        {
            cachedMethod = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
            cachedType = target.GetType();
            if (cachedMethod is null)
            {
                return false;
            }
        }

        var result = cachedMethod.Invoke(target, null);
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
