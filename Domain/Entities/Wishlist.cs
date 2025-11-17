using Entities.Base;

namespace Entities;

public class Wishlist : EntityBase
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<Book>? Books { get; set; } = [];
}
