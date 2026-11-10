namespace Smart.WinUI;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWinUIServices(this IServiceCollection services)
    {
        services.AddSingleton(static _ => DispatcherQueue.GetForCurrentThread() ?? throw new InvalidOperationException("DispatcherQueue must be resolved on the UI thread."));
        return services;
    }
}
