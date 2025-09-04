using System.Collections.Concurrent;
using System.Threading.Channels;
using Microsoft.Extensions.Options;
using Stocklify.StockFakeValueGenerator.Options;
using Stocklify.StockFakeValueGenerator.ValueObjects;

namespace Stocklify.StockFakeValueGenerator.Services;

public class StockBroadcaster
{
    private readonly ConcurrentQueue<Stock> _items = new();
    private readonly ConcurrentDictionary<Channel<Stock>, byte> _channels = new();
    
    private readonly int _maxCapacity;
    private readonly ILogger<StockBroadcaster> _logger;
    
    public StockBroadcaster(IOptions<ApplicationOptions> options, ILogger<StockBroadcaster> logger)
    {
        _maxCapacity = options.Value.BroadcastCapacity;
        _logger = logger;
    }
    
    public Channel<Stock> Subscribe()
    {
        var channel = Channel.CreateBounded<Stock>(1);
        _channels.TryAdd(channel, 0);
        return channel;
    }
    
    public void UnSubscribe(Channel<Stock> channel)
    {
        if (_channels.TryRemove(channel, out _))
        {
            channel.Writer.TryComplete();
        }
    }

    public void Broadcast(ref Stock stock)
    {
        if(_items.Count <= _maxCapacity)
            _items.Enqueue(stock);

        while (!_items.IsEmpty)
        {
            if(!_items.TryDequeue(out var item))
                _logger.LogError("Failed to dequeue item from queue.");
            foreach(var channel in _channels.Keys)
            {
                channel.Writer.TryWrite(item);
            }
        }
    }

    public bool IsMaxCapacityReached() => _maxCapacity == _items.Count;
}