namespace Stocklify.Faker.ProtocolGateway.Options;

public class ApplicationOptions
{
    public const string SectionName = "Application";
    
    public string? StockValueServiceClientEndpoint { get; set; }
}