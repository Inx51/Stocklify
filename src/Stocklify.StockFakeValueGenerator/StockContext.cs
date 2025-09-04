using Microsoft.Extensions.Options;
using Stocklify.StockFakeValueGenerator.Options;
using Stocklify.StockFakeValueGenerator.ValueObjects;

namespace Stocklify.StockFakeValueGenerator;

public class StockContext
{
    public int NumOfStocks { get; private set; }
    
    private readonly Stock[] _stocks;

    public StockContext(IOptions<ApplicationOptions> options)
    {
        _stocks = new Stock[options.Value.NumberOfStocksInMarket];
        NumOfStocks = options.Value.NumberOfStocksInMarket;
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

    public void Update(int id, long timestamp, double value)
    {
        _stocks[id].Timestamp = timestamp;
        _stocks[id].Value += value;
        //Let´s not allow negative stock values.
        if(_stocks[id].Value < 0)
            _stocks[id].Value = 0;
    }
    
    public ref Stock Get(int id) => ref _stocks[id];
    
    public Stock[] GetCurrentState() => _stocks;
}