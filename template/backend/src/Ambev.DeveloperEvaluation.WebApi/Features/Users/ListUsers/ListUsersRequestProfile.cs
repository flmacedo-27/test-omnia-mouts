using Ambev.DeveloperEvaluation.Application.Users.ListUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListUsers;

public class ListUsersRequestProfile : Profile
{
    public ListUsersRequestProfile()
    {
        CreateMap<ListUsersRequest, ListUserCommand>();
    }
} 