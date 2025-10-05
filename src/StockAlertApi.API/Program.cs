using Microsoft.EntityFrameworkCore;
using StockAlertApi.Application.Services;
using StockAlertApi.Core.Interfaces.Repositories;
using StockAlertApi.Core.Interfaces.Services;
using StockAlertApi.Infrastructure.Data;
using StockAlertApi.Infrastructure.Data.Repositories;
using StockAlertApi.Infrastructure.ExternalApis;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IAlertsService, AlertsService>();
builder.Services.AddScoped<IStocksService, StocksService>();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();

// External APIs
builder.Services.AddHttpClient<IFinanceApiService, FinnhubService>();

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();