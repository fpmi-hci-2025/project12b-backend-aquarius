using Application.Dto.Request;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using Entities;

namespace Application.Contracts;

public interface IOrderService
{
    Task<IEnumerable<OrderResponse>> GetUserOrdersAsync(Guid userId, Pagination pagination);
    Task<IEnumerable<OrderResponse>> GetAllOrdersAsync(Pagination pagination);
    Task<string> GetOrderStatusAsync(Guid userId, Guid orderId);
    Task CreateOrderAsync(Guid userId, CreateOrderRequest request);
    Task PayOrderAsync(Guid userId, Guid orderId, PaymentRequest request);
    Task CancelOrderAsync(Guid orderId, Guid userId);
}