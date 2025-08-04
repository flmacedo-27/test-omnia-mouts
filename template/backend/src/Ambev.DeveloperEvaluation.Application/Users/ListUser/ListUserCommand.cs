using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUser;

public class ListUserCommand : IRequest<ListUserResult>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 