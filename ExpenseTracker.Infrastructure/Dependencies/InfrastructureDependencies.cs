using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Infrastructure.Dependencies
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            services.AddSingleton<ICategoryRepository, InMemoryCategoryRepository>();
            services.AddSingleton<IExpenseRepository, InMemoryExpenseRepository>();
            services.AddSingleton<IIncomeRepository, InMemoryIncomeRepository>();
            
            return services;
        }
    }
}
