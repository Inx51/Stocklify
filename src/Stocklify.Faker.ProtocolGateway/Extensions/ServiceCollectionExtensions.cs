using Microsoft.Extensions.Options;
using Stocklify.Faker.ProtocolGateway.HttpHandlers;
using Stocklify.Faker.ProtocolGateway.Options;
using Stocklify.Faker.Services.Grpc;

namespace Stocklify.Faker.ProtocolGateway.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStockValueServiceGrpcClient(this IServiceCollection services)
    {
        services.AddGrpcClient<StockValueService.StockValueServiceClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<ApplicationOptions>>();
            client.Address = new Uri(options.Value.StockValueServiceClientEndpoint!);
        });

        return services;
    }
    
    public static IServiceCollection AddHttpHandlers(this IServiceCollection services)
    {
        services.AddSingleton<StockValueServiceGetStocks>();

        return services;
    }
}