using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IProductRepository
{
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Product?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<Product?> GetBySKUAsync(string sku, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Product> Products, int TotalCount)> GetAllAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySKUAsync(string sku, CancellationToken cancellationToken = default);
}