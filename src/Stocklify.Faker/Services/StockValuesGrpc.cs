using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Stocklify.Faker.Services.Grpc;

namespace Stocklify.Faker.Services;

public class StockValuesGrpc : StockValueService.StockValueServiceBase
{
    private readonly StockBroadcaster _stockBroadcaster;
    private readonly StockContext _stockContext;
    
    public StockValuesGrpc(StockBroadcaster stockBroadcaster, StockContext stockContext)
    {
        _stockBroadcaster = stockBroadcaster;
        _stockContext = stockContext;
    }

    public override Task<Stocks> GetStocks(Empty request, ServerCallContext context)
    {
        var result = new Stocks();
        
        var stocks = _stockContext.GetCurrentState();
        for (var i = 0; i < _stockContext.NumOfStocks; i++)
        {
            ref var stock = ref stocks[i];
            result.Stocks_.Add(new Stock
            {
                StockId = stock.Id,
                Value = stock.Value,
                Timestamp = stock.Timestamp
            });
        }

        return Task.FromResult(result);
    }

    public override async Task SubscribeToChanges(Empty request, IServerStreamWriter<Stock> responseStream, ServerCallContext context)
    {
        var stockBroadcastSubscriberChannel = _stockBroadcaster.Subscribe();
        try
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await foreach (var stock in stockBroadcastSubscriberChannel.Reader.ReadAllAsync(context.CancellationToken))
                {
                    await responseStream.WriteAsync
                    (
                        new Stock
                        {
                            StockId = stock.Id,
                            Value = stock.Value,
                            Timestamp = stock.Timestamp
                        }
                    );
                }
                await Task.Delay(10, context.CancellationToken);
            }
        }
        finally
        {
            _stockBroadcaster.UnSubscribe(stockBroadcastSubscriberChannel);
        }
    }
}