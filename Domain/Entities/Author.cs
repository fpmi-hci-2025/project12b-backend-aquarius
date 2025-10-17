using Entities.Base;

namespace Entities;

public class Author : EntityBase
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public ICollection<Book> Books { get; set; }
}
