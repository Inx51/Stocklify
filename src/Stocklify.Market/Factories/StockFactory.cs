using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Nodes;
using Stocklify.Market.Entities;
using Stocklify.Market.Services;

namespace Stocklify.Market.Factories;

public class StockFactory
{
    private readonly RandomGenerator _randomGenerator;
    
    private HashSet<(int prefix , int suffix)> _claimedCombinations = [];
    private string[] _symbolPrefixes = new string[100];
    private string[] _symbolSuffixes = new string[100];
    private string[] _companyNamePrefixes = new string[100];
    private string[] _companyNameSuffixes = new string[100];
    
    public StockFactory
    (
        RandomGenerator randomGenerator
    )
    {
        _randomGenerator = randomGenerator;
        InitializeSymbolsAndCompanyNames();
    }

    private void InitializeSymbolsAndCompanyNames()
    {
        var dataFile = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Data.json");
        var json = JsonSerializer.Deserialize<JsonObject>(dataFile)!;
        _symbolPrefixes = json["symbolPrefixes"].Deserialize<string[]>()!;
        _symbolSuffixes = json["symbolPrefixes"].Deserialize<string[]>()!;
        _companyNamePrefixes = json["companyNamePrefixes"].Deserialize<string[]>()!;
        _companyNameSuffixes = json["companyNameSuffixes"].Deserialize<string[]>()!;
    }

    public Stock CreateStock()
    {
        var (prefixSeed, suffixSeed) = GetUniqueSeedCombination();
        
        var companyName = _companyNamePrefixes[prefixSeed]+_companyNameSuffixes[suffixSeed];
        var symbol = _symbolPrefixes[prefixSeed]+_symbolSuffixes[suffixSeed];

        var stock = new Stock(symbol, companyName);
        
        return stock;
    }

    private (int prefixSeed, int suffixSeed) GetUniqueSeedCombination()
    {
        var prefixSeed = _randomGenerator.GetRandomIntValueInRange(0, 100);
        var suffixSeed = _randomGenerator.GetRandomIntValueInRange(0, 100);
        
        if(!_claimedCombinations.Add((prefixSeed, suffixSeed)))
            return GetUniqueSeedCombination();

        return (prefixSeed, suffixSeed);
    }
}