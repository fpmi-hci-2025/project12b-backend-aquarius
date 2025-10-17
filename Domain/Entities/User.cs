using Domain.Entities;
using Entities.Base;

namespace Entities;

public class User : EntityBase
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public DateOnly? DateOfBirth { get; set; }

    public Guid CartId { get; set; }
    public Cart Cart { get; set; }

    public Guid? WishlistId { get; set; }
    public Wishlist? Wishlist { get; set; }

    public Guid TokensId { get; set; }
    public UserTokens Tokens { get; set; }

    public ICollection<Role> Roles { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Review> Reviews { get; set; }
}
