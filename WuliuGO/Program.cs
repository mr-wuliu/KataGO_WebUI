using Microsoft.EntityFrameworkCore;
using WuliuGO.Services;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<GoGameService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoomService>();

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

app.UseSession();

// 配置HTTP请求管道
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); // 将请求映射到控制器
app.Run();
