namespace Application.Dto.Request.Filters;

public class ReviewFilters : Pagination
{
    public Guid BookId { get; set; }
    public DateTime? CreatedAtFrom { get; set; }
    public DateTime? CreatedAtTo { get; set; }
}
