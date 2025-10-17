namespace Application.Dto.Request.Filters;

public class UserFilters : BaseFilter
{
    public DateTime? DateOfBirthFrom { get; set; }
    public DateTime? DateOfBirthTo { get; set; }
    public DateTime? CreatedAtFrom { get; set; }
    public DateTime? CreatedAtTo { get; set; }
    public string? RoleName { get; set; }
    public bool? HasOrders { get; set; }
    public bool? HasReviews { get; set; }
}
