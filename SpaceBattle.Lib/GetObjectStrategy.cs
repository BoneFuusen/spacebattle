using Hwdtech;

namespace SpaceBattle.Lib;

public class GetObjectStrategy : IGameStrategy
{
    public object Run(params object[] args)
    {
        return IoC.Resolve<IDictionary<int, IUObject>>("Game.IUObject.List")[(int)args[0]];
    }
}
