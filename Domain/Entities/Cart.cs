using Entities;
using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Cart : EntityBase
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ICollection<CartItem>? CartItems { get; set; }
}
