namespace Application.Dto.Request;

public class CreateOrderRequest
{
    public string? CustomerNotes { get; set; }
    public string DeliveryAddress { get; set; }
    public List<OrderItem> OrderItems { get; set; }

    public class OrderItem
    {
        public Guid BookId { get; set; }
        public int Count { get; set; }
    }
}
