using Application.Dto.Request;
using Application.Dto.Request.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/reviews")]
[ApiController]
public class ReviewsController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Получение списка отзывов с поддержкой пагинации и фильтрации.")]
    [EndpointSummary("Получить список отзывов")]
    public IActionResult GetReviews([FromQuery] ReviewFilters filters)
    {
        // Get all reviews
        return Ok();
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
    public IActionResult CreateReview([FromBody] CreateReviewRequest request)
    {
        // Create a new review
        return Ok();
    }
}
