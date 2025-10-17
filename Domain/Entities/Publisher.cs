using Entities.Base;

namespace Entities;

public class Publisher : EntityBase
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ContactEmail { get; set; }

    public ICollection<Book> Books { get; set; }
}
