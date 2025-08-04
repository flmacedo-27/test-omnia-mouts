using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.ListBranches;

/// <summary>
/// Response for listing branches.
/// </summary>
public class ListBranchesResponse : PaginatedResponseBase<BranchListItem>
{
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
    /// Gets or sets the branch code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch active.
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; }
} 