using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
        
        RegisterRepositories(builder.Services);
    }

    /// <summary>
    /// Registers all repository interfaces with their implementations using reflection
    /// </summary>
    /// <param name="services">The service collection</param>
    private static void RegisterRepositories(IServiceCollection services)
    {
        var repositoryMappings = new Dictionary<Type, Type>
        {
            { typeof(IUserRepository), typeof(UserRepository) },
            { typeof(ICustomerRepository), typeof(CustomerRepository) },
            { typeof(IBranchRepository), typeof(BranchRepository) },
            { typeof(IProductRepository), typeof(ProductRepository) },
            { typeof(ISaleRepository), typeof(SaleRepository) }
        };

        foreach (var mapping in repositoryMappings)
        {
            services.AddScoped(mapping.Key, mapping.Value);
        }
    }
}