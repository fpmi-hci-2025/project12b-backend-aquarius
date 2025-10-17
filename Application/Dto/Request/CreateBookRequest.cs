namespace Api.Controllers;

public class CreateBookRequest
{
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? PublicationYear { get; set; }
    public int? PageCount { get; set; }
    public int? Weight { get; set; }
    public decimal Price { get; set; }
    public string PublisherName { get; set; }
    public List<string> AuthorNames { get; set; }
    public List<string> GenreNames { get; set; }
}