using Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureSwaggerAuth();

        var connectionString = Environment.GetEnvironmentVariable("DB_CONN");
        builder.Services.AddDbContext<BookStoreDbContext>(options =>
            options.UseNpgsql(connectionString));

        var app = builder.Build();

        // Apply Migrations
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<BookStoreDbContext>();
            context.Database.Migrate();
        }

        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
