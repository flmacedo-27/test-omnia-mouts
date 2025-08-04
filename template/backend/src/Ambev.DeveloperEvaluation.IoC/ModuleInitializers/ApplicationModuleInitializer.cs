using Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;
using Ambev.DeveloperEvaluation.Application.Branches.GetBranch;
using Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;
using Ambev.DeveloperEvaluation.Application.Branches.ListBranches;
using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ambev.DeveloperEvaluation.Application.Users.ListUser;
using Ambev.DeveloperEvaluation.Application.Customers.ListCustomers;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;

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
        builder.Services.AddScoped<ListUserCommandValidator>();
        builder.Services.AddScoped<CreateBranchCommandValidator>();
        builder.Services.AddScoped<DeleteBranchCommandValidator>();
        builder.Services.AddScoped<GetBranchCommandValidator>();
        builder.Services.AddScoped<UpdateBranchCommandValidator>();
        builder.Services.AddScoped<ListBranchesCommandValidator>();
        builder.Services.AddScoped<CreateCustomerCommandValidator>();
        builder.Services.AddScoped<DeleteCustomerCommandValidator>();
        builder.Services.AddScoped<UpdateCustomerCommandValidator>();
        builder.Services.AddScoped<GetCustomerCommandValidator>();
        builder.Services.AddScoped<ListCustomersCommandValidator>();
        builder.Services.AddScoped<CreateProductCommandValidator>();
        builder.Services.AddScoped<DeleteProductCommandValidator>();
        builder.Services.AddScoped<UpdateProductCommandValidator>();
        builder.Services.AddScoped<GetProductCommandValidator>();
        builder.Services.AddScoped<ListProductsCommandValidator>();
        builder.Services.AddScoped<CreateSaleCommandValidator>();
        builder.Services.AddScoped<CancelSaleCommandValidator>();
        builder.Services.AddScoped<GetSaleCommandValidator>();
        builder.Services.AddScoped<ListSalesCommandValidator>();
    }
}