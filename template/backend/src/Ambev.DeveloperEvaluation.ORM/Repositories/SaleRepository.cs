using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Repository implementation for Sale entity
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;
    private readonly ILogger<SaleRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the SaleRepository
    /// </summary>
    /// <param name="context">The database context</param>
    /// <param name="logger">The logger</param>
    public SaleRepository(DefaultContext context, ILogger<SaleRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new sale
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating sale with number: {SaleNumber}", sale.SaleNumber);
        
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Sale created successfully with ID: {SaleId}", sale.Id);
        return sale;
    }

    /// <summary>
    /// Gets a sale by ID
    /// </summary>
    /// <param name="id">The sale ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <summary>
    /// Gets a sale by sale number
    /// </summary>
    /// <param name="saleNumber">The sale number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellationToken);
    }

    /// <summary>
    /// Gets all sales with pagination
    /// </summary>
    /// <param name="pageNumber">The page number</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A tuple containing the sales and total count</returns>
    public async Task<(IEnumerable<Sale> Sales, int TotalCount)> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Branch)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .OrderByDescending(s => s.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);
        
        var sales = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (sales, totalCount);
    }

    /// <summary>
    /// Updates a sale
    /// </summary>
    /// <param name="sale">The sale to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale</returns>
    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating sale with ID: {SaleId}", sale.Id);
        
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Sale updated successfully with ID: {SaleId}", sale.Id);
        return sale;
    }

    /// <summary>
    /// Generates a unique sale number
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A unique sale number</returns>
    public async Task<string> GenerateSaleNumberAsync(CancellationToken cancellationToken)
    {
        var lastSale = await _context.Sales
            .OrderByDescending(s => s.SaleNumber)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastSale == null)
        {
            return "SALE-000001";
        }

        var lastNumber = int.Parse(lastSale.SaleNumber.Split('-')[1]);
        var nextNumber = lastNumber + 1;
        
        return $"SALE-{nextNumber:D6}";
    }
} 