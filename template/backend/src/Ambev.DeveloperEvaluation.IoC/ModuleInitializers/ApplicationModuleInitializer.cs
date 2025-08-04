using Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;
using Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;
using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class ApplicationModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        
        builder.Services.AddScoped<CreateUserCommandValidator>();
        builder.Services.AddScoped<GetUserCommandValidator>();
        builder.Services.AddScoped<DeleteUserCommandValidator>();
        builder.Services.AddScoped<UpdateUserCommandValidator>();
        builder.Services.AddScoped<CreateBranchCommandValidator>();
        builder.Services.AddScoped<DeleteBranchCommandValidator>();
        builder.Services.AddScoped<UpdateBranchCommandValidator>();
        builder.Services.AddScoped<CreateCustomerCommandValidator>();
        builder.Services.AddScoped<DeleteCustomerCommandValidator>();
        builder.Services.AddScoped<UpdateCustomerCommandValidator>();
        builder.Services.AddScoped<GetCustomerCommandValidator>();
        builder.Services.AddScoped<CreateProductCommandValidator>();
        builder.Services.AddScoped<DeleteProductCommandValidator>();
        builder.Services.AddScoped<UpdateProductCommandValidator>();
        builder.Services.AddScoped<GetProductCommandValidator>();
    }
}