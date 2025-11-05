namespace Infrastructure.Auth.Options;

public class AccessTokenOptions
{
    public string ValidIssuer { get; set; }
    public double ExpirationTimeMinutes { get; set; }
}
