namespace Stocklify.Market.Entities;

public class Stock
{
    public string? Symbol { get; private set; }
    
    public string? CompanyName { get; private set; }

    //We could get this value from the buffer, but this prevents the need of a lookup.
    public double Value { get; private set; }

    //Let´s just use List here for now... we can change this later if we find this to be a bottleneck.
    private List<StockValue> _valuesBuffer = [];
    private readonly Lock _valuesLock = new();

    public Stock(string symbol, string companyName)
    {
        Symbol = symbol;
        CompanyName = companyName;
    }

    public void AppendUpdatedValue(double value, long timestamp)
    {
        lock (_valuesLock)
        {
            Value = value; 
            _valuesBuffer.Add(new StockValue(value, timestamp));
        }
    }

    public IEnumerable<StockValue> FlushValues()
    {
        lock(_valuesLock)
        {
            var flush = _valuesBuffer;
            _valuesBuffer = [];
            return flush;
        }
    }
    
    public struct StockValue
    {
        public double Value { get; set; }

        public long UpdateTimestamp { get; set; }
        
        public StockValue(double value, long updateTimestamp)
        {
            Value = value;
            UpdateTimestamp = updateTimestamp;
        }
    }
}