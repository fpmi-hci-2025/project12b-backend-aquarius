using Application.Contracts;
using Application.Dto.Request;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(
        IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Authorize(Roles = "User, Admin")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [EndpointDescription("Получает все заказы для текущего авторизованного пользователя. Обычные пользователи видят только свои заказы, администраторы видят все заказы.")]
    [EndpointSummary("Получить заказы пользователя")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetUserOrders([FromQuery] Pagination pagination)
    {
        var userId = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

        var result = await _orderService.GetUserOrdersAsync(userId, pagination);

        return Ok(result);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [EndpointDescription("Получает все заказы в системе с поддержкой пагинации и фильтрации. Доступно только для администраторов.")]
    [EndpointSummary("Получить все заказы")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAllOrders([FromQuery] Pagination pagination)
    {
        var result = await _orderService.GetAllOrdersAsync(pagination);

        return Ok(result);
    }

    [HttpGet("{orderId}/status")]
    [Authorize(Roles = "User, Admin")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Получает текущий статус конкретного заказа. Пользователи могут проверять статус только своих заказов, администраторы - любых заказов.")]
    [EndpointSummary("Получить статус заказа")]
    public async Task<ActionResult<string>> GetOrderStatus([FromRoute] Guid orderId)
    {
        var userId = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

        var result = await _orderService.GetOrderStatusAsync(userId, orderId);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [EndpointDescription("Создает новый заказ на основе товаров в корзине пользователя. После создания заказа корзина очищается.")]
    [EndpointSummary("Создать новый заказ")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var userId = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

        await _orderService.CreateOrderAsync(userId, request);

        return Ok();
    }

    [HttpPost("{orderId}/pay")]
    [Authorize(Roles = "User")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Выполняет оплату указанного заказа. Доступно только для владельца заказа.")]
    [EndpointSummary("Оплатить заказ")]
    public async Task<IActionResult> PayOrder([FromRoute] Guid orderId, [FromBody] PaymentRequest request)
    {
        var userId = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

        await _orderService.PayOrderAsync(userId, request);

        return Ok();
    }

    [HttpPut("{orderId}/cancel")]
    [Authorize(Roles = "User, Admin")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [EndpointDescription("Отменяет указанный заказ. Пользователи могут отменять только свои заказы, администраторы могут отменять любые заказы.")]
    [EndpointSummary("Отменить заказ")]
    public async Task<IActionResult> CancelOrder([FromRoute] Guid orderId)
    {
        var userId = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

        await _orderService.CancelOrderAsync(orderId, userId);

        return Ok();
    }
}
