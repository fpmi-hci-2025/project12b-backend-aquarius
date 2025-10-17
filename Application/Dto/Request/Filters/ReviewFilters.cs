namespace Application.Dto.Request.Filters;

public class ReviewFilters : BaseFilter
{
    public Guid? BookId { get; set; }
    public Guid? UserId { get; set; }
    public DateTime? CreatedAtFrom { get; set; }
    public DateTime? CreatedAtTo { get; set; }
}
