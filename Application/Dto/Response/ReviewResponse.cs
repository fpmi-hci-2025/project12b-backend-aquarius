namespace Application.Dto.Response;

public class ReviewResponse
{
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public string BookTitle { get; set; }
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
}
