using Application.Contracts;
using Application.Dto.Request;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using Application.Exceptions;
using AutoMapper;
using Domain;
using Domain.Entities;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Payment> _paymentRepository;
    private readonly IRepository<Book> _bookRepository;
    private readonly IRepository<Cart> _cartRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IRepository<Order> orderRepository,
        IRepository<Payment> paymentRepository,
        IRepository<Book> bookRepository,
        IRepository<Cart> cartRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _bookRepository = bookRepository;
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderResponse>> GetUserOrdersAsync(Guid userId, Pagination pagination)
    {
        var orders = await _orderRepository.GetPagedAsync(
            pageNumber: pagination.PageNumber,
            pageSize: pagination.PageSize,
            predicate: o => o.UserId == userId
            );

        var ordersResponse = _mapper.Map<IEnumerable<OrderResponse>>(orders);

        return ordersResponse;
    }

    public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync(Pagination pagination)
    {
        var orders = await _orderRepository.GetPagedAsync(
            pageNumber: pagination.PageNumber,
            pageSize: pagination.PageSize,
            predicate: x => true
            );

        var ordersResponse = _mapper.Map<List<OrderResponse>>(orders);
        
        return ordersResponse;
    }

    public async Task<string> GetOrderStatusAsync(Guid userId, Guid orderId)
    {
        var order = (await _orderRepository.FindAsync(
            o => o.Id == orderId
            ))
            .FirstOrDefault();

        if (order == null)
            throw new NotFoundException($"Order {orderId} was not found");

        if (order.UserId != userId)
            throw new ForbiddenException($"You have no permission to access order {orderId}");

        return order.Status;
    }

    public async Task CreateOrderAsync(Guid userId, CreateOrderRequest request)
    {
        decimal totalAmount = 0;
        var orderId = Guid.NewGuid();
        var orderItems = new List<OrderItem>();

        var userCart = (await _cartRepository.FindAsync(x => x.UserId == userId)).FirstOrDefault();

        foreach (var item in request.OrderItems)
        {
            var userCartItem = userCart.CartItems?.FirstOrDefault(x => x.BookId == item.BookId);
            if (userCart == null || userCartItem == null)
            {
                throw new BadRequestException($"Book {item.BookId} was not found in cart of user {userId}");
            }

            if (userCartItem.Quantity < item.Count)
            {
                throw new BadRequestException($"User {userId} has {userCartItem.Quantity} items for book {item.BookId} in cart but {item.Count} in request");
            }

            var book = await _bookRepository.GetByIdAsync(item.BookId);
            if (book == null)
            {
                throw new NotFoundException($"Book {item.BookId} was not found");
            }

            if (book.Quantity < item.Count)
            {
                throw new BadRequestException($"Insufficient quantity for book {book.Title}");
            }

            book.Quantity -= item.Count;
            userCartItem.Quantity -= item.Count;

            if (userCartItem.Quantity == 0)
            {
                userCart.CartItems.Remove(userCartItem);
                await _cartRepository.UpdateAsync(userCart);
            }

            await _bookRepository.UpdateAsync(book);

            totalAmount += book.Price * item.Count;

            orderItems.Add(new OrderItem
            {
                BookId = item.BookId,
                Quantity = item.Count,
                OrderId = orderId,
            });
        }

        await _bookRepository.SaveChangesAsync();
        await _cartRepository.SaveChangesAsync();

        var order = new Order
        {
            Id = orderId,
            UserId = userId,
            CustomerNotes = request.CustomerNotes,
            DeliveryAddress = request.DeliveryAddress,
            Status = "Pending",
            OrderItems = orderItems
        };

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();
    }

    public async Task PayOrderAsync(Guid userId, Guid orderId, PaymentRequest request)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            throw new NotFoundException($"Order {orderId} was not found");
        }

        if (order.UserId != userId)
        {
            throw new ForbiddenException($"Order {orderId} doesn't belong to user {userId}");
        }

        if (order.Status != "Pending")
        {
            throw new BadRequestException($"Cannot pay for order with status '{order.Status}'");
        }

        if (order.OrderItems.Sum(oi => oi.Quantity * oi.Book.Price) != request.Amount)
        {
            throw new BadRequestException($"Order price and request price do not match");
        }

        var payment = new Payment
        {
            OrderId = orderId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            TransactionNumber = Guid.NewGuid().ToString()
        };

        order.Payment = payment;
        order.Status = "Paid";

        await _paymentRepository.AddAsync(payment);
        await _paymentRepository.SaveChangesAsync();

        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveChangesAsync();
    }

    public async Task CancelOrderAsync(Guid orderId, Guid userId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            throw new KeyNotFoundException($"Order {orderId} was not found");
        }

        if (order.UserId != userId)
        {
            throw new ForbiddenException($"Order {orderId} doesn't belong to user {userId}");
        }

        if (order.Status == "Cancelled" || order.Status == "Paid")
        {
            throw new InvalidOperationException($"Cannot cancel order with status '{order.Status}'");
        }

        foreach (var orderItem in order.OrderItems)
        {
            var book = orderItem.Book;
            book.Quantity += orderItem.Quantity;
            await _bookRepository.UpdateAsync(book);
        }

        await _bookRepository.SaveChangesAsync();

        order.Status = "Cancelled";
        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveChangesAsync();
    }
}
