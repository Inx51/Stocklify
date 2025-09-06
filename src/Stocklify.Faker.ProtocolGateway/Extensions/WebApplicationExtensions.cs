using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Stocklify.Faker.ProtocolGateway.HttpHandlers;
using Stocklify.Faker.Services.Grpc;

namespace Stocklify.Faker.ProtocolGateway.Extensions;

public static class WebApplicationExtensions
{
    public static void MapGetStocks(this WebApplication app)
    {
        //Let's create a super simple mapping to get all the stocks using a more RESTful-like approach.
        //Since we only have one endpoint, I will not bother with groups etc...
        app.MapGet("/stocks", async ([FromServices]StockValueServiceGetStocks handler) => await handler.HandleAsync())
            .WithDisplayName("Get all stocks")
            .WithName("GetStocks")
            .WithTags("Stocks")
            .WithDescription("Returns a list of all stocks current values.");
    }
}