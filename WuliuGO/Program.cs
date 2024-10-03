using Microsoft.EntityFrameworkCore;
using WuliuGO.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// set logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    // 临时显示到控制台
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/wuliugo-.log",
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true,
        retainedFileCountLimit: 10
        ) // 主日志文件
    .CreateLogger();

builder.Host.UseSerilog();

// Use Newtonsoft.Json as Default JsonSerilize Tool
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
    });

// Connect pg
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 添加服务到容器
builder.Services.AddHttpContextAccessor();

// Register GoGameService
// ASP.NET Core 依赖注入的三种生命周期
// 1. Transient：每次请求都会创建一个新的实例。
// 2. Scoped: 每一个Http请求会创建一个新的实例，但会在同一个请求中复用这个实例。
// 3. Singleton: 整个应用程序运行期间只会创建一个实例。'


builder.Services.AddScoped<GoGameService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IKatagoRepository, KatagoRepository>();

// 单例, 保证一致性
builder.Services.AddSingleton<CacheService>();
// 单例, 但同时添加到后台服务自启后台服务
builder.Services.AddSingleton<IKatagoServer, KatagoServer>();
builder.Services.AddSingleton(provider =>
    (IHostedService)provider.GetRequiredService<IKatagoServer>());


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport();

// 和Session有关的
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession( options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
}
);
// 添加视图
builder.Services.AddControllersWithViews();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// Timer 
app.UseMiddleware<RequestTimingMiddleware>();
app.UseStaticFiles();  // 确保启用静态文件支持

app.UseSession();
app.UseCors("AllowAllOrigins");

// 配置HTTP请求管道
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); // 将请求映射到控制器
app.Run();
