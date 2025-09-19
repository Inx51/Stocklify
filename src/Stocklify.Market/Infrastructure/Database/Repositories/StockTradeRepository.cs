using QuestDB.Senders;
using Stocklify.Market.Infrastructure.Services;

namespace Stocklify.Market.Infrastructure.Database.Repositories;

public class StockTradeRepository
{
    private readonly ISenderV2 _sender;
    private readonly QuestDbHttpClient _questDbHttpClient;
    
    public StockTradeRepository(ISenderV2 sender, QuestDbHttpClient questDbHttpClient)
    {
        _sender = sender;
        _questDbHttpClient = questDbHttpClient;
    }
    
    public async Task AddAsync
    (
        string symbol,
        double value,
        long timestamp,
        CancellationToken cancellationToken = default
    )
    {
        await _sender.Table("trades")
            .Symbol("symbol", symbol)
            .Column("value", value)
            .AtAsync(timestamp, cancellationToken);
    }

    public async Task DeleteAllAsync()
    {
        var response = await _questDbHttpClient.ExecuteAsync("DROP TABLE trades");
        response.EnsureSuccessStatusCode();
    }
    
    public async Task<bool> ExistsAsync()
    {
        var response = await _questDbHttpClient.ExecuteAsync("SELECT TOP 1 TABLE trades");
        return response.IsSuccessStatusCode;
    }
}