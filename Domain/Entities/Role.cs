using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;

public class Role : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; }
}
