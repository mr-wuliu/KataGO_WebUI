
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WuliuGo.Services
{
    public class DesignTimeDContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext> 
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
            // 获取连接字符串
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            // 配置DbContext实现
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();   
            optionsBuilder.UseNpgsql(connectionString); 
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}