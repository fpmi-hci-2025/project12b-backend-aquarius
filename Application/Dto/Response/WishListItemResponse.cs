namespace Application.Dto.Response;

public class WishListItemResponse
{
    public Guid BookId { get; set; }
    public string BookTitle { get; set; }
    public byte[] CoverImage { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public List<string> Authors { get; set; }
    public List<string> Genres { get; set; }
}
