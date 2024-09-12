using Microsoft.EntityFrameworkCore;
using WuliuGO.Models;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // PostgreSQL connection string
        optionsBuilder.UseNpgsql("Host=localhost;Database=wuliugo;Username=postgres;Password=mysecretpassword");
    }
}
