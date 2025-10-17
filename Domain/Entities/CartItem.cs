using Entities;
using Entities.Base;

namespace Domain.Entities;

public class CartItem : EntityBase
{
    public Guid CartId { get; set; }
    public Cart Cart { get; set; }
    public ICollection<Book>? Books { get; set; }
}
