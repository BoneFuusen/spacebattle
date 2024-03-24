using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class ExceptionThreadTest
{
    public ExceptionThreadTest()
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
    public void ExceptionCommandShouldNotStopThread()
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

        var mockObject = new Mock<Action>();

        var hs = IoC.Resolve<ICommand>("ServerThread.Commands.HardStopTheThread", 1, () => { mre.Set(); });

        var handleCommand = new Mock<ICommand>();
        handleCommand.Setup(m => m.Execute()).Verifiable();

        var mockObjectExeption = new Mock<Action>();
        mockObjectExeption.Setup(m => m.Invoke()).Throws<Exception>().Verifiable();

        var scopeCommand = IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
                IoC.Resolve<object>("Scopes.New",
                    IoC.Resolve<object>("Scopes.Root")
                )
            );

        var adaptedScopeCommand = new ICommandAdapter(scopeCommand);

        var adaptedRegisterCommand = new ICommandAdapter(IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Handle", (object[] args) => handleCommand.Object));

        IoC.Resolve<string>("ServerThread.Commands.SendCommand", 1, adaptedScopeCommand);
        IoC.Resolve<string>("ServerThread.Commands.SendCommand", 1, adaptedRegisterCommand);
        IoC.Resolve<string>("ServerThread.Commands.SendCommand", 1, new ActionCommand(mockObject.Object));
        IoC.Resolve<string>("ServerThread.Commands.SendCommand", 1, new ActionCommand(mockObjectExeption.Object));
        IoC.Resolve<string>("ServerThread.Commands.SendCommand", 1, new ActionCommand(mockObject.Object));
        IoC.Resolve<string>("ServerThread.Commands.SendCommand", 1, hs);
        IoC.Resolve<string>("ServerThread.Commands.SendCommand", 1, new ActionCommand(mockObject.Object));

        mre.WaitOne();

        mockObject.Verify(m => m.Invoke(), Times.Exactly(2));
        handleCommand.Verify(m => m.Execute(), Times.Once());
    }
}
