using Domain.Entities;
using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities;

public class User : EntityBase
{
    [EmailAddress]
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [Phone]
    public string? Phone { get; set; }
    public DateOnly? DateOfBirth { get; set; }

    public Guid CartId { get; set; }
    public virtual Cart Cart { get; set; }

    public Guid WishlistId { get; set; }
    public virtual Wishlist Wishlist { get; set; }

    public Guid TokensId { get; set; }
    public virtual UserTokens Tokens { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = [];
    public virtual ICollection<Order> Orders { get; set; } = [];
    public virtual ICollection<Review> Reviews { get; set; } = [];
}
