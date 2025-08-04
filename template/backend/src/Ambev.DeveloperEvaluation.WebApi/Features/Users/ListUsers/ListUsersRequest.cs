namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;

public class ListUsersRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 