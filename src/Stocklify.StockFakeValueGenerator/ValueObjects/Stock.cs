namespace Stocklify.StockFakeValueGenerator.ValueObjects;

public struct Stock
{
    public int Id { get; init; }
    
    public double Value { get; set; }

    public long Timestamp { get; set; }
}