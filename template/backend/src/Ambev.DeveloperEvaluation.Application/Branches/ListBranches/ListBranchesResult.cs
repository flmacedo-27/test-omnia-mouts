using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

/// <summary>
/// Result for listing branches with pagination.
/// </summary>
public class ListBranchesResult : PaginatedResult<BranchListItem>
{
    /// <summary>
    /// Initializes a new instance of ListBranchesResult
    /// </summary>
    /// <param name="branches">The list of branches</param>
    /// <param name="totalCount">The total count of branches</param>
    /// <param name="currentPage">The current page number</param>
    /// <param name="pageSize">The page size</param>
    public ListBranchesResult(IEnumerable<BranchListItem> branches, int totalCount, int currentPage, int pageSize)
        : base(branches, totalCount, currentPage, pageSize)
    {
    }
}

/// <summary>
/// Represents a branch item in the list.
/// </summary>
public class BranchListItem
{
    /// <summary>
    /// Gets or sets the branch ID.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the branch address.
    /// </summary>
    public string Address { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the branch phone number.
    /// </summary>
    public string Phone { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the branch email.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets whether the branch is active.
    /// </summary>
    public bool Active { get; set; }
    
    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the last update date.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 