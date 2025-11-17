using Microsoft.EntityFrameworkCore;
using Persistence;
using Entities;

namespace Api.DbInit;

public static class DbInitializer
{
    public static async Task Initialize(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        // Apply migrations
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<BookStoreDbContext>();
        context.Database.Migrate();

        // Seed roles
        var roles = new string[] { "User", "Admin" };
        var existingRoles = await context.Roles
            .Where(x => roles.Contains(x.Name))
            .ToListAsync();
        var rolesToAdd = roles.Except(existingRoles.Select(x => x.Name));

        await context.Roles.AddRangeAsync(
            rolesToAdd.Select(x => new Role { Name = x, Users = [] }).ToArray());
        await context.SaveChangesAsync();

        // Seed admin
        var adminExists = (await context.Roles
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Name == "Admin")
            ).Users.Count != 0;

        if (!adminExists)
        {
            var email = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
            var password = Environment.GetEnvironmentVariable("ADMIN_PASS");

            var admin = new User
            {
                CreatedAt = DateTime.UtcNow,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password),
                Tokens = new UserTokens(),
                Roles = [],
            };
            context.Users.Add(admin);

            var adminRole = await context.Roles.Include(x => x.Users).FirstAsync(x => x.Name == "Admin");
            adminRole.Users.Add(admin);
            admin.Roles.Add(adminRole);

            await context.SaveChangesAsync();
        }
    }
}
