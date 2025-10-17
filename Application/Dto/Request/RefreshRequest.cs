namespace Application.Dto.Request;

public class RefreshRequest
{
    public string RefreshToken { get; set; }
    public string AccessToken { get; set; }
}
