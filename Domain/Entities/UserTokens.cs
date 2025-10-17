using Entities.Base;

namespace Entities;

public class UserTokens : EntityBase
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpirationDate { get; set; }
}
