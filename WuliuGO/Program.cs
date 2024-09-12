using Microsoft.EntityFrameworkCore;
using WuliuGO.Services;

var builder = WebApplication.CreateBuilder(args);

// Use Newtonsoft.Json as Default JsonSerilize Tool
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        // options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
    });

// Connect pg
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register GoGameService
builder.Services.AddSingleton<GoGameService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); // 将请求映射到控制器
app.Run();
