using Entities.Base;

namespace Entities;

public class PickupAddress : EntityBase
{
    public string AddressLine1 { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public ICollection<Order> Orders { get; set; }
}
