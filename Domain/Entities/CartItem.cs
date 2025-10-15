using Entities;
using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class CartItem : EntityBase
{
    public Guid CartId { get; set; } 
    public Cart Cart { get; set; }
    public ICollection<Book>? Books { get; set; }
}
