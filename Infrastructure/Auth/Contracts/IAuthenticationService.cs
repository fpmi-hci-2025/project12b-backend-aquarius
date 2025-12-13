using Application.Dto.Request;
using Application.Dto.Response;

namespace Infrastructure.Auth.Contracts;

public interface IAuthenticationService
{
    Task<ExtendedTokensResponse> LoginAsync(LoginRequest loginRequest);
    Task<ExtendedTokensResponse> RegisterAsync(RegisterRequest registerRequest);
    Task<TokensResponse> RefreshAsync(RefreshRequest refreshRequest);
    Task LogoutAsync(LogoutRequest logoutRequest);
}
