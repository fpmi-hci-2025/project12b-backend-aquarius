using Application.Contracts;
using Application.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/wishlists")]
[ApiController]
public class WishlistsController : ControllerBase
{
    private readonly IWishlistService _wishlistService;

    public WishlistsController(
        IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    [HttpGet]
    [Authorize(Roles = "User")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Получение списка избранного текущего пользователя. Возвращает все книги, добавленные в вишлист.")]
    [EndpointSummary("Получить список избранного")]
    public async Task<ActionResult<IEnumerable<WishListItemResponse>>> GetWishlist()
    {
        var userId = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var result = await _wishlistService.GetWishlistItems(userId);

        return Ok(result);
    }

    [HttpPost("{bookId}")]
    [Authorize(Roles = "User")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Добавление книги в список избранного. Если книга уже находится в вишлисте, возвращается ошибка. Проверяет существование книги перед добавлением.")]
    [EndpointSummary("Добавить книгу в избранное")]
    public async Task<ActionResult> AddToWishlist([FromRoute] Guid bookId)
    {
        var userId = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        await _wishlistService.AddToWishlist(userId, bookId);

        return Ok();
    }

    [HttpDelete("{bookId}")]
    [Authorize(Roles = "User")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Удаление книги из списка избранного. Полностью удаляет книгу из вишлиста пользователя.")]
    [EndpointSummary("Удалить книгу из избранного")]
    public async Task<ActionResult> RemoveFromWishlist([FromRoute] Guid bookId)
    {
        var userId = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        await _wishlistService.RemoveFromWishlist(userId, bookId);

        return Ok();
    }
}
