using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using System.Threading.Channels;
using Stocklify.Faker.ValueObjects;

namespace Stocklify.Faker.Services;

public class StockBroadcaster
{
    private readonly ConcurrentQueue<Stock> _items = new();
    private readonly ConcurrentDictionary<Channel<Stock>, byte> _channels = new();
    
    private readonly int _maxCapacity;
    private readonly Gauge<double> _stockValuesGauge;
    private readonly ILogger<StockBroadcaster> _logger;
    
    public StockBroadcaster
    (
        int maxCapacity, 
        Meter meter,
        ILogger<StockBroadcaster> logger
    )
    {
        _maxCapacity = maxCapacity;
        _logger = logger;
        _stockValuesGauge = meter.CreateGauge<double>("stock.charts");
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
            _stockValuesGauge.Record(item.Value, new KeyValuePair<string, object?>("id", item.Id));
            foreach(var channel in _channels.Keys)
            {
                channel.Writer.TryWrite(item);
            }
        }
    }

    public bool IsMaxCapacityReached() => _maxCapacity == _items.Count;
}