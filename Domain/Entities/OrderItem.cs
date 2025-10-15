using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;

public class OrderItem : EntityBase
{
    public int Quantity { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    public Guid BookId { get; set; }
    public Book Book { get; set; }
}
