using Entities;
using Entities.Base;

namespace Domain.Entities;

public class CartItem : EntityBase
{
    public Guid CartId { get; set; }
    public virtual Cart Cart { get; set; }
    public Guid BookId { get; set; }
    public virtual Book Book { get; set; }
    public int Quantity { get; set; }
}
