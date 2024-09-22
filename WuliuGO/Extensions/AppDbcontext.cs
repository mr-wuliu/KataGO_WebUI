using Microsoft.EntityFrameworkCore;
using Serilog;
using WuliuGO.Models;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<KatagoQuery> KatagoQueries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        optionsBuilder
            .UseLoggerFactory(LoggerFactory.Create(builder => 
            {
                builder.AddSerilog();
            }))
            .EnableSensitiveDataLogging();
    
    }
}
