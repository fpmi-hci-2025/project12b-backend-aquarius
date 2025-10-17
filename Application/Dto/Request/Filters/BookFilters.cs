namespace Application.Dto.Request.Filters;

public class BookFilters : BaseFilter
{
    public string? Title { get; set; }
    public string? AuthorName { get; set; }
    public string? GenreName { get; set; }
    public string? PublisherName { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? PublicationYearFrom { get; set; }
    public int? PublicationYearTo { get; set; }
    public int? MinPageCount { get; set; }
    public int? MaxPageCount { get; set; }
    public bool? InStock { get; set; }
    public decimal? MinRating { get; set; }
}
