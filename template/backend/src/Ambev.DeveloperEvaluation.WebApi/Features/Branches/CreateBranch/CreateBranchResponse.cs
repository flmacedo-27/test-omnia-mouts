using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;

/// <summary>
/// Response for creating a new branch.
/// </summary>
public class CreateBranchResponse
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
    /// Gets or sets the branch code.
    /// </summary>
    public string code { get; set; } = string.Empty;

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