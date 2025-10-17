using Application.Dto.Request;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [EndpointDescription("Получение списка всех пользователей системы с поддержкой пагинации и фильтрации.")]
        [EndpointSummary("Получить список пользователей")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers(UserFilters filters)
        {
            // Get all users (admin only)
            return Ok();
        }

        [HttpPut("{userId}/roles")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [EndpointDescription("Обновление ролей и прав доступа пользователя. Позволяет назначать и удалять роли.")]
        [EndpointSummary("Обновить роли пользователя")]
        public IActionResult UpdateUserRoles([FromRoute] Guid userId, [FromBody] UpdateRolesRequest request)
        {
            // Update user roles and permissions (admin only)
            return Ok();
        }
    }
}
