using Microsoft.EntityFrameworkCore;
using Serilog;
using WuliuGO.Models;
using System.ComponentModel.DataAnnotations.Schema;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Analysis> AnalysisQuery { get; set; }
    public DbSet<FriendVex> FriendVexes { get; set; }
    public DbSet<FriendEdge> FriendEdges { get; set; }

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
