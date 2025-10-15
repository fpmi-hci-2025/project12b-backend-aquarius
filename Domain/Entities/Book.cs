using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities;

public class Book : EntityBase
{
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? PublicationYear { get; set; }
    public int? PageCount { get; set; }
    public int? Weight { get; set; }
    public decimal Price { get; set; }
    public byte[]? CoverImageUrl { get; set; }
    public int Quantity { get; set; }


    public Guid PublisherId { get; set; }
    public Publisher Publisher { get; set; }
    public ICollection<Author> Authors { get; set; }
    public ICollection<Genre> Genres { get; set; }
    public ICollection<Review> Reviews { get; set; }
}
