namespace Application.Dto.Response;

public class CartResponse
{
    IEnumerable<CartItemResponse> CartItems { get; set; }
    public int TotalItems { get; set; }
    public decimal ItemsPrice { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalPrice { get; set; }
}

public class CartItemResponse
{
    public Guid BookId { get; set; }
    public string BookTitle { get; set; }
    public decimal BookPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}

