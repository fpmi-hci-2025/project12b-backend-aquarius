using Entities.Base;

namespace Entities;

public class Order : EntityBase
{
    public string? CustomerNotes { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public string Status { get; set; }

    public Guid? PaymentId { get; set; }
    public Payment? Payment { get; set; }

    public Guid ShippingAddressId { get; set; }
    public PickupAddress ShippingAddress { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
}
