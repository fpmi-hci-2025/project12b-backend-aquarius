using Entities.Base;

namespace Entities;

public class Wishlist : EntityBase
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ICollection<Book>? Books { get; set; }
}
