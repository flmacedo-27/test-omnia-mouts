
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly DefaultContext _context;
    private readonly ILogger<CustomerRepository> _logger;

    public CustomerRepository(DefaultContext context, ILogger<CustomerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);
            return customer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating customer with email: {Email}", customer.Email);
            throw;
        }
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Customers.FindAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer with ID: {CustomerId}", id);
            throw new InvalidOperationException("Failed to retrieve customer", ex);
        }
    }

    public async Task<(IEnumerable<Customer> Customers, int TotalCount)> GetAllAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _context.Customers.AsQueryable();
            var totalCount = await query.CountAsync(cancellationToken);

            var customers = await query
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (customers, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customers with page {PageNumber} and size {PageSize}", pageNumber, pageSize);
            throw new InvalidOperationException("Failed to retrieve customers", ex);
        }
    }

    public async Task<Customer> UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync(cancellationToken);
            return customer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating customer with ID: {CustomerId}", customer.Id);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await GetByIdAsync(id, cancellationToken);
            
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting customer with ID: {CustomerId}", id);
            throw;
        }
    }

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer with email: {CustomerEmail}", email);
            throw new InvalidOperationException("Failed to retrieve customer by email", ex);
        }
    }

    public async Task<Customer?> GetByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.DocumentNumber == documentNumber, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer with document number: {CustomerDocument}", documentNumber);
            throw new InvalidOperationException("Failed to retrieve customer by document number", ex);
        }
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Customers.AnyAsync(c => c.Email == email, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if customer exists with email: {CustomerEmail}", email);
            throw new InvalidOperationException("Failed to check if customer exists", ex);
        }
    }

    public async Task<bool> ExistsByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Customers.AnyAsync(c => c.DocumentNumber == documentNumber, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if customer exists with document number: {CustomerDocument}", documentNumber);
            throw new InvalidOperationException("Failed to check if customer exists", ex);
        }
    }
}