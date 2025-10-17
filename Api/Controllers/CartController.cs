using Application.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
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
            // Get user's cart
            return Ok();
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
        public IActionResult AddToCart([FromRoute] Guid bookId, [FromQuery] int quantity = 1)
        {
            // Add book to cart
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
        public IActionResult RemoveFromCart([FromRoute] Guid bookId)
        {
            // Remove book from cart
            return Ok();
        }
    }
}
