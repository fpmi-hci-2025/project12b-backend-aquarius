using Entities;
using Infrastructure.Auth.Contracts;
using Infrastructure.Auth.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Auth.Services;

public class TokenService : ITokenService
{
    private readonly IOptions<AccessTokenOptions> _accessTokenOptions;
    private readonly BookStoreDbContext _dbContext;

    public TokenService(
        IOptions<AccessTokenOptions> accessTokenOptions,
        BookStoreDbContext dbContext)
    {
        _accessTokenOptions = accessTokenOptions;
        _dbContext = dbContext;
    }

    public async Task<string> CreateAccessToken(User user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public string CreateRefreshToken()
    {
        var token = new byte[64];

        var generator = RandomNumberGenerator.Create();
        generator.GetBytes(token);

        return Convert.ToBase64String(token);
    }

    public ClaimsPrincipal GetClaimsPrincipalFromExpired(string token)
    {
        var secretKey = Environment.GetEnvironmentVariable("JWT_KEY");

        var validation = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _accessTokenOptions.Value.ValidIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out var _);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"));
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var roles = (await _dbContext.Users
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == user.Email)
            )?.Roles;

        if (roles != null)
        {
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
        (
        issuer: _accessTokenOptions.Value.ValidIssuer,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_accessTokenOptions.Value.ExpirationTimeMinutes),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }
}
