using Hwdtech;

namespace SpaceBattle.Lib;

public class QueuePopStrategy : ISimpleStrategy
{
    public object Run()
    {
        var queue = IoC.Resolve<IQueue>("Game.Queue");
        return queue.Take();
    }
}
