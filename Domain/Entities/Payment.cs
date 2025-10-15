using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;

public class Payment : EntityBase
{
    public string TransactionNumber { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}
