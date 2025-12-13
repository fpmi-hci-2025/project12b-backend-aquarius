using Application.Dto.Request;
using Application.Dto.Response;
using Infrastructure.Auth.Contracts;
using Infrastructure.Auth.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthController(
        IAuthenticationService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("sign-up")]
    [ProducesResponseType(200)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    [EndpointDescription("Регистрация нового пользователя в системе. Создает учетную запись пользователя с ролью 'User'. Устанавливает JWT токены.")]
    [EndpointSummary("Регистрация нового пользователя")]
    public async Task<ActionResult<ExtendedTokensResponse>> SignUp([FromBody] RegisterRequest registerDto)
    {
        var tokensResponse = await _authService.RegisterAsync(registerDto);

        return Ok(tokensResponse);
    }

    [AllowAnonymous]
    [HttpPost("sign-in")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Аутентификация пользователя в системе. Проверяет учетные данные и устанавливает JWT токены.")]
    [EndpointSummary("Вход в систему")]
    public async Task<ActionResult<ExtendedTokensResponse>> SignIn([FromBody] LoginRequest loginDto)
    {
        var tokensResponse = await _authService.LoginAsync(loginDto);

        return Ok(tokensResponse);
    }

    [Authorize]
    [HttpPost("sign-out")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [EndpointDescription("Выход пользователя из системы. Деактивирует текущие токены.")]
    [EndpointSummary("Выход из системы")]
    public async Task<ActionResult> Signout()
    {
        var userId = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        await _authService.LogoutAsync(new LogoutRequest { UserId = userId });

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
    public async Task<ActionResult<TokensResponse>> Refresh([FromBody] RefreshRequest tokensToRefresh)
    {
        var tokensResponse = await _authService.RefreshAsync(tokensToRefresh);

        return Ok(tokensResponse);
    }
}
