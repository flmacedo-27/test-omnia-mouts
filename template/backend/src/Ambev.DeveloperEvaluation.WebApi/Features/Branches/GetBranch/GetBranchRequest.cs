namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;

/// <summary>
/// Request for getting a branch.
/// </summary>
public class GetBranchRequest
{
    /// <summary>
    /// Gets or sets the branch ID.
    /// </summary>
    public Guid Id { get; set; }
} 