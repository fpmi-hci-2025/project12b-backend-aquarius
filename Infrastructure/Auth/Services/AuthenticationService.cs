using Application.Dto.Request;
using Application.Dto.Response;
using Application.Exceptions;
using Domain.Entities;
using Entities;
using Infrastructure.Auth.Contracts;
using Infrastructure.Auth.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Auth.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly BookStoreDbContext _dbContext;
    private readonly ITokenService _tokenService;
    private readonly IOptions<RefreshTokenOptions> _refreshTokenOptions;

    public AuthenticationService(
        BookStoreDbContext dbContext,
        ITokenService tokenService,
        IOptions<RefreshTokenOptions> refreshTokenOptions)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
        _refreshTokenOptions = refreshTokenOptions;
    }

    public async Task<TokensResponse> LoginAsync(LoginRequest loginRequest)
    {
        var user = await _dbContext.Users
            .Include(x => x.Tokens)
            .FirstOrDefaultAsync(x => x.Email == loginRequest.Email);

        if (user == null)
        {
            throw new NotFoundException("User with given email wasn't found");
        }

        if (!BCrypt.Net.BCrypt.EnhancedVerify(loginRequest.PasswordHash, user.PasswordHash))
        {
            throw new BadRequestException("Invalid credentials");
        }

        var accessToken = await _tokenService.CreateAccessToken(user);
        var refreshToken = _tokenService.CreateRefreshToken();

        user.Tokens.RefreshToken = refreshToken;
        user.Tokens.RefreshTokenExpirationDate = DateTime.UtcNow.AddHours(_refreshTokenOptions.Value.ExpirationTimeHours);
        await _dbContext.SaveChangesAsync();

        return new TokensResponse { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    public async Task LogoutAsync(LogoutRequest logoutRequest)
    {
        var user = await _dbContext.Users
            .Include(x => x.Tokens)
            .FirstOrDefaultAsync(x => x.Id.ToString() == logoutRequest.UserId);

        if (user == null)
        {
            throw new NotFoundException("User wasn't found");
        }

        user.Tokens.RefreshToken = null;
        user.Tokens.RefreshTokenExpirationDate = new DateTime();
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TokensResponse> RefreshAsync(RefreshRequest refreshRequest)
    {
        var accessToken = refreshRequest.AccessToken;
        var refreshToken = refreshRequest.RefreshToken;

        if (accessToken == null || refreshToken == null)
        {
            throw new BadRequestException("Not all tokens are present in request");
        }

        var principal = _tokenService.GetClaimsPrincipalFromExpired(accessToken);
        var userEmail = principal.Claims.First(x => x.Type == ClaimTypes.Email).Value;

        var user = await _dbContext.Users
            .Include(x => x.Tokens)
            .FirstOrDefaultAsync(x => x.Email == userEmail);

        if (user == null)
        {
            throw new NotFoundException("User with given email wasn't found");
        }

        if (user.Tokens.RefreshToken != refreshToken)
        {
            throw new BadRequestException("No such refresh token found for user");
        }

        if (user.Tokens.RefreshTokenExpirationDate < DateTime.UtcNow)
        {
            throw new BadRequestException("Refresh token expired");
        }

        var newAccessToken = await _tokenService.CreateAccessToken(user);
        var newRefreshToken = _tokenService.CreateRefreshToken();

        user.Tokens.RefreshToken = newRefreshToken;
        user.Tokens.RefreshTokenExpirationDate = DateTime.UtcNow.AddHours(_refreshTokenOptions.Value.ExpirationTimeHours);

        await _dbContext.SaveChangesAsync();

        return new TokensResponse { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
    }

    public async Task<TokensResponse> RegisterAsync(RegisterRequest registerRequest)
    {
        var existingUser = _dbContext.Users.FirstOrDefault(x => x.Email == registerRequest.Email);
        if (existingUser != null)
        {
            throw new ConflictException("User with given email already exists");
        }

        var user = new User
        {
            CreatedAt = DateTime.UtcNow,
            Email = registerRequest.Email,
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(registerRequest.PasswordHash),
            Phone = registerRequest.Phone,
            DateOfBirth = registerRequest.DateOfBirth,
            Tokens = new UserTokens(),
            Cart = new Cart(),
            Wishlist = new Wishlist(),
        };

        var userRole = await _dbContext.Roles.Include(x => x.Users).FirstAsync(x => x.Name == "User");
        userRole.Users.Add(user);
        user.Roles.Add(userRole);

        var accessToken = await _tokenService.CreateAccessToken(user);
        var refreshToken = _tokenService.CreateRefreshToken();

        user.Tokens.RefreshToken = refreshToken;
        user.Tokens.RefreshTokenExpirationDate = DateTime.UtcNow.AddHours(_refreshTokenOptions.Value.ExpirationTimeHours);

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return new TokensResponse { AccessToken = accessToken, RefreshToken = refreshToken };
    }
}
