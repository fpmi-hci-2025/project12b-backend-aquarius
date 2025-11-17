using Application.Contracts;
using Application.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/carts")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(
        ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    [Authorize(Roles = "User")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Получение корзины текущего авторизованного пользователя.")]
    [EndpointSummary("Получить корзину")]
    public async Task<ActionResult<CartResponse>> GetCart()
    {
        var userId = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

        var result = await _cartService.GetCart(userId);

        return Ok(result);
    }

    [HttpPost("{bookId}")]
    [Authorize(Roles = "User")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Добавление книги в корзину авторизованного пользователя. Если книга уже есть в корзине, увеличивает количество на указанное значение. Проверяет доступность книги на складе.")]
    [EndpointSummary("Добавить книгу в корзину")]
    public async Task<IActionResult> AddToCart([FromRoute] Guid bookId, [FromQuery] int quantity = 1)
    {
        var userId = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

        await _cartService.AddItemToCart(userId, bookId, quantity);

        return Ok();
    }

    [HttpDelete("{bookId}")]
    [Authorize(Roles = "User")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Удаление книги из корзины пользователя. Полностью удаляет позицию с указанной книгой независимо от количества.")]
    [EndpointSummary("Удалить книгу из корзины")]
    public async Task<IActionResult> RemoveFromCart([FromRoute] Guid bookId)
    {
        var userId = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

        await _cartService.RemoveFromCart(userId, bookId);

        return Ok();
    }
}
