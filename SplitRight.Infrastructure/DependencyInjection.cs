using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SplitRight.Infrastructure.Contracts.Interfaces;
using SplitRight.Infrastructure.Contracts.Services;

namespace SplitRight.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = configuration["Database:Provider"];
            var connectionString = configuration.GetConnectionString("MiniFinancialLedgerKey");

            services.AddDbContext<AppDbContext>(options =>
            {
                switch (provider)
                {
                    case "SqlServer":
                        options.UseSqlServer(connectionString);
                        break;

                    default:
                        throw new InvalidOperationException("Provider não suportado.");
                }
            });


            services.AddScoped<IApplicationDbContext, AppDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
