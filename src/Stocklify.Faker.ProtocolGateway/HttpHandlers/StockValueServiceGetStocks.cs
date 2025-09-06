using Google.Protobuf.WellKnownTypes;
using Stocklify.Faker.Services.Grpc;

namespace Stocklify.Faker.ProtocolGateway.HttpHandlers;

public class StockValueServiceGetStocks
{
    private readonly StockValueService.StockValueServiceClient _stockValueServiceClient;
    
    public StockValueServiceGetStocks(StockValueService.StockValueServiceClient stockValueServiceClient)
    {
        _stockValueServiceClient = stockValueServiceClient;
    }
    
    public async Task<IResult> HandleAsync() => Results.Ok(await _stockValueServiceClient.GetStocksAsync(new Empty()));
}