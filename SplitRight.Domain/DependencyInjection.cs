using Microsoft.Extensions.DependencyInjection;
using SplitRight.Domain.Contracts.Interfaces;
using SplitRight.Domain.Contracts.Services;

namespace SplitRight.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<IJwtTokenGeneratorService, JwtTokenGeneratorService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddSingleton(typeof(IAppLoggerService<>), typeof(AppLoggerService<>));

            services.AddScoped<AbuseProtectionService>();

            return services;
        }
    }
}
