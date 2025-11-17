using Application.Contracts;
using Application.Dto.Request;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(
        IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Получение списка всех пользователей системы с поддержкой пагинации и фильтрации.")]
    [EndpointSummary("Получить список пользователей")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers([FromQuery] UserFilters filters)
    {
        var users = await _userService.GetUsersAsync(filters);

        return Ok(users);
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
    public async Task<IActionResult> UpdateUserRoles([FromRoute] Guid userId, [FromBody] UpdateRolesRequest request)
    {
        await _userService.UpdateUserRolesAsync(userId, request);

        return Ok();
    }
}
