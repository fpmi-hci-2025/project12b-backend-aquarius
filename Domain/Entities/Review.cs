using Entities.Base;

namespace Entities;

public class Review : EntityBase
{
    public int Rating { get; set; }
    public string? Comment { get; set; }

    public Guid BookId { get; set; }
    public virtual Book Book { get; set; }

    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}
