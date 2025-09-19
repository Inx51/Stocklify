using System.Text.RegularExpressions;
using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;

namespace Stocklify.Market.Hubs;

public class MarketHub : Hub
{
    private Channel<(string symbol, double value)> _channel = Channel.CreateUnbounded<(string symbol, double value)>();
    
    public Task<GetStocksResponse[]> GetStocks()
    {
        return Task.FromResult(new Stock[0]);
    }

    public Task<GetStocksHistoryResponse[]> GetStocksHistory(string[] symbols, DateTime from, DateTime to)
    {
        
    }
    
    public async Task SubscribeToStockValueChanges(string[] symbols)
    {
        foreach (var symbol in symbols)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, SubscriberGroupName(symbol));
        }
    }
    
    public async Task UnsubscribeToStockValueChanges(string[] symbols)
    {
        Clients.Group("").SendAsync("stockValueChanged", symbol, value, Context.ConnectionAborted);
        foreach (var symbol in symbols)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, SubscriberGroupName(symbol));
        }
    }
    
    private string SubscriberGroupName(string symbol) => $"subscribers_{symbol}";
}