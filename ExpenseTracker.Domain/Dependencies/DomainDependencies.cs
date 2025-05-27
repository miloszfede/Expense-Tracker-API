namespace ExpenseTracker.Domain.Dependencies
{
    public static class DomainDependencies
    {
        public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddDomainDependencies(this Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            return services;
        }
    }
}
