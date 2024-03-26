using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class HardStopTests
{
    public HardStopTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();
    }

    [Fact]
    public void HardStopCommandShouldStopServer()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();

        new ThreadInitialization().Execute();

        var mre = new ManualResetEvent(false);
        IoC.Resolve<SpaceBattle.Lib.ICommand>("ServerThread.Commands.CreateAndStartThread", 1, () => { }).Execute();

        var hs = IoC.Resolve<ICommand>("ServerThread.Commands.HardStopTheThread", 1, () => { mre.Set(); });

        var mockObject = new Mock<Action>();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("ServerThread.Commands.SendCommand", 1, new ActionCommand(mockObject.Object)).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("ServerThread.Commands.SendCommand", 1, new ActionCommand(mockObject.Object)).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("ServerThread.Commands.SendCommand", 1, hs).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("ServerThread.Commands.SendCommand", 1, new ActionCommand(mockObject.Object)).Execute();

        mre.WaitOne();

        mockObject.Verify(m => m.Invoke(), Times.Exactly(2));
    }

    [Fact]
    public void HardStopCommandExeptionCheck()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();

        new ThreadInitialization().Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("ServerThread.Commands.CreateAndStartThread", 1, () => { }).Execute();

        var hs = IoC.Resolve<ICommand>("ServerThread.Commands.HardStopTheThread", 1);

        Assert.Throws<Exception>(hs.Execute);

        IoC.Resolve<SpaceBattle.Lib.ICommand>("ServerThread.Commands.SendCommand", 1, hs).Execute();
    }
}
