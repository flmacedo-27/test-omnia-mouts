using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a branch in the system.
/// </summary>
public class Branch : BaseEntity
{
    /// <summary>
    /// Gets the branch name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the branch code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets the branch address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets whether the branch is active.
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Initializes a new instance of the Branch class.
    /// </summary>
    public Branch()
    {
        Active = true;
    }

    /// <summary>
    /// Updates the branch information.
    /// </summary>
    /// <param name="name">The new branch name</param>
    /// <param name="address">The new branch address</param>
    /// <param name="phone">The new branch phone</param>
    /// <param name="email">The new branch email</param>
    public void Update(string name, string code, string address)
    {
        Name = name;
        Code = code;
        Address = address;
        UpdateTimestamp();
    }
}