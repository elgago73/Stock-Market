using Microsoft.EntityFrameworkCore;
using StockMarket.Data;
using StockMarket.Data.Repositories;
using StockMarket.Service;
using StockMarket.Service.Contract;
using StockMarket.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IStockMarketService, StockMarketService>();
builder.Services.AddDbContext<StockMarketDbContext>(b => b.UseSqlServer("server=.\\sqlexpress;database=StockMarket;MultipleActiveResultSets=true;trusted_connection=true;encrypt=yes;trustservercertificate=yes;"));
builder.Services.AddScoped<IOrderReadRepository, OrderReadRepository>();
builder.Services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
builder.Services.AddSingleton<IStockMarketProcessorFactory, StockMarketProcessorFactroy>();
builder.Services.AddScoped<ITradeReadRepository, TradeReadRepository>();
builder.Services.AddScoped<ITradeWriteRepository, TradeWriteRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
