using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;

public class Wishlist : EntityBase
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ICollection<Book>? Books { get; set; }
}
