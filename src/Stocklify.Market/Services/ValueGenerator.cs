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
        var result = currentValue + GetChange();
        if (result < 0)
            return 0;

        return result;
    }

    private double GetChange()
    {
        var changeHappened = _randomGenerator.GetRandomDoubleValueInRange(0.0, 1) > 0.4;
        if (!changeHappened)
            return 0;

        var change = _randomGenerator.GetRandomDoubleValueInRange(-3, 3);
        var changeShouldMultiply = _randomGenerator.GetRandomDoubleValueInRange(0.0, 10.0) > 7;
        if (changeShouldMultiply)
        {
            var changeMultiplier = _randomGenerator.GetRandomDoubleValueInRange(0.5, 4.0);
            change *= changeMultiplier;
        }

        return change;
    }
    
    public double GenerateRandomValue(double min, double max) => _randomGenerator.GetRandomDoubleValueInRange(min, max);
}