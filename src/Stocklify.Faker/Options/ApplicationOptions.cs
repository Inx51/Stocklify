namespace Stocklify.Faker.Options;

public class ApplicationOptions
{
    public const string SectionName = "Application";
    
    public int BroadcastCapacity { get; set; } = 100;
    
    public int NumberOfStocksInMarket { get; set; } = 10;
    
    public int MinChangeRate { get; set; } = 100;

    public int MaxChangeRate { get; set; } = 300;
}