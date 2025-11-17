using Application.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts;

public interface IWishlistService
{
    Task<IEnumerable<WishListItemResponse>> GetWishlistItems(Guid userId);

    Task AddToWishlist(Guid userId, Guid bookId);

    Task RemoveFromWishlist(Guid userId, Guid bookId);
}
