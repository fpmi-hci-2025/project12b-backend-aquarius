using Entities.Base;

namespace Entities;

public class OrderItem : EntityBase
{
    public int Quantity { get; set; }

    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; }

    public Guid BookId { get; set; }
    public virtual Book Book { get; set; }
}
