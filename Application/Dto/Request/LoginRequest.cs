using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Request;

public class LoginRequest
{
    [EmailAddress]
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}
