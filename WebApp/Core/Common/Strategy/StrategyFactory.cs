namespace WebApp.Core.Common.Strategy;

public interface IStrategyFactory
{
    IStrategy<T>? GetStrategy<T>(T strategyType) where T : Enum;
}
public class StrategyFactory(IServiceProvider serviceProvider) : IStrategyFactory
{
    public IStrategy<T>? GetStrategy<T>(T strategyType) where T : Enum
    {
        var strategies = serviceProvider.GetServices(typeof(IStrategy<T>));
        var strategy = strategies.FirstOrDefault(s => ((IStrategy<T>)s).StrategyType.Equals(strategyType));
        if (strategy == null)
        {
            throw new Exception($"Strategy not found for type {strategyType}");
        }
        return strategy as IStrategy<T>;
    }
}
