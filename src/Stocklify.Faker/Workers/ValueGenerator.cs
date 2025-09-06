using Microsoft.Extensions.Options;
using Stocklify.Faker.Options;
using Stocklify.Faker.Services;

namespace Stocklify.Faker.Workers;

public class ValueGenerator : BackgroundService
{
    private bool _droppingStockChanges;

    private readonly StockBroadcaster _stockBroadcaster;
    private readonly StockContext _stockContext;
    private readonly ILogger<ValueGenerator> _logger;
    private readonly double _minChangeRate;
    private readonly double _maxChangeRate;
    
    public ValueGenerator
    (
        StockBroadcaster stockBroadcaster,
        StockContext stockContext,
        int minChangeRate,
        int maxChangeRate,
        ILogger<ValueGenerator> logger
    )
    {
        _stockBroadcaster = stockBroadcaster;
        _stockContext = stockContext;
        _minChangeRate = minChangeRate;
        _maxChangeRate = maxChangeRate;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        //Set the initial value of stocks...
        for (var i = 0; i < _stockContext.NumOfStocks; i++)
        {
            InitializeNewStock(i);
        }

        do
        {
            for (var i = 0; i < _stockContext.NumOfStocks; i++)
            {
                await UpdateRandomStockValueChange(i);
            }
            
            //Let´s just not kill the CPU entirely...
            await Task.Delay(10, stoppingToken);

        } while (!stoppingToken.IsCancellationRequested);
    }

    private void InitializeNewStock(int i)
    {
        _stockContext.Create(id: i, timestamp: DateTime.UtcNow.Ticks, value: GetRandomValueInRange(10.0, 100.0));
    }

    private async Task UpdateRandomStockValueChange(int i)
    {
        //...and then change it randomly.
        var changeHappened = GetRandomValueInRange(0.0, 1) > 0.95;
        if (!changeHappened)
            return;

        //Calculate the base-change of value for the stock.
        var change = GetRandomValueInRange(-2.0, 2.0);
        var changeShouldMultiply = GetRandomValueInRange(0.0, 10.0) > 8;
        if (changeShouldMultiply)
        {
            //Make the change a bit more extreme sometimes.
            var changeMultiplier = GetRandomValueInRange(0.1, 4.0);
            change *= changeMultiplier;
        }

        _stockContext.Update(i, timestamp: DateTime.UtcNow.Ticks, value: change);
        await BroadcastAsync(i);
    }

    private static double GetRandomValueInRange(double min, double max)
    {
        return Random.Shared.NextDouble() * (max - min) + min;
    }
    
    //Notice: Since we can't pass the stock struct by reference we provide the index of the stock to broadcast.
    private async Task BroadcastAsync(int index)
    {
        var changeRate = (int)GetRandomValueInRange(_minChangeRate, _maxChangeRate);
        
        if (changeRate != 0)
            await Task.Delay(changeRate);
        
        if (_stockBroadcaster.IsMaxCapacityReached() && !_droppingStockChanges)
        {
            //If the channel is full, drop stock changes until it's not and prevent flooding the log.
            _droppingStockChanges = true;
            _logger.LogWarning("Broadcast capacity reached. Dropping stock changes.");
        }

        if (_stockBroadcaster.IsMaxCapacityReached()) 
            return;
        
        //Re-enable dropping stock changes if the channel is no longer full.
        _droppingStockChanges = false;
        var stock = _stockContext.Get(index);
        _stockBroadcaster.Broadcast(ref stock);
    }
}