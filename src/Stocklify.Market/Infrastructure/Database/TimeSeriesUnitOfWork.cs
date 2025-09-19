using QuestDB.Senders;
using Stocklify.Market.Infrastructure.Database.Repositories;

namespace Stocklify.Market.Infrastructure.Database;

public class TimeSeriesUnitOfWork
{
    public StockTradeRepository StockTrades { get; private set; }

    private readonly ISenderV2 _sender;
    
    public TimeSeriesUnitOfWork(ISenderV2 sender, StockTradeRepository stockRepository)
    {
        StockTrades = stockRepository;
        _sender = sender;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _sender.SendAsync(cancellationToken);
    }
}