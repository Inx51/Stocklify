using System.Diagnostics.Metrics;
using Stocklify.Faker.ValueObjects;

namespace Stocklify.Faker;

public class StockContext
{
    public int NumOfStocks { get; private set; }
    
    private readonly Stock[] _stocks;
    
    public StockContext(int numberOfStocksInMarket, Meter meter)
    {
        _stocks = new Stock[numberOfStocksInMarket];
        NumOfStocks = numberOfStocksInMarket;
    }

    public void Create(int id, long timestamp, double value)
    {
        var stock = new Stock
        {
            Id = id,
            Timestamp = timestamp,
            Value = value
        };
        
        _stocks[id] = stock;
    }

    public void Update(int id, long timestamp, double change)
    {
        _stocks[id].Timestamp = timestamp;
        _stocks[id].Value += change;
        //Let´s not allow negative stock values.
        if(_stocks[id].Value < 0)
            _stocks[id].Value = 0;
    }
    
    public ref Stock Get(int id) => ref _stocks[id];
    
    public Stock[] GetCurrentState() => _stocks;
}