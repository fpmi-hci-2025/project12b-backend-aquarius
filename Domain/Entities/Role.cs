using Entities.Base;

namespace Entities;

public class Role : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<User> Users { get; set; } = [];
}
