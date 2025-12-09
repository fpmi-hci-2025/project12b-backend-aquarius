using Application.Dto.Request.Filters;
using Application.Dto.Request;
using Application.Dto.Response;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain;
using Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Unit;

public class OrderServiceTests
{
    private readonly Mock<IRepository<Order>> _mockOrderRepository;
    private readonly Mock<IRepository<Payment>> _mockPaymentRepository;
    private readonly Mock<IRepository<Book>> _mockBookRepository;
    private readonly Mock<IRepository<Cart>> _mockCartRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _mockOrderRepository = new Mock<IRepository<Order>>();
        _mockPaymentRepository = new Mock<IRepository<Payment>>();
        _mockBookRepository = new Mock<IRepository<Book>>();
        _mockCartRepository = new Mock<IRepository<Cart>>();
        _mockMapper = new Mock<IMapper>();
        _orderService = new OrderService(
            _mockOrderRepository.Object,
            _mockPaymentRepository.Object,
            _mockBookRepository.Object,
            _mockCartRepository.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task GetUserOrdersAsync_ValidUserId_ReturnsOrders()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var pagination = new Pagination { PageNumber = 1, PageSize = 10 };
        var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), UserId = userId, Status = "Pending" },
                new Order { Id = Guid.NewGuid(), UserId = userId, Status = "Paid" }
            };
        var orderResponses = new List<OrderResponse>
            {
                new OrderResponse { Status = "Pending" },
                new OrderResponse { Status = "Paid" }
            };

        _mockOrderRepository.Setup(x => x.GetPagedAsync(1, 10, It.IsAny<System.Linq.Expressions.Expression<System.Func<Order, bool>>>()))
            .ReturnsAsync(orders);
        _mockMapper.Setup(x => x.Map<IEnumerable<OrderResponse>>(orders))
            .Returns(orderResponses);

        // Act
        var result = await _orderService.GetUserOrdersAsync(userId, pagination);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockOrderRepository.Verify(x => x.GetPagedAsync(1, 10, It.Is<System.Linq.Expressions.Expression<System.Func<Order, bool>>>(expr =>
            expr.Compile()(new Order { UserId = userId }))), Times.Once);
    }

    [Fact]
    public async Task CreateOrderAsync_ValidRequest_CreatesOrder()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var request = new CreateOrderRequest
        {
            OrderItems = new List<CreateOrderRequest.OrderItem>
                {
                    new CreateOrderRequest.OrderItem { BookId = bookId, Count = 2 }
                },
            CustomerNotes = "Handle with care",
            DeliveryAddress = "123 Main St"
        };

        var book = new Book { Id = bookId, Title = "Test Book", Price = 10.0m, Quantity = 5 };
        var cart = new Cart
        {
            UserId = userId,
            CartItems = new List<CartItem>
                {
                    new CartItem { BookId = bookId, Quantity = 2 }
                }
        };

        _mockCartRepository.Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Cart, bool>>>()))
            .ReturnsAsync(new List<Cart> { cart });
        _mockBookRepository.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync(book);
        _mockBookRepository.Setup(x => x.UpdateAsync(book)).Returns(Task.FromResult(book));
        _mockBookRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));
        _mockCartRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));
        _mockOrderRepository.Setup(x => x.AddAsync(It.IsAny<Order>())).Returns(Task.FromResult(new Order()));
        _mockOrderRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));

        // Act
        await _orderService.CreateOrderAsync(userId, request);

        // Assert
        _mockBookRepository.Verify(x => x.UpdateAsync(It.Is<Book>(b => b.Quantity == 3)), Times.Once);
        _mockOrderRepository.Verify(x => x.AddAsync(It.Is<Order>(o =>
            o.UserId == userId &&
            o.Status == "Pending" &&
            o.OrderItems.Count == 1)), Times.Once);
        _mockOrderRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task PayOrderAsync_ValidPayment_ProcessesPayment()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var request = new PaymentRequest
        {
            OrderId = orderId,
            Amount = 20.0m,
            PaymentMethod = "CreditCard"
        };

        var order = new Order
        {
            Id = orderId,
            UserId = userId,
            Status = "Pending",
            OrderItems = new List<OrderItem>
                {
                    new OrderItem { BookId = Guid.NewGuid(), Quantity = 2, Book = new Book { Price = 10.0m } }
                }
        };

        _mockOrderRepository.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(order);
        _mockOrderRepository.Setup(x => x.UpdateAsync(order)).Returns(Task.CompletedTask);
        _mockOrderRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));
        _mockPaymentRepository.Setup(x => x.AddAsync(It.IsAny<Payment>())).Returns(Task.FromResult(new Payment()));
        _mockPaymentRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));

        // Act
        await _orderService.PayOrderAsync(userId, request);

        // Assert
        Assert.Equal("Paid", order.Status);
        Assert.NotNull(order.Payment);
        _mockPaymentRepository.Verify(x => x.AddAsync(It.Is<Payment>(p =>
            p.OrderId == orderId &&
            p.Amount == 20.0m &&
            !string.IsNullOrEmpty(p.TransactionNumber))), Times.Once);
    }
}
