using Entities;
using Entities.Base;

namespace Domain.Entities;

public class Cart : EntityBase
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<CartItem>? CartItems { get; set; } = [];
}
