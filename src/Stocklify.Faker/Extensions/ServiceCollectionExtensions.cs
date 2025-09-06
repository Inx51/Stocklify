using System.Diagnostics.Metrics;
using Microsoft.Extensions.Options;
using Stocklify.Faker.Options;
using Stocklify.Faker.Services;
using Stocklify.Faker.Workers;

namespace Stocklify.Faker.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMeter(this IServiceCollection services, string meterName) => services.AddSingleton(new Meter(meterName));
    
    public static IServiceCollection AddStockContext(this IServiceCollection services) =>
        //NOTICE: Doing this "Options dance" since I don't like to pass the Options object to the constructor
        //since it hides the actual required parameters.
        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ApplicationOptions>>();
            return new StockContext(options.Value.NumberOfStocksInMarket, sp.GetRequiredService<Meter>());
        });
    
    public static IServiceCollection AddStockBroadcaster(this IServiceCollection services) =>
        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ApplicationOptions>>();
            return new StockBroadcaster
            (
                options.Value.BroadcastCapacity,
                sp.GetRequiredService<Meter>(),
                sp.GetRequiredService<ILogger<StockBroadcaster>>()
            );
        });
    
    public static IServiceCollection AddHostedServiceValueGenerator(this IServiceCollection services) =>
        services.AddHostedService(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ApplicationOptions>>();
            return new ValueGenerator
            (
                sp.GetRequiredService<StockBroadcaster>(),
                sp.GetRequiredService<StockContext>(),
                options.Value.MinChangeRate,
                options.Value.MaxChangeRate,
                sp.GetRequiredService<ILogger<ValueGenerator>>()   
            );
        });
}