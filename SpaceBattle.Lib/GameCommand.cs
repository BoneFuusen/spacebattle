using Hwdtech;
using System.Diagnostics;
namespace SpaceBattle.Lib;

public class GameCommand : ICommand
{
    private Queue<ICommand> _queue;
    private object _scope;
    private Stopwatch _watch;

    public GameCommand(Queue<ICommand> queue, object scope)
    {
        _queue = queue;
        _scope = scope;
        _watch = new Stopwatch();
    }

    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _scope).Execute();

        while (_watch.ElapsedMilliseconds <= (int)IoC.Resolve<object>("GetQuant"))
        {
            if (_queue.Count == 0)
            {
                break;
            }
            _watch.Start();
            var cmd = _queue.Dequeue();
            try
            {
                cmd.Execute();
            }
            catch (Exception e)
            {
                IoC.Resolve<ICommand>("ExceptionHandler.Handle", cmd, e).Execute();
            }
            _watch.Stop();
        }
    }
}
