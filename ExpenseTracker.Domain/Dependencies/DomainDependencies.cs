using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Domain.Dependencies
{
    public static class DomainDependencies
    {
        public static IServiceCollection AddDomainDependencies(this IServiceCollection services)
        {
            return services;
        }
    }
}
