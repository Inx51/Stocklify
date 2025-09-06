using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using Stocklify.Faker.Services.Grpc;

namespace Stocklify.Faker.ProtocolGateway.SingalR;

public class StockValueServiceHub : Hub
{
    private readonly StockValueService.StockValueServiceClient _stockValueServiceClient;

    private static readonly HashSet<string> Subscribers = new();
    private static readonly Lock Lock = new ();
    private static readonly string SubscribesToChangesGroup = "SubscribesToChanges";
    private static CancellationTokenSource? _subscriptionControlFlowCancellationTokenSource;
    
    public StockValueServiceHub(StockValueService.StockValueServiceClient stockValueServiceClient)
    {
        _stockValueServiceClient = stockValueServiceClient;
    }
    
    public async Task GetStocksAsync()
    {
        var stocks = await _stockValueServiceClient.GetStocksAsync(new Empty());
        await Clients.Caller.SendAsync("getStocks", stocks);
    }
    
    public async Task SubscribeToChangesAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, SubscribesToChangesGroup);
        
        lock (Lock)
        {
            Subscribers.Add(Context.ConnectionId);
            
            if (Subscribers.Count == 1)
            {
                _subscriptionControlFlowCancellationTokenSource = new CancellationTokenSource();
                _ = StartSubscribeToChangesLoopAsync(_subscriptionControlFlowCancellationTokenSource.Token);
            }
        }
    }

    private async Task StartSubscribeToChangesLoopAsync(CancellationToken cancellationToken)
    {
        using var call = _stockValueServiceClient.SubscribeToChanges(new Empty(), cancellationToken: cancellationToken);
        await foreach (var stock in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            await Clients.Group(SubscribesToChangesGroup).SendAsync("subscribeToChanges", stock, cancellationToken);
        }
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        lock (Lock)
        {
            if (Subscribers.Contains(Context.ConnectionId))
            {

                Subscribers.Remove(Context.ConnectionId);
                Groups.RemoveFromGroupAsync(Context.ConnectionId, SubscribesToChangesGroup);
                
                if (Subscribers.Count == 0)
                {
                    _subscriptionControlFlowCancellationTokenSource?.Cancel();
                    _subscriptionControlFlowCancellationTokenSource?.Dispose();
                    _subscriptionControlFlowCancellationTokenSource = null;
                }
            }
        }

        return Task.CompletedTask;
    }
}