using Application.Contracts;
using Application.Dto.Request;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/reviews")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(
        IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Получение списка отзывов с поддержкой пагинации и фильтрации.")]
    [EndpointSummary("Получить список отзывов")]
    public async Task<ActionResult<IEnumerable<ReviewResponse>>> GetReviews([FromQuery] ReviewFilters filters)
    {
        var reviews = await _reviewService.GetReviews(filters);

        return Ok(reviews);
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Создание нового отзыва на книгу. Пользователь может оставить только один отзыв на каждую книгу. Проверяется, что пользователь приобретал данную книгу.")]
    [EndpointSummary("Создать отзыв")]
    public async Task<ActionResult<ReviewResponse>> CreateReview([FromBody] CreateReviewRequest request)
    {
        var userId = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var result = await _reviewService.CreateReview(userId, request);

        return Ok(result);
    }
}
