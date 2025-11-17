using Api.Controllers;
using Application.Dto.Response;
using AutoMapper;
using Entities;

namespace Application.Mapper;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookResponse>()
            .ForMember(dest => dest.Base64CoverImage, opt => opt.MapFrom(src => Convert.ToBase64String(src.CoverImage)))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Reviews.Count != 0 ? src.Reviews.Average(x => x.Rating) : 0))
            .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count));


        CreateMap<CreateBookRequest, Book>()
            .ForMember(dest => dest.CoverImage, opt => opt.Ignore());
    }
}
