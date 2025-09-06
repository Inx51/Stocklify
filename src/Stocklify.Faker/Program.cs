using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Stocklify.Faker.Extensions;
using Stocklify.Faker.Options;
using Stocklify.Faker.Services;

const string serviceName = "Stocklify.Faker";

var builder = WebApplication.CreateBuilder();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddLogging();
builder.Services.AddOpenTelemetry()
    .WithLogging(o =>
    {
        o.AddOtlpExporter();
    })
    .WithMetrics(o =>
    {
        o.AddOtlpExporter();
        o.AddMeter(serviceName);
    })
    .WithTracing(o =>
    {
        o.AddGrpcCoreInstrumentation();
        o.AddOtlpExporter();
    });
builder.Services.AddMeter(serviceName);

builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection(ApplicationOptions.SectionName));

builder.Services.AddStockContext()
    .AddStockBroadcaster()
    .AddHostedServiceValueGenerator()
    .AddGrpcReflection()
    .AddGrpc();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapGrpcService<StockValuesGrpc>();
app.MapGrpcReflectionService();

app.Run();