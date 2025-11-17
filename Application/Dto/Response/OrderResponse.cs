namespace Application.Dto.Response;

public class OrderResponse
{
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CustomerNotes { get; set; }
    public string DeliveryAddress { get; set; }
    public List<OrderItemShortResponse> Items { get; set; }
}

public class OrderItemShortResponse
{
    public Guid BookId { get; set; }
    public string BookTitle { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}