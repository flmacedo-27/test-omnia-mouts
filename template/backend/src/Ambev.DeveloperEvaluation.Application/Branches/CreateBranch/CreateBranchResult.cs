namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

public class CreateBranchResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
} 