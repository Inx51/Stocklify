using Microsoft.Extensions.Hosting;
using Stocklify.Market.Entities;
using Stocklify.Market.Factories;
using Stocklify.Market.Infrastructure.Database;
using Stocklify.Market.Services;

namespace Stocklify.Market.Workers;

public class Worker : BackgroundService
{
    private readonly int _numberOfStocks = 100;
    private readonly int _tickInterval = 200;
    private readonly StockFactory _stockFactory;
    private readonly ValueGenerator _valueGenerator;
    private readonly TimeSeriesUnitOfWork _timeSeriesUnitOfWork;
    private readonly Stock[] _stocks;
    
    public Worker
    (
        StockFactory stockFactory,
        ValueGenerator valueGenerator,
        TimeSeriesUnitOfWork timeSeriesUnitOfWork
    )
    {
        _stockFactory = stockFactory;
        _valueGenerator = valueGenerator;
        _timeSeriesUnitOfWork = timeSeriesUnitOfWork;
        _stocks = new Stock[_numberOfStocks];
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await CleanupAsync();
        GenerateStocks();
        StartValueGenerationTick(stoppingToken);
        PersistChangesTick(stoppingToken);
    }

    private async Task CleanupAsync()
    {
        if(await _timeSeriesUnitOfWork.StockTrades.ExistsAsync())
            await _timeSeriesUnitOfWork.StockTrades.DeleteAllAsync();
    }

    private void PersistChangesTick(CancellationToken stoppingToken)
    {
        _ = Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var stock in _stocks)
                {
                    var updates = stock.FlushValues();
                    foreach (var update in updates)
                    {
                        await _timeSeriesUnitOfWork.StockTrades.AddAsync
                        (
                          stock.Symbol!,
                          update.Value,
                          update.UpdateTimestamp,
                          stoppingToken
                        );
                    }
                }

                await _timeSeriesUnitOfWork.CommitAsync(stoppingToken);
                await Task.Delay(600, stoppingToken);
            }
        }, stoppingToken);
    }

    private void StartValueGenerationTick(CancellationToken stoppingToken)
    {
        _ = Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var stock in _stocks)
                {
                    var nextValue = _valueGenerator.GenerateNextValue(stock.Value);
                    
                    stock.AppendUpdatedValue(nextValue, UnixTimestamp.UtcNowInNanoseconds);
                }

                await Task.Delay(_tickInterval, stoppingToken);
            }
        }, stoppingToken);
    }

    private void GenerateStocks()
    {
        for (var i = 0; i < _stocks.Length; i++)
        {
            var stock = _stockFactory.CreateStock();
            var value = _valueGenerator.GenerateRandomValue(1, 300);
            stock.AppendUpdatedValue(value, UnixTimestamp.UtcNowInNanoseconds);
            _stocks[i] = stock;
        }
    }
}