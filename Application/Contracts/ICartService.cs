using Application.Dto.Response;

namespace Application.Contracts;

public interface ICartService
{
    Task<CartResponse> GetCart(Guid userId);

    Task AddItemToCart(Guid userId, Guid bookId, int quantity);

    Task RemoveFromCart(Guid userId, Guid bookId);
}
