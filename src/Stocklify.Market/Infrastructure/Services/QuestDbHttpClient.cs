namespace Stocklify.Market.Infrastructure.Services;

public class QuestDbHttpClient
{
    private readonly HttpClient _httpClient;
    
    public QuestDbHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<HttpResponseMessage> ExecuteAsync(string query, CancellationToken cancellationToken = default) => await _httpClient.GetAsync($"/exec?query={query}", cancellationToken);
}