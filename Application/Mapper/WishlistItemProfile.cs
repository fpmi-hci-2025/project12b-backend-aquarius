using Application.Dto.Response;
using AutoMapper;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper;

public class WishlistItemProfile : Profile
{
    public WishlistItemProfile()
    {
        CreateMap<Book, WishListItemResponse>()
            .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.Authors))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres));
    }
}
