namespace Example;

using Microsoft.Extensions.Hosting;

using Smart.WinUI;
using Smart.WinUI.Hosting;

internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddWinUI<App>();
        builder.Services.AddWinUIServices();

        using var host = builder.Build();
        host.Run();
    }
}
