using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUser;

/// <summary>
/// Result for listing users with pagination.
/// </summary>
public class ListUserResult : PaginatedResult<UserListItem>
{
    /// <summary>
    /// Initializes a new instance of ListUserResult
    /// </summary>
    /// <param name="users">The list of users</param>
    /// <param name="totalCount">The total count of users</param>
    /// <param name="currentPage">The current page number</param>
    /// <param name="pageSize">The page size</param>
    public ListUserResult(IEnumerable<UserListItem> users, int totalCount, int currentPage, int pageSize)
        : base(users, totalCount, currentPage, pageSize)
    {
    }
}

/// <summary>
/// Represents a user item in the list.
/// </summary>
public class UserListItem
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the user name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user email.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user phone number.
    /// </summary>
    public string Phone { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user role.
    /// </summary>
    public UserRole Role { get; set; }
    
    /// <summary>
    /// Gets or sets the user status.
    /// </summary>
    public UserStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the last update date.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
} 