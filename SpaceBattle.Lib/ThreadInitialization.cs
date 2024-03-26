using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class ThreadInitialization : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "ServerThread.Commands.CreateAndStartThread",
            (object[] args) =>
            {
                var queue = new BlockingCollection<ICommand>();
                var thread = new ServerThread(queue);
                int id = (int)args[0];

                IoC.Resolve<Hwdtech.ICommand>(
                    "IoC.Register",
                    "ServerThread.Commands.Search" + id,
                    (object[] objs) =>
                    {
                        return thread;
                    }).Execute();

                IoC.Resolve<Hwdtech.ICommand>(
                    "IoC.Register",
                    "ServerThread.Commands.SearchQ" + id,
                    (object[] objs) =>
                    {
                        return queue;
                    }).Execute();

                if (args.Count() == 2)
                {
                    new ActionCommand((Action)args[1]).Execute();
                }

                var cmd = new ActionCommand(() => {thread.Start();});
                return cmd;
            }).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "ServerThread.Commands.SendCommand",
            (object[] args) =>
            {
                var q = IoC.Resolve<BlockingCollection<ICommand>>("ServerThread.Commands.SearchQ" + (int)args[0]);
                var cmd = new ActionCommand(() => {q.Add((ICommand)args[1]);});
                return cmd;
            }).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "ServerThread.Commands.HardStopTheThread",
            (object[] args) =>
            {
                var thread = IoC.Resolve<ServerThread>("ServerThread.Commands.Search" + (int)args[0]);

                if (args.Count() == 2)
                {
                    return new ActionCommand(() =>
                    {
                        new HardStopCommand(thread).Execute();
                        new ActionCommand((Action)args[1]).Execute();
                    });
                }

                return new ActionCommand(() =>
                {
                    new HardStopCommand(thread).Execute();
                });
            }).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "ServerThread.Commands.SoftStopTheThread",
            (object[] args) =>
            {
                var thread = IoC.Resolve<ServerThread>("ServerThread.Commands.Search" + (int)args[0]);

                if (args.Count() == 2)
                {
                    return new ActionCommand(() =>
                    {
                        new SoftStopCommand(thread, (Action)args[1]).Execute();
                    });
                }
                return new ActionCommand(() =>
                {
                    new SoftStopCommand(thread).Execute();
                });
            }).Execute();
    }
}
