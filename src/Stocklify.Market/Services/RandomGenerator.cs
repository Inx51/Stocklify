namespace Stocklify.Market.Services;

public class RandomGenerator
{
    public int GetRandomIntValueInRange(int min, int max)
    {
        return Random.Shared.Next(min, max);
    }
    
    public double GetRandomDoubleValueInRange(double min, double max)
    {
        return Random.Shared.NextDouble() * (max - min) + min;
    }
}