using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuestDB;
using QuestDB.Senders;
using Stocklify.Market.Factories;
using Stocklify.Market.Infrastructure.Database;
using Stocklify.Market.Infrastructure.Database.Repositories;
using Stocklify.Market.Infrastructure.Services;
using Stocklify.Market.Services;
using Stocklify.Market.Workers;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddSingleton<StockFactory>();
builder.Services.AddSingleton<ValueGenerator>();
builder.Services.AddSingleton<RandomGenerator>();
builder.Services.AddSingleton<ISenderV2>(_ =>  Sender.New("tcp::addr=localhost:9009"));
builder.Services.AddSingleton<StockTradeRepository>();
builder.Services.AddSingleton<TimeSeriesUnitOfWork>();

builder.Services.AddHttpClient<QuestDbHttpClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:9000");
});

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.Run();