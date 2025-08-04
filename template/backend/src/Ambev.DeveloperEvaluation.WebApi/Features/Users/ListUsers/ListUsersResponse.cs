using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Users.ListUser;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;

/// <summary>
/// Response for listing users with pagination.
/// </summary>
public class ListUsersResponse : PaginatedList<UserListItem>
{
    /// <summary>
    /// Initializes a new instance of ListUsersResponse
    /// </summary>
    /// <param name="users">The list of users</param>
    /// <param name="totalCount">The total count of users</param>
    /// <param name="currentPage">The current page number</param>
    /// <param name="pageSize">The page size</param>
    public ListUsersResponse(IEnumerable<UserListItem> users, int totalCount, int currentPage, int pageSize)
        : base(users.ToList(), totalCount, currentPage, pageSize)
    {
    }
} 