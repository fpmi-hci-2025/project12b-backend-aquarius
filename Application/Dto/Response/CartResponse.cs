namespace Application.Dto.Response;

public class CartResponse
{
    public IEnumerable<CartItemResponse> CartItems { get; set; }
    public int TotalItems { get => CartItems.Sum(x => x.Quantity); }
    public decimal ItemsPrice { get => CartItems.Sum(x => x.TotalPrice); }
    public decimal ShippingCost { get => ItemsPrice * (decimal)0.05; }
    public decimal TotalPrice { get => ItemsPrice + ShippingCost; }
}

public class CartItemResponse
{
    public string? Base64CoverImage { get; set; }
    public Guid BookId { get; set; }
    public string BookTitle { get; set; }
    public decimal BookPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get => BookPrice * Quantity; }
}

