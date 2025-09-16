using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Stocklify.Faker.Extensions;
using Stocklify.Faker.Options;
using Stocklify.Faker.Services;

const string serviceName = "Stocklify.Faker";

var builder = WebApplication.CreateBuilder();
builder.Configuration.AddEnvironmentVariables();

// Configure Kestrel for HTTP/1.1 and HTTP/2 support in containers
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    });
});

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

// Don't use HTTPS redirection in container environment for gRPC
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapGrpcService<StockValuesGrpc>();
app.MapGrpcReflectionService();

app.Run();