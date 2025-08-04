using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(DefaultContext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating product with SKU: {SKU}", product.SKU);
            throw;
        }
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Products.FindAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product with ID: {ProductId}", id);
            throw new InvalidOperationException("Failed to retrieve product", ex);
        }
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> GetAllAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _context.Products.AsQueryable();
            var totalCount = await query.CountAsync(cancellationToken);

            var products = await query
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (products, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products with page {PageNumber} and size {PageSize}", pageNumber, pageSize);
            throw new InvalidOperationException("Failed to retrieve products", ex);
        }
    }
    
    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating product with ID: {ProductId}", product.Id);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await GetByIdAsync(id, cancellationToken);
            
            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting product with ID: {ProductId}", id);
            throw;
        }
    }

    public async Task<Product?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product with code: {ProductCode}", code);
            throw new InvalidOperationException("Failed to retrieve product by code", ex);
        }
    }

    public async Task<Product?> GetBySKUAsync(string sku, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.SKU == sku, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product with SKU: {ProductSKU}", sku);
            throw new InvalidOperationException("Failed to retrieve product by SKU", ex);
        }
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Products.AnyAsync(p => p.Code == code, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if product exists with code: {ProductCode}", code);
            throw new InvalidOperationException("Failed to check if product exists", ex);
        }
    }

    public async Task<bool> ExistsBySKUAsync(string sku, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Products.AnyAsync(p => p.SKU == sku, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if product exists with SKU: {ProductSKU}", sku);
            throw new InvalidOperationException("Failed to check if product exists", ex);
        }
    }
} 