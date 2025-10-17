namespace Application.Dto.Request;

public class PaymentRequest
{
    public string PaymentMethod { get; set; }
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
}
