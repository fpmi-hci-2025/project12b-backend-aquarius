namespace Application.Dto.Response;

public class BookResponse
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? PublicationYear { get; set; }
    public int? PageCount { get; set; }
    public decimal Price { get; set; }
    public int? Weight { get; set; }
    public byte[]? CoverImageUrl { get; set; }
    public string PublisherName { get; set; }
    public List<string> Authors { get; set; }
    public List<string> Genres { get; set; }
    public double? AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public int Quantity { get; set; }
}
