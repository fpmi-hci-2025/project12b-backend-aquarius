using Api.DbInit;
using Api.Extensions;
using Application.Contracts;
using Application.Mapper;
using Application.Services;
using Domain;
using Hellang.Middleware.ProblemDetails;
using Infrastructure.Auth.Contracts;
using Infrastructure.Auth.Services;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureSwaggerAuth();

        builder.Services.AddAuth(builder.Configuration);

        var connectionString = Environment.GetEnvironmentVariable("DB_CONN");
        builder.Services.AddDbContext<BookStoreDbContext>(options => 
        {
            options.UseNpgsql(connectionString);
            options.UseLazyLoadingProxies();
        });

        builder.Services.AddExceptionHandling(builder.Environment);

        builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(BookProfile).Assembly));

        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddScoped<ICartService, CartService>();
        builder.Services.AddScoped<IWishlistService, WishlistService>();
        builder.Services.AddScoped<IReviewService, ReviewService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IReportService, ReportService>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        var app = builder.Build();

        await DbInitializer.Initialize(app);

        app.UseProblemDetails();
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
