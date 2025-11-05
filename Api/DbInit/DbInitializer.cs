using Microsoft.EntityFrameworkCore;
using Persistence;
using Entities;

namespace Api.DbInit;

public static class DbInitializer
{
    public static async Task Initialize(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<BookStoreDbContext>();
            context.Database.Migrate();
        }

        using (var scope = app.Services.CreateScope())
        {
            var roles = new string[] { "User", "Admin" };
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<BookStoreDbContext>();

            var existingRoles = await context.Roles
                .Where(x => roles.Contains(x.Name))
                .ToListAsync();
            var rolesToAdd = roles.Except(existingRoles.Select(x => x.Name));

            await context.Roles.AddRangeAsync(
                rolesToAdd.Select(x => new Role { Name = x, Users = [] }).ToArray());
            await context.SaveChangesAsync();
        }
    }
}
