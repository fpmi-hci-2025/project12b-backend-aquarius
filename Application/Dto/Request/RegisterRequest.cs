using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Request;

public class RegisterRequest
{
    [EmailAddress]
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public DateOnly? DateOfBirth { get; set; }
}
