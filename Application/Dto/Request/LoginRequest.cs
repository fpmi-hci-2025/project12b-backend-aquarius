namespace Application.Dto.Request;

public class LoginRequest
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}
