using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/wishlists")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [EndpointDescription("Получение списка избранного текущего пользователя. Возвращает все книги, добавленные в вишлист.")]
        [EndpointSummary("Получить список избранного")]
        public IActionResult GetWishlist()
        {
            // Get user's wishlist
            return Ok();
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
        public IActionResult AddToWishlist([FromRoute] Guid bookId)
        {
            // Add book to wishlist
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
        public IActionResult RemoveFromWishlist([FromRoute] Guid bookId)
        {
            // Remove book from wishlist
            return Ok();
        }
    }
}
