namespace Application.Dto.Response;

public class SalesReportResponse
{
    public ReportPeriod Period { get; set; }
    public List<SalesDataPoint> Data { get; set; }
    public decimal TotalSum { get; set; }
    public int TotalOrders { get; set; }
    public int TotalItemsSold { get; set; }
    public decimal AverageOrderValue { get; set; }
}

public class ReportPeriod
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class SalesDataPoint
{
    public DateTime Date { get; set; }
    public decimal Sum { get; set; }
    public int Orders { get; set; }
    public int ItemsSold { get; set; }
}