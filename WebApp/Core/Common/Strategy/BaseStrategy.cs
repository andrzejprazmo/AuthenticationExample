namespace WebApp.Core.Common.Strategy;

public enum StrategyType
{
    None = 0,
    StrategyA = 1,
    StrategyB = 2
}

public interface IStrategy<T> where T : Enum
{
    T StrategyType { get; }
    void Execute();
}
public abstract class BaseStrategy<T> where T : Enum
{
    public required T StrategyType { get; set; }

    public BaseStrategy(T strategyType)
    {
        StrategyType = strategyType;
    }

    public abstract void Execute();
}

public class StrategyA : IStrategy<StrategyType>
{
    public StrategyType StrategyType => StrategyType.StrategyA;

    public void Execute()
    {
        throw new NotImplementedException();
    }
}

public class StrategyB : IStrategy<StrategyType>
{
    public StrategyType StrategyType => StrategyType.StrategyB;

    public void Execute()
    {
        throw new NotImplementedException();
    }
}