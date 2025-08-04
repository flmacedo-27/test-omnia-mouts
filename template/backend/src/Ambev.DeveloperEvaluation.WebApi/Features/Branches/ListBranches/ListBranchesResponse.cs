using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.ListBranches;

/// <summary>
/// Response for listing branches with pagination.
/// </summary>
public class ListBranchesResponse : PaginatedList<BranchListItem>
{
    /// <summary>
    /// Initializes a new instance of ListBranchesResponse
    /// </summary>
    /// <param name="branches">The list of branches</param>
    /// <param name="totalCount">The total count of branches</param>
    /// <param name="currentPage">The current page number</param>
    /// <param name="pageSize">The page size</param>
    public ListBranchesResponse(IEnumerable<BranchListItem> branches, int totalCount, int currentPage, int pageSize)
        : base(branches.ToList(), totalCount, currentPage, pageSize)
    {
    }
} 