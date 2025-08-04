using Ambev.DeveloperEvaluation.Application.Common.Paginated;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

public class ListBranchesResult : PaginatedResult<BranchListItem>
{
    public IEnumerable<BranchListItem> Branches 
    { 
        get => Items; 
        set => Items = value; 
    }
}

public class BranchListItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
} 