using Stocklify.StockFakeValueGenerator;
using Stocklify.StockFakeValueGenerator.Options;
using Stocklify.StockFakeValueGenerator.Services;
using Stocklify.StockFakeValueGenerator.Workers;

var builder = WebApplication.CreateBuilder();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddLogging();
builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection(ApplicationOptions.SectionName));
builder.Services.AddSingleton<StockContext>();
builder.Services.AddSingleton<StockBroadcaster>();
builder.Services.AddHostedService<ValueGenerator>();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapGrpcService<StockValuesGrpc>();
app.MapGrpcReflectionService();

app.Run();