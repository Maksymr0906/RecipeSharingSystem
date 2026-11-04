using Microsoft.Extensions.DependencyInjection;
using RecipeSharingSystem.Core.Interfaces.Infrastructure;
using RecipeSharingSystem.Infrastructure.Auth;
using RecipeSharingSystem.Infrastructure.Crypto;

namespace RecipeSharingSystem.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        // Singleton: the key ring is built once; EF Core model building keeps a reference to it.
        services.AddSingleton<IFieldEncryptor, AesGcmFieldEncryptor>();

        return services;
    }
}
