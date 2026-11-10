namespace Smart.WinUI.Internal;

using System.Runtime.InteropServices;

internal static partial class NativeMethods
{
    public const uint WmClose = 0x0010;

    [LibraryImport("user32.dll", EntryPoint = "PostMessageW")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool PostMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);
}
