using Entities;
using Entities.Base;

namespace Domain.Entities;

public class Cart : EntityBase
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ICollection<CartItem>? CartItems { get; set; }
}
