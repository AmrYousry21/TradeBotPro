using Microsoft.EntityFrameworkCore;
using TradeBotPro.App.Models.DataModels;

namespace TradeBotPro.App;

public class DatabaseContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DatabaseContext() : base() { }
    public DatabaseContext(DbContextOptions options) : base(options) { }
    public DatabaseContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Closure> Closures { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(_configuration.GetConnectionString("TradeBotProDatabase"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>()
            .Property(e => e.Status)
            .HasConversion<int>();

        modelBuilder.Entity<Client>()
            .Navigation(e => e.Users)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        modelBuilder.Entity<User>()
            .HasOne(e => e.Client)
            .WithMany(e => e.Users);

        modelBuilder.Entity<User>()
            .Property(e => e.Status)
            .HasConversion<int>();

        modelBuilder.Entity<User>()
            .Property(e => e.UserRole)
            .HasConversion<int>();

        modelBuilder.Entity<Order>()
            .HasOne(e => e.User);

        modelBuilder.Entity<Closure>()
            .HasOne(e => e.User);
    }
}