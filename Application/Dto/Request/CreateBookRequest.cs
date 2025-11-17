using Microsoft.AspNetCore.Http;

namespace Api.Controllers;

public class CreateBookRequest
{
    public IFormFile CoverImage { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? PublicationYear { get; set; }
    public int? PageCount { get; set; }
    public int? Weight { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Publisher { get; set; }
    public string[] Authors { get; set; }
    public string[] Genres { get; set; }
}