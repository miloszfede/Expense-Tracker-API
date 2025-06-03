using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Infrastructure.Dependencies
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
