using Entities.Base;

namespace Entities;

public class Order : EntityBase
{
    public string? CustomerNotes { get; set; }

    public Guid UserId { get; set; }
    public virtual User User { get; set; }

    public string Status { get; set; }

    public Guid? PaymentId { get; set; }
    public virtual Payment? Payment { get; set; }

    public string DeliveryAddress { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = [];
}
