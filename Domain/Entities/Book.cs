using Entities.Base;

namespace Entities;

public class Book : EntityBase
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? PublicationYear { get; set; }
    public int? PageCount { get; set; }
    public int? Weight { get; set; }
    public decimal Price { get; set; }
    public byte[]? CoverImage { get; set; }
    public int Quantity { get; set; }
    public string Publisher { get; set; }
    public string[] Authors { get; set; }
    public string[] Genres { get; set; }
    public virtual ICollection<Review> Reviews { get; set; } = [];
}
