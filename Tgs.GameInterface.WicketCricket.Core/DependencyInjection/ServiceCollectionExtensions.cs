using System.Reflection;
using Tgs.GameInterface.WicketCricket.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfaceCore(
        this IServiceCollection services,
        Action<PolicySettingsOptions, IServiceProvider> policySettingsOptions)
        => services
        .AddOptions<PolicySettingsOptions>()
        .Configure(policySettingsOptions)
        .Services
        .RegisterAllServiceUtilityTypes(
            Assembly.GetExecutingAssembly(),
            ServiceLifetime.Transient)
        .AddHttpClient<IGameApiClient, GameApiClient>()
        .Services;
}
