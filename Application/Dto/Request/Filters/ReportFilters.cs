namespace Application.Dto.Request.Filters;

public class ReportFilters : BaseFilter
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? PublisherName { get; set; }
    public string? GenreName { get; set; }
    public string? AuthorName { get; set; }
}
