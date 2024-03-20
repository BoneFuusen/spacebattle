using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class SoftStopTests
{
    public SoftStopTests()
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
    public void SoftStopCommandShouldStopServer()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();

        new ThreadInitialization().Execute();

        var mre = new ManualResetEvent(false);
        IoC.Resolve<string>("ServerThread.Commands.CreateAndStartThread", 1, () => { });

        var ss = IoC.Resolve<ICommand>("ServerThread.Commands.SoftStopTheThread", 1, () => { mre.Set(); });

        var mockObject = new Mock<Action>();

        IoC.Resolve<string>("ServerThread.Commands.SendCommand", 1, new ActionCommand(mockObject.Object));
        IoC.Resolve<string>("ServerThread.Commands.SendCommand", 1, new ActionCommand(mockObject.Object));
        IoC.Resolve<string>("ServerThread.Commands.SendCommand", 1, ss);
        IoC.Resolve<string>("ServerThread.Commands.SendCommand", 1, new ActionCommand(mockObject.Object));

        mre.WaitOne();

        mockObject.Verify(m => m.Invoke(), Times.Exactly(3));
    }

    [Fact]
    public void SoftStopCommandExeptionCheck()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();

        new ThreadInitialization().Execute();

        IoC.Resolve<string>("ServerThread.Commands.CreateAndStartThread", 1, () => { });

        var ss = IoC.Resolve<ICommand>("ServerThread.Commands.SoftStopTheThread", 1);

        Assert.Throws<Exception>(ss.Execute);

        IoC.Resolve<string>("ServerThread.Commands.SendCommand", 1, ss);
    }
}
