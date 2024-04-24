namespace SpaceBattle.Lib;
using Hwdtech;

public class PushCommandToQueue : ICommand
{
    private int id;
    private ICommand cmd;

    public PushCommandToQueue(int id, ICommand cmd)
    {
        this.id = id;
        this.cmd = cmd;
    }

    public void Execute()
    {
        var queue = IoC.Resolve<Queue<ICommand>>("Game.GetQueue", id);
        queue.Enqueue(cmd);
    }
}
