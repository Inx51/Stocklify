namespace Stocklify.Market.Services;

public class ValueGenerator
{
    private readonly RandomGenerator _randomGenerator;
    
    public ValueGenerator
    (
        RandomGenerator randomGenerator
    )
    {
        _randomGenerator = randomGenerator;
    }
    
    public double GenerateNextValue(double currentValue)
    {
        return currentValue + GetChange();;
    }

    private double GetChange()
    {
        var changeHappened = _randomGenerator.GetRandomDoubleValueInRange(0.0, 1) > 0.7;
        if (!changeHappened)
            return 0;

        var change = _randomGenerator.GetRandomDoubleValueInRange(-2, 2);
        var changeShouldMultiply = _randomGenerator.GetRandomDoubleValueInRange(0.0, 10.0) > 8;
        if (changeShouldMultiply)
        {
            var changeMultiplier = _randomGenerator.GetRandomDoubleValueInRange(0.1, 4.0);
            change *= changeMultiplier;
        }

        return change;
    }
    
    public double GenerateRandomValue(double min, double max) => _randomGenerator.GetRandomDoubleValueInRange(min, max);
}