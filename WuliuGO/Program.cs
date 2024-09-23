using Microsoft.EntityFrameworkCore;
using WuliuGO.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// set logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
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

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSingleton<GoGameService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoomService>();

builder.Services.AddScoped<IKatagoRepository, KatagoRepository>();
builder.Services.AddSingleton<KatagoServer>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

var app = builder.Build();

// Timer 
app.UseMiddleware<RequestTimingMiddleware>();

app.UseSession();

// 配置HTTP请求管道
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); // 将请求映射到控制器
app.Run();
