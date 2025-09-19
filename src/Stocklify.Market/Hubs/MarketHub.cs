namespace Stocklify.Market.Hubs;

public class MarketHub
{
    public Task<Stock[]> GetStocks()
    {

    }

    public Task<Stock[]> GetStocksHistory(string[] symbols, DateTime from, DateTime to)
    {
        
    }
    
    public void SubscribeToStockValueChanges(string[] symbols)
    {
        
    }
    
    public void UnsubscribeToStockValueChanges(string[] symbols)
    {
        
    }
}