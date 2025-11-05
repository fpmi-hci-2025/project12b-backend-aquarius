using Entities;
using System.Security.Claims;

namespace Infrastructure.Auth.Contracts;

public interface ITokenService
{
    public ClaimsPrincipal GetClaimsPrincipalFromExpired(string token);

    public Task<string> CreateAccessToken(User user);

    public string CreateRefreshToken();
}
