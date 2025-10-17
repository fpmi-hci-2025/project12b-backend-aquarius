namespace Application.Dto.Request;

public class CreateOrderRequest
{
    public string CustomerNotes { get; set; }
    public string ShippingAddress { get; set; }
    public List<(Guid BookId, int Count)> OrderItems { get; set; }
}
