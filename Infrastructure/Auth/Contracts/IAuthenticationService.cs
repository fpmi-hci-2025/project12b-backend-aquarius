using Application.Dto.Request;
using Application.Dto.Response;

namespace Infrastructure.Auth.Contracts;

public interface IAuthenticationService
{
    Task<TokensResponse> LoginAsync(LoginRequest loginRequest);
    Task<TokensResponse> RegisterAsync(RegisterRequest registerRequest);
    Task<TokensResponse> RefreshAsync(RefreshRequest refreshRequest);
    Task LogoutAsync(LogoutRequest logoutRequest);
}
