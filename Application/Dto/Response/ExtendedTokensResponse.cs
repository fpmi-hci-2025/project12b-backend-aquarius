namespace Application.Dto.Response;

public class ExtendedTokensResponse : TokensResponse
{
    public UserDetails UserDetails { get; set; }
}

public class UserDetails 
{
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
}