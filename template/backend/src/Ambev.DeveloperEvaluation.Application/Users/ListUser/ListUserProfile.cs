using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.ListUser;

public class ListUserProfile : Profile
{
    public ListUserProfile()
    {
        CreateMap<User, UserListItem>();
    }
} 