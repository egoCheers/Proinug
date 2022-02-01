using Proinug.WebUI.Interfaces;
using Proinug.WebUI.Services;

namespace Proinug.WebUI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthService(this IServiceCollection services, IConfiguration configuration)
    {
        var authApiBaseAddress = configuration["AuthApiBaseAddress"];
        services.AddHttpClient<IAuthService, AuthService>(client =>
        {
            client.BaseAddress = new Uri(authApiBaseAddress);
        });
        return services;
    }

    // public static IServiceCollection AddKktCloudService(this IServiceCollection services, IConfiguration configuration)
    // {
    //     var kktApiBaseAddress = configuration["WebApiBaseAddress"];
    //     services.AddHttpClient<IKktCloudService, KktCloudService>(client =>
    //     {
    //         client.BaseAddress = new Uri(kktApiBaseAddress);
    //     });
    //     return services;
    // }
    //
    // public static IServiceCollection AddUsersService(this IServiceCollection services, IConfiguration configuration)
    // {
    //     var usersApiBaseAddress = configuration["AuthApiBaseAddress"];
    //     services.AddHttpClient<IUsersService, UsersService>(client =>
    //     {
    //         client.BaseAddress = new Uri((usersApiBaseAddress));
    //     });
    //     return services;
    // }
}