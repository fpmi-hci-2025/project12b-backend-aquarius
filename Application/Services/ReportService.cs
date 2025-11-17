using Application.Contracts;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using Domain;
using Entities;
using System.Linq.Expressions;

namespace Application.Services;

public class ReportService : IReportService
{
    private readonly IRepository<Order> _orderRepository;

    public ReportService(IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<SalesReportResponse> GenerateSalesReportAsync(ReportFilters filters)
    {
        var predicate = BuildSalesReportPredicate(filters);

        var orders = await _orderRepository.FindAsync(predicate);
        var ordersList = orders.ToList();

        if (!ordersList.Any())
        {
            return CreateEmptyReport(filters);
        }

        var dataPoints = GenerateSalesDataPoints(ordersList, filters);
        var summary = CalculateSummaryStatistics(ordersList);

        return new SalesReportResponse
        {
            Period = new ReportPeriod
            {
                StartDate = filters.StartDate ?? ordersList.Min(o => o.CreatedAt).Date,
                EndDate = filters.EndDate ?? ordersList.Max(o => o.CreatedAt).Date
            },
            Data = dataPoints,
            TotalSum = summary.TotalSum,
            TotalOrders = summary.TotalOrders,
            TotalItemsSold = summary.TotalItemsSold,
            AverageOrderValue = summary.AverageOrderValue
        };
    }

    private Expression<Func<Order, bool>> BuildSalesReportPredicate(ReportFilters filters)
    {
        Expression<Func<Order, bool>> predicate = o => o.Status == "Paid";

        if (filters.StartDate.HasValue)
        {
            var startDate = filters.StartDate.Value.Date;
            predicate = AndAlso(predicate, o => o.CreatedAt >= startDate);
        }

        if (filters.EndDate.HasValue)
        {
            var endDate = filters.EndDate.Value.Date;
            predicate = AndAlso(predicate, o => o.CreatedAt <= endDate);
        }

        if (!string.IsNullOrWhiteSpace(filters.PublisherName))
        {
            predicate = AndAlso(predicate, o => o.OrderItems.Any(oi =>
                oi.Book != null &&
                !string.IsNullOrEmpty(oi.Book.Publisher) &&
                oi.Book.Publisher.Contains(filters.PublisherName.Trim())));
        }

        if (!string.IsNullOrWhiteSpace(filters.GenreName))
        {
            var genreFilter = filters.GenreName.Trim();
            predicate = AndAlso(predicate, o => o.OrderItems.Any(oi =>
                oi.Book != null &&
                oi.Book.Genres != null &&
                oi.Book.Genres.Any(g => !string.IsNullOrEmpty(g) && g.Contains(genreFilter))));
        }

        if (!string.IsNullOrWhiteSpace(filters.AuthorName))
        {
            var authorFilter = filters.AuthorName.Trim();
            predicate = AndAlso(predicate, o => o.OrderItems.Any(oi =>
                oi.Book != null &&
                oi.Book.Authors != null &&
                oi.Book.Authors.Any(a => !string.IsNullOrEmpty(a) && a.Contains(authorFilter))));
        }

        return predicate;
    }

    private Expression<Func<Order, bool>> AndAlso(Expression<Func<Order, bool>> expr1, Expression<Func<Order, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(Order));
        var combined = Expression.AndAlso(
            Expression.Invoke(expr1, parameter),
            Expression.Invoke(expr2, parameter)
        );
        return Expression.Lambda<Func<Order, bool>>(combined, parameter);
    }

    private List<SalesDataPoint> GenerateSalesDataPoints(List<Order> orders, ReportFilters filters)
    {
        var startDate = filters.StartDate ?? orders.Min(o => o.CreatedAt).Date;
        var endDate = filters.EndDate ?? orders.Max(o => o.CreatedAt).Date;

        var allDates = new List<DateTime>();
        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            allDates.Add(date);
        }

        var ordersByDate = orders
            .GroupBy(o => o.CreatedAt.Date)
            .ToDictionary(g => g.Key, g => g.ToList());

        return allDates.Select(date =>
        {
            if (ordersByDate.TryGetValue(date, out var dateOrders))
            {
                var orderItems = dateOrders.SelectMany(o => o.OrderItems).ToList();
                return new SalesDataPoint
                {
                    Date = date,
                    Sum = orderItems.Sum(oi => oi.Book.Price * oi.Quantity),
                    Orders = dateOrders.Count,
                    ItemsSold = orderItems.Sum(oi => oi.Quantity)
                };
            }
            else
            {
                return new SalesDataPoint
                {
                    Date = date,
                    Sum = 0,
                    Orders = 0,
                    ItemsSold = 0
                };
            }
        })
        .OrderBy(d => d.Date)
        .ToList();
    }

    private (decimal TotalSum, int TotalOrders, int TotalItemsSold, decimal AverageOrderValue)
        CalculateSummaryStatistics(List<Order> orders)
    {
        var totalOrders = orders.Count;
        var allOrderItems = orders.SelectMany(o => o.OrderItems).ToList();
        var totalSum = allOrderItems.Sum(oi => oi.Book.Price * oi.Quantity);
        var totalItemsSold = allOrderItems.Sum(oi => oi.Quantity);
        var averageOrderValue = totalOrders > 0 ? totalSum / totalOrders : 0;

        return (totalSum, totalOrders, totalItemsSold, averageOrderValue);
    }

    private SalesReportResponse CreateEmptyReport(ReportFilters filters)
    {
        var startDate = filters.StartDate ?? DateTime.UtcNow.Date;
        var endDate = filters.EndDate ?? DateTime.UtcNow.Date;

        var dataPoints = new List<SalesDataPoint>();
        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            dataPoints.Add(new SalesDataPoint
            {
                Date = date,
                Sum = 0,
                Orders = 0,
                ItemsSold = 0
            });
        }

        return new SalesReportResponse
        {
            Period = new ReportPeriod
            {
                StartDate = startDate,
                EndDate = endDate
            },
            Data = dataPoints,
            TotalSum = 0,
            TotalOrders = 0,
            TotalItemsSold = 0,
            AverageOrderValue = 0
        };
    }
}