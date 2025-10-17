using Domain.Entities;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class BookStoreDbContext : DbContext
{

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PickupAddress> PickupAddresses { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }

    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cart - User (One-to-One)
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithOne(u => u.Cart)
            .HasForeignKey<Cart>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Cart - CartItem (One-to-Many)
        modelBuilder.Entity<CartItem>()
            .HasOne(x => x.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(c => c.CartId)
            .OnDelete(DeleteBehavior.Restrict);

        // Book - Publisher (One-to-Many)
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Publisher)
            .WithMany(p => p.Books)
            .HasForeignKey(b => b.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);

        // Book - Author (Many-to-Many)
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Authors)
            .WithMany(a => a.Books)
            .UsingEntity<Dictionary<string, object>>(
                "Book-Author",
                j => j.HasOne<Author>().WithMany().HasForeignKey("AuthorId"),
                j => j.HasOne<Book>().WithMany().HasForeignKey("BookId"),
                j => j.HasKey("BookId", "AuthorId"));

        // Book - Genre (Many-to-Many)
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Genres)
            .WithMany(g => g.Books)
            .UsingEntity<Dictionary<string, object>>(
                "Book-Genre",
                j => j.HasOne<Genre>().WithMany().HasForeignKey("GenreId"),
                j => j.HasOne<Book>().WithMany().HasForeignKey("BookId"),
                j => j.HasKey("BookId", "GenreId"));

        // Book - Review (One-to-Many)
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Book)
            .WithMany(b => b.Reviews)
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        // User - Review (One-to-Many)
        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // User - Order (One-to-Many)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Order - Payment (One-to-One)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Order - OrderItem (One-to-Many)
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Book - OrderItem (One-to-Many)
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Book)
            .WithMany()
            .HasForeignKey(oi => oi.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        // Order - ShippingAddress (One-to-Many)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.ShippingAddress)
            .WithMany(x => x.Orders)
            .HasForeignKey(o => o.ShippingAddressId)
            .OnDelete(DeleteBehavior.Restrict);

        // User - Wishlist (One-to-One)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Wishlist)
            .WithOne(w => w.User)
            .HasForeignKey<Wishlist>(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Wishlist - Book (Many-to-Many)
        modelBuilder.Entity<Wishlist>()
            .HasMany(w => w.Books)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "Wishlist-Book",
                j => j.HasOne<Book>().WithMany().HasForeignKey("BookId"),
                j => j.HasOne<Wishlist>().WithMany().HasForeignKey("WishlistId"),
                j => j.HasKey("WishlistId", "BookId"));

        // User - UserTokens (One-to-One)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Tokens)
            .WithOne()
            .HasForeignKey<UserTokens>(ut => ut.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // User - Role (Many-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<Dictionary<string, object>>(
                "User-Role",
                j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                j => j.HasKey("UserId", "RoleId"));

        // Unique constraints
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Review>()
            .HasIndex(r => new { r.BookId, r.UserId })
            .IsUnique();

        modelBuilder.Entity<Payment>()
            .HasIndex(p => p.TransactionNumber)
            .IsUnique();

        // Configure decimal precision for Price and Amount
        modelBuilder.Entity<Book>()
            .Property(b => b.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(18, 2);

        // Configure string lengths
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(255);

        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .HasMaxLength(500);

        modelBuilder.Entity<Book>()
            .Property(b => b.ISBN)
            .HasMaxLength(20);

        modelBuilder.Entity<Book>()
            .Property(b => b.Title)
            .HasMaxLength(500);

        modelBuilder.Entity<Payment>()
            .Property(p => p.TransactionNumber)
            .HasMaxLength(100);

        modelBuilder.Entity<Payment>()
            .Property(p => p.PaymentMethod)
            .HasMaxLength(50);

        modelBuilder.Entity<PickupAddress>()
            .Property(pa => pa.AddressLine1)
            .HasMaxLength(255);

        modelBuilder.Entity<PickupAddress>()
            .Property(pa => pa.City)
            .HasMaxLength(100);

        modelBuilder.Entity<PickupAddress>()
            .Property(pa => pa.Country)
            .HasMaxLength(100);

        modelBuilder.Entity<Publisher>()
            .Property(p => p.Name)
            .HasMaxLength(255);

        modelBuilder.Entity<Publisher>()
            .Property(p => p.ContactEmail)
            .HasMaxLength(255);

        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .HasMaxLength(50);

        modelBuilder.Entity<UserTokens>()
            .Property(ut => ut.RefreshToken)
            .HasMaxLength(500);

        // Configure required properties
        modelBuilder.Entity<Author>()
            .Property(a => a.FirstName)
            .IsRequired();

        modelBuilder.Entity<Author>()
            .Property(a => a.LastName)
            .IsRequired();

        modelBuilder.Entity<Book>()
            .Property(b => b.Title)
            .IsRequired();

        modelBuilder.Entity<Book>()
            .Property(b => b.Price)
            .IsRequired();

        modelBuilder.Entity<Genre>()
            .Property(g => g.Name)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .IsRequired();

        modelBuilder.Entity<Payment>()
            .Property(p => p.PaymentMethod)
            .IsRequired();

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .IsRequired();

        modelBuilder.Entity<PickupAddress>()
            .Property(pa => pa.AddressLine1)
            .IsRequired();

        modelBuilder.Entity<PickupAddress>()
            .Property(pa => pa.City)
            .IsRequired();

        modelBuilder.Entity<PickupAddress>()
            .Property(pa => pa.Country)
            .IsRequired();

        modelBuilder.Entity<Publisher>()
            .Property(p => p.Name)
            .IsRequired();

        modelBuilder.Entity<Review>()
            .Property(r => r.Rating)
            .IsRequired();

        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .IsRequired();
    }
}
