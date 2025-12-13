using Application.Dto.Response;
using AutoMapper;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>();
        CreateMap<User, UserDetails>();
    }
}
