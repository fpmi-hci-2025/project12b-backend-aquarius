using Entities.Base;

namespace Entities;

public class Genre : EntityBase
{
    public string Name { get; set; }

    public ICollection<Book>? Books { get; set; }
}
