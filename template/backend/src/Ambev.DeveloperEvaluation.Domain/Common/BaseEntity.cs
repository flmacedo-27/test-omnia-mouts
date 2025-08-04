namespace Ambev.DeveloperEvaluation.Domain.Common;

/// <summary>
/// Base entity class that provides common properties for all entities
/// </summary>
public class BaseEntity : IComparable<BaseEntity>
{
    /// <summary>
    /// Gets or sets the unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the last update to the entity
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the BaseEntity class
    /// </summary>
    public BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the UpdatedAt timestamp
    /// </summary>
    public void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public int CompareTo(BaseEntity? other)
    {
        if (other == null)
        {
            return 1;
        }

        return other!.Id.CompareTo(Id);
    }
}
