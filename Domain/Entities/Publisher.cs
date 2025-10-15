using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;

public class Publisher : EntityBase
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ContactEmail { get; set; }

    public ICollection<Book> Books { get; set; }
}
