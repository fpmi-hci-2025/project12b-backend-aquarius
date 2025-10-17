using Entities.Base;

namespace Entities;

public class Payment : EntityBase
{
    public string TransactionNumber { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}
