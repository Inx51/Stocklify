using Scalar.AspNetCore;
using Stocklify.Faker.ProtocolGateway.Extensions;
using Stocklify.Faker.ProtocolGateway.Options;
using Stocklify.Faker.ProtocolGateway.SingalR;

var builder = WebApplication.CreateBuilder(args);
//Let´s only use environment variables for configuration.
builder.Configuration.Sources.Clear();
builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection(ApplicationOptions.SectionName));

builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddStockValueServiceGrpcClient();
builder.Services.AddHttpHandlers();
//In a production scenario, you would want to be more restrictive here.
const string corsPolicyName = "allow-dev-cors";
builder.Services.AddCors(o => o.AddPolicy(corsPolicyName, policy => policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

var app = builder.Build();

app.UseCors(corsPolicyName);
app.MapScalarApiReference();
app.MapOpenApi();
app.UseHttpsRedirection();
//Redirect to the scalar API reference.
app.MapGet("/", () => Results.Redirect("/scalar"))
    .ExcludeFromDescription();
app.MapGetStocks();
//Let's just add the Hub here... there is really no ceremony to be made here, so not that much "bloat".
app.MapHub<StockValueServiceHub>("hub/stockValueServiceHub");
app.Run();