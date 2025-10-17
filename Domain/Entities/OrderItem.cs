using Entities.Base;

namespace Entities;

public class OrderItem : EntityBase
{
    public int Quantity { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    public Guid BookId { get; set; }
    public Book Book { get; set; }
}
