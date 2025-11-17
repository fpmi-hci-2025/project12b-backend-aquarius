using Application.Contracts;
using Application.Dto.Request;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using Application.Exceptions;
using AutoMapper;
using Domain;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class ReviewService : IReviewService
{
    private readonly IRepository<Review> _reviewRepository;
    private readonly IRepository<Order> _orderRepository;
    private readonly IMapper _mapper;

    public ReviewService(
        IRepository<Review> reviewRepository,
        IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    public async Task CreateReview(Guid userId, CreateReviewRequest request)
    {
        var userBookOrder = await _orderRepository.FirstOrDefaultAsync(
            x => x.UserId == userId && 
                x.OrderItems.Select(o => o.BookId).Contains(request.BookId));

        if (userBookOrder == null)
            throw new BadRequestException($"User {userId} has never bought book {request.BookId}");

        var review = new Review 
        { 
            UserId = userId, 
            BookId = request.BookId, 
            Comment = request.Comment, 
            Rating = request.Rating, 
            CreatedAt = DateTime.UtcNow 
        };

        await _reviewRepository.AddAsync(review);
        await _reviewRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<ReviewResponse>> GetReviews(ReviewFilters filters)
    {
        filters.CreatedAtFrom ??= DateTime.MinValue;
        filters.CreatedAtTo ??= DateTime.UtcNow;

        var reviews = await _reviewRepository.GetPagedAsync(
            filters.PageNumber,
            filters.PageSize,
            x => x.BookId == filters.BookId && 
                x.CreatedAt >= filters.CreatedAtFrom && 
                x.CreatedAt <= filters.CreatedAtTo);

        var result = _mapper.Map<IEnumerable<ReviewResponse>>(reviews);

        return result;
    }
}
