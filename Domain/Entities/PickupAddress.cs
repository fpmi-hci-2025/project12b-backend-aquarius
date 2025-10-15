using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;

public class PickupAddress : EntityBase
{
    public string AddressLine1 { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public ICollection<Order> Orders { get; set; }
}
