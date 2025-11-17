namespace Application.Dto.Response;

public class BookResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? PublicationYear { get; set; }
    public int? PageCount { get; set; }
    public decimal Price { get; set; }
    public int? Weight { get; set; }
    public string? Base64CoverImage { get; set; }
    public string Publisher { get; set; }
    public string[] Authors { get; set; }
    public string[] Genres { get; set; }
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public int Quantity { get; set; }
}
