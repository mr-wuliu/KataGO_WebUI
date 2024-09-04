using Microsoft.EntityFrameworkCore;

namespace WuliuGo.Services
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : DbContext(options)
    {
        // 定义数据表
        public DbSet<User>? Users { get; set; }
    }
}