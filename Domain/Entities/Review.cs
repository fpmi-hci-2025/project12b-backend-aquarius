using Entities.Base;

namespace Entities;

public class Review : EntityBase
{
    public int Rating { get; set; }
    public string? Comment { get; set; }

    public Guid BookId { get; set; }
    public Book Book { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
}
