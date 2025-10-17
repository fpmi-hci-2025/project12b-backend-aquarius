namespace Application.Dto.Request;

public class CreateReviewRequest
{
    public int Rating { get; set; }
    public string Comment { get; set; }
    public Guid BookId { get; set; }
}
