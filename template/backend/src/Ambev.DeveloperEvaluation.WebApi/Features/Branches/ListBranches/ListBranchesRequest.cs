using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.ListBranches;

/// <summary>
/// Request for listing branches.
/// </summary>
public class ListBranchesRequest : PaginatedRequest
{
    /// <summary>
    /// Gets or sets whether to include only active branches.
    /// </summary>
    public bool ActiveOnly { get; set; } = false;
} 