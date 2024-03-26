using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib;

public class ServerThread
{
    private Action _behaviour;
    private BlockingCollection<ICommand> _queue;
    private Thread _thread;
    private bool _stop = false;

    public ServerThread(BlockingCollection<ICommand> queue)
    {
        _queue = queue;

        _behaviour = () =>
        {
            var cmd = _queue.Take();
            try
            {
                cmd.Execute();
            }
            catch (Exception e)
            {
                IoC.Resolve<ICommand>("ExceptionHandler.Handle", cmd, e).Execute();
            }
        };

        _thread = new Thread(() =>
        {

            while (!_stop)
            {
                _behaviour();
            }
        });
    }

    public void Stop()
    {
        _stop = !_stop;
    }

    public bool IsCurrent()
    {
        return _thread == Thread.CurrentThread;
    }

    internal Action GetBehaviour()
    {
        return _behaviour;
    }

    internal void SetBehaviour(Action newBehaviour)
    {
        _behaviour = newBehaviour;
    }

    public void Start()
    {
        _thread.Start();
    }

    public int QCount()
    {
        return _queue.Count();
    }
}
