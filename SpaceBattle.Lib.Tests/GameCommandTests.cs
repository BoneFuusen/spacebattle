using Castle.Components.DictionaryAdapter;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class GameCommandTests
{
    public GameCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
    }

    [Fact]
    public void GameCommandIsWorksWithExceptionInCommandsButNotStop()
    {
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            scope
        ).Execute();
        var queue = new Queue<ICommand>();
        var quant = new Mock<IStrategy>();
        quant.Setup(m => m.Strategy()).Returns(10);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQuant", (object[] args) =>
        {
            return (object)quant.Object.Strategy();
        }).Execute();


        var handleCommand = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Handle", (object[] args) => handleCommand.Object).Execute();

        var excCmd = new Mock<ICommand>();
        excCmd.Setup(m => m.Execute()).Throws<Exception>();

        var cmd = new Mock<ICommand>();
        cmd.Setup(m => m.Execute()).Callback(() => { quant.Setup(m => m.Strategy()).Returns(0); });

        queue.Enqueue(excCmd.Object);
        queue.Enqueue(cmd.Object);
        queue.Enqueue(cmd.Object);

        var gamecmd = new GameCommand(queue, scope);
        gamecmd.Execute();

        excCmd.Verify(m => m.Execute(), Times.Once());
        cmd.Verify(m => m.Execute(), Times.Once());
        Assert.Single(queue);
    }

    [Fact]
    public void GameCommandIsWorksWithZeroQueue()
    {
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            scope
        ).Execute();
        var queue = new Queue<ICommand>();
        var quant = new Mock<IStrategy>();
        quant.Setup(m => m.Strategy()).Returns(10);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQuant", (object[] args) =>
        {
            return (object)quant.Object.Strategy();
        }).Execute();

        var gamecmd = new GameCommand(queue, scope);
        gamecmd.Execute();

        Assert.Empty(queue);
    }
}
