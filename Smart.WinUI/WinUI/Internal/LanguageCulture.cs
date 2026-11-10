namespace Smart.WinUI.Internal;

using System.Globalization;

internal static class LanguageCulture
{
    public static CultureInfo Resolve(string? language) =>
        String.IsNullOrEmpty(language) ? CultureInfo.CurrentCulture : CultureInfo.GetCultureInfo(language);
}
