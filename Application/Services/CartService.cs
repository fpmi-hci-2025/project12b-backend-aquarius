using Application.Contracts;
using Application.Dto.Response;
using Domain;
using Domain.Entities;
using Entities;

namespace Application.Services;

public class CartService : ICartService
{
    private readonly IRepository<Cart> _cartRepo;
    private readonly IRepository<Book> _bookRepo;
    private readonly IRepository<CartItem> _cartItemRepo;

    public CartService(
        IRepository<Cart> cartRepo,
        IRepository<CartItem> cartItemRepo,
        IRepository<Book> bookRepo)
    {
        _cartRepo = cartRepo;
        _cartItemRepo = cartItemRepo;
        _bookRepo = bookRepo;
    }

    public async Task AddItemToCart(Guid userId, Guid bookId, int quantity)
    {
        var cart = (await _cartRepo.FindAsync(x => x.UserId == userId)).First();
        var existingCartItem = cart.CartItems.FirstOrDefault(x => x.BookId == bookId);

        if (existingCartItem == null)
        {
            var newItem = new CartItem
            {
                BookId = bookId,
                CartId = cart.Id,
                Quantity = quantity
            };

            cart.CartItems.Add(newItem);
            await _cartItemRepo.AddAsync(newItem);
            await _cartItemRepo.SaveChangesAsync();
        }
        else
        {
            existingCartItem.Quantity += quantity;
        }

        await _cartRepo.SaveChangesAsync();
    }

    public async Task<CartResponse> GetCart(Guid userId)
    {
        var cart = (await _cartRepo.FindAsync(x => x.UserId == userId)).First();

        if (cart.CartItems == null)
        {
            return new CartResponse { CartItems = [] };
        }

        var quantityMap = cart.CartItems.ToDictionary(x => x.BookId, x => x.Quantity);
        var cartBooksIds = cart.CartItems.Select(x => x.BookId);
        var cartBooks = await _bookRepo.FindAsync(x => cartBooksIds.Contains(x.Id));

        var cartItemsResponse = cartBooks.Select(x =>
            new CartItemResponse {
                BookId = x.Id,
                BookPrice = x.Price,
                BookTitle = x.Title, 
                Quantity = quantityMap[x.Id]
            });
        var response = new CartResponse { CartItems = cartItemsResponse };

        return response;
    }

    public async Task RemoveFromCart(Guid userId, Guid bookId)
    {
        var cart = (await _cartRepo.FindAsync(x => x.UserId == userId)).First();
        if (cart.CartItems != null)
        {
            var itemToRemove = cart.CartItems.FirstOrDefault(x => x.BookId == bookId);

            if (itemToRemove != null)
            {
                cart.CartItems.Remove(itemToRemove);
                await _cartRepo.UpdateAsync(cart);
                await _cartItemRepo.DeleteAsync(itemToRemove);
                await _cartItemRepo.SaveChangesAsync();
            }
        }
    }
}
