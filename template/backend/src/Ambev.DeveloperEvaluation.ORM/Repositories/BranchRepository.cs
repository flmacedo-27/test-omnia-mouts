using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly DefaultContext _context;
    private readonly ILogger<BranchRepository> _logger;

    public BranchRepository(DefaultContext context, ILogger<BranchRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Branch> CreateAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Branches.Add(branch);
            await _context.SaveChangesAsync(cancellationToken);
            return branch;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating branch with name: {BranchName}", branch.Name);
            throw;
        }
    }

    public async Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Branches.FindAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving branch with ID: {BranchId}", id);
            throw new InvalidOperationException("Failed to retrieve branch", ex);
        }
    }

    public async Task<(IEnumerable<Branch> Branches, int TotalCount)> GetAllAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _context.Branches.AsQueryable();
            var totalCount = await query.CountAsync(cancellationToken);

            var branches = await query
                .OrderBy(b => b.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (branches, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving branches with page {PageNumber} and size {PageSize}", pageNumber, pageSize);
            throw new InvalidOperationException("Failed to retrieve branches", ex);
        }
    }

    public async Task<Branch> UpdateAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Branches.Update(branch);
            await _context.SaveChangesAsync(cancellationToken);
            return branch;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating branch with ID: {BranchId}", branch.Id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var branch = await GetByIdAsync(id, cancellationToken);
            if (branch == null)
            {
                _logger.LogWarning("Branch not found for deletion with ID: {BranchId}", id);
                return false;
            }

            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogDebug("Branch deleted successfully with ID: {BranchId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting branch with ID: {BranchId}", id);
            throw;
        }
    }
} 