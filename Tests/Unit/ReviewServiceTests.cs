using Application.Dto.Request.Filters;
using Application.Dto.Request;
using Application.Dto.Response;
using Application.Exceptions;
using Application.Services;
using AutoMapper;
using Domain;
using Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Unit;

public class ReviewServiceTests
{
    private readonly Mock<IRepository<Review>> _mockReviewRepository;
    private readonly Mock<IRepository<Order>> _mockOrderRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ReviewService _reviewService;

    public ReviewServiceTests()
    {
        _mockReviewRepository = new Mock<IRepository<Review>>();
        _mockOrderRepository = new Mock<IRepository<Order>>();
        _mockMapper = new Mock<IMapper>();
        _reviewService = new ReviewService(_mockReviewRepository.Object, _mockOrderRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateReview_UserBoughtBook_CreatesReview()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var request = new CreateReviewRequest
        {
            BookId = bookId,
            Comment = "Great book!",
            Rating = 5
        };

        var order = new Order
        {
            UserId = userId,
            OrderItems = new List<OrderItem>
                {
                    new OrderItem { BookId = bookId }
                }
        };

        _mockOrderRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Order, bool>>>()))
            .ReturnsAsync(order);
        _mockReviewRepository.Setup(x => x.AddAsync(It.IsAny<Review>())).Returns(Task.FromResult(new Review()));
        _mockReviewRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));

        // Act
        await _reviewService.CreateReview(userId, request);

        // Assert
        _mockReviewRepository.Verify(x => x.AddAsync(It.Is<Review>(r =>
            r.UserId == userId &&
            r.BookId == bookId &&
            r.Comment == "Great book!" &&
            r.Rating == 5)), Times.Once);
        _mockReviewRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateReview_UserNeverBoughtBook_ThrowsBadRequestException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var request = new CreateReviewRequest { BookId = bookId };

        _mockOrderRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Order, bool>>>()))
            .ReturnsAsync((Order?)null);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _reviewService.CreateReview(userId, request));
    }

    [Fact]
    public async Task GetReviews_ValidFilters_ReturnsReviews()
    {
        // Arrange
        var filters = new ReviewFilters
        {
            BookId = Guid.NewGuid(),
            PageNumber = 1,
            PageSize = 10,
            CreatedAtFrom = DateTime.UtcNow.AddDays(-7),
            CreatedAtTo = DateTime.UtcNow
        };

        var reviews = new List<Review>
            {
                new Review { Id = Guid.NewGuid(), BookId = filters.BookId, Comment = "Good", Rating = 4 },
                new Review { Id = Guid.NewGuid(), BookId = filters.BookId, Comment = "Excellent", Rating = 5 }
            };

        var reviewResponses = new List<ReviewResponse>
            {
                new ReviewResponse { Comment = "Good", Rating = 4 },
                new ReviewResponse { Comment = "Excellent", Rating = 5 }
            };

        _mockReviewRepository.Setup(x => x.GetPagedAsync(1, 10, It.IsAny<System.Linq.Expressions.Expression<System.Func<Review, bool>>>()))
            .ReturnsAsync(reviews);
        _mockMapper.Setup(x => x.Map<IEnumerable<ReviewResponse>>(reviews))
            .Returns(reviewResponses);

        // Act
        var result = await _reviewService.GetReviews(filters);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockReviewRepository.Verify(x => x.GetPagedAsync(1, 10, It.IsAny<System.Linq.Expressions.Expression<System.Func<Review, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetReviews_NoDateFilters_UsesDefaultDates()
    {
        // Arrange
        var filters = new ReviewFilters
        {
            BookId = Guid.NewGuid(),
            PageNumber = 1,
            PageSize = 10
        };

        var reviews = new List<Review> { new Review { Id = Guid.NewGuid() } };
        var reviewResponses = new List<ReviewResponse> { new ReviewResponse() };

        _mockReviewRepository.Setup(x => x.GetPagedAsync(1, 10, It.IsAny<System.Linq.Expressions.Expression<System.Func<Review, bool>>>()))
            .ReturnsAsync(reviews);
        _mockMapper.Setup(x => x.Map<IEnumerable<ReviewResponse>>(reviews))
            .Returns(reviewResponses);

        // Act
        var result = await _reviewService.GetReviews(filters);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _mockReviewRepository.Verify(x => x.GetPagedAsync(1, 10, It.IsAny<System.Linq.Expressions.Expression<System.Func<Review, bool>>>()), Times.Once);
    }
}