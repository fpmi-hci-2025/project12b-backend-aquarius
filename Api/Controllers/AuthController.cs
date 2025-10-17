using Application.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{

    public AuthController()
    {

    }

    [AllowAnonymous]
    [HttpPost("sign-up")]
    [ProducesResponseType(200)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    [EndpointDescription("Регистрация нового пользователя в системе. Создает учетную запись пользователя с ролью 'User'. Возвращает JWT токены.")]
    [EndpointSummary("Регистрация нового пользователя")]
    public async Task<ActionResult> SignUp([FromBody] RegisterRequest registerDto)
    {
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("sign-in")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Аутентификация пользователя в системе. Проверяет учетные данные и возвращает JWT токены.")]
    [EndpointSummary("Вход в систему")]
    public async Task<ActionResult> SignIn([FromBody] LoginRequest loginDto)
    {
        return Ok();
    }

    [Authorize]
    [HttpPost("sign-out")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Выход пользователя из системы. Деактивирует текущие токены.")]
    [EndpointSummary("Выход из системы")]
    public async Task<ActionResult> Logout()
    {
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(400)]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Обновление JWT токенов. Использует валидный refresh token для получения новой пары access и refresh токенов.")]
    [EndpointSummary("Обновление токенов")]
    public async Task<ActionResult> Refresh([FromBody] RefreshRequest tokensToRefresh)
    {
        return Ok();
    }
}
