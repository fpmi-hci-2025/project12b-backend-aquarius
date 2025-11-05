using Application.Dto.Request;
using Application.Dto.Response;
using Infrastructure.Auth.Contracts;
using Infrastructure.Auth.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly IOptions<RefreshTokenOptions> _refreshTokenOptions;
    private readonly IOptions<AccessTokenOptions> _accessTokenOptions;

    public AuthController(
        IAuthenticationService authService,
        IOptions<AccessTokenOptions> accessTokenOptions,
        IOptions<RefreshTokenOptions> refreshTokenOptions)
    {
        _authService = authService;
        _accessTokenOptions = accessTokenOptions;
        _refreshTokenOptions = refreshTokenOptions;
    }

    [AllowAnonymous]
    [HttpPost("sign-up")]
    [ProducesResponseType(200)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    [EndpointDescription("Регистрация нового пользователя в системе. Создает учетную запись пользователя с ролью 'User'. Устанавливает JWT токены.")]
    [EndpointSummary("Регистрация нового пользователя")]
    public async Task<ActionResult> SignUp([FromBody] RegisterRequest registerDto)
    {
        var tokens = await _authService.RegisterAsync(registerDto);
        SetTokensInCookies(tokens);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("sign-in")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Аутентификация пользователя в системе. Проверяет учетные данные и устанавливает JWT токены.")]
    [EndpointSummary("Вход в систему")]
    public async Task<ActionResult> SignIn([FromBody] LoginRequest loginDto)
    {
        var tokens = await _authService.LoginAsync(loginDto);
        SetTokensInCookies(tokens);

        return Ok();
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
        var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        if (email == null)
        {
            return BadRequest("Token contains no email claims");
        }

        await _authService.LogoutAsync(new LogoutRequest { Email = email });

        Response.Cookies.Delete("accessToken");
        Response.Cookies.Delete("refreshToken");

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
        var tokens = await _authService.RefreshAsync(tokensToRefresh);

        SetTokensInCookies(tokens);

        return Ok();
    }

    private void SetTokensInCookies(TokensResponse tokens)
    {
        var refreshTokenCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddHours(_refreshTokenOptions.Value.ExpirationTimeHours)
        };

        var accessTokenCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(_accessTokenOptions.Value.ExpirationTimeMinutes)
        };

        Response.Cookies.Append("accessToken", tokens.AccessToken, accessTokenCookieOptions);
        Response.Cookies.Append("refreshToken", tokens.RefreshToken, refreshTokenCookieOptions);
    }
}
