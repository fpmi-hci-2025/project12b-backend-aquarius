using Application.Contracts;
using Application.Dto.Response;
using Application.Exceptions;
using AutoMapper;
using Domain;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class WishlistService : IWishlistService
{
    private readonly IRepository<Book> _bookRepository;
    private readonly IRepository<Wishlist> _wishlistRepository;
    private readonly IMapper _mapper;

    public WishlistService(
        IRepository<Book> bookRepository,
        IRepository<Wishlist> wishlistRepository,
        IMapper mapper)
    {
        _bookRepository = bookRepository;
        _wishlistRepository = wishlistRepository;
        _mapper = mapper;
    }

    public async Task AddToWishlist(Guid userId, Guid bookId)
    {
        var wishlist = (await _wishlistRepository.FindAsync(x => x.UserId == userId)).FirstOrDefault();

        if (wishlist == null)
            throw new NotFoundException($"Wishlist for user with id {userId} wasn't found");

        if (wishlist.Books.Any(x => x.Id == bookId))
            throw new BadRequestException($"Wishlist already contains book with id {bookId}");

        var book = await _bookRepository.GetByIdAsync(bookId);
        wishlist.Books.Add(book);

        await _wishlistRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<WishListItemResponse>> GetWishlistItems(Guid userId)
    {
        var wishlist = (await _wishlistRepository.FindAsync(x => x.UserId == userId)).FirstOrDefault();

        return _mapper.Map<IEnumerable<WishListItemResponse>>(wishlist.Books);
    }

    public async Task RemoveFromWishlist(Guid userId, Guid bookId)
    {
        var wishlist = (await _wishlistRepository.FindAsync(x => x.UserId == userId)).FirstOrDefault();

        if (wishlist == null)
            throw new NotFoundException($"Wishlist for user with id {userId} wasn't found");

        var bookToRemove = wishlist.Books.FirstOrDefault(x => x.Id == bookId);

        if (bookToRemove == null)
            throw new BadRequestException($"Wishlist contains no book with id {bookId}");

        wishlist.Books.Remove(bookToRemove);

        await _wishlistRepository.SaveChangesAsync();
    }
}
