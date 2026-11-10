namespace Smart.WinUI.Hosting;

using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWinUI<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TApp>(this IServiceCollection services)
        where TApp : Application
    {
        services.AddSingleton<TApp>();
        services.AddHostedService<ApplicationHostingService<TApp>>();

        return services;
    }
}
