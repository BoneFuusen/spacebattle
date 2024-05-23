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
    public void GameCommandIsWorksWithBaseExceptionInCommandsButNotStop()
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
            return quant.Object.Strategy();
        }).Execute();

        var excCmd = new Mock<ICommand>();
        excCmd.Setup(m => m.Execute()).Throws<Exception>();

        var cmd = new Mock<ICommand>();
        cmd.Setup(m => m.Execute()).Callback(() => { quant.Setup(m => m.Strategy()).Returns(0); });

        var Dict = new Dictionary<object, object>() { { excCmd.Object, new Exception() } };

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExeptionTree", (object[] args) =>
        {
            return Dict;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "BaseStrategy", (object[] args) =>
        {
            return new Mock<ICommand>().Object;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "ExceptionHandler.Handle",
            (object[] args) =>
            {
                return new ExceptionHandler((ICommand)args[0], (Exception)args[1]);
            }
        ).Execute();

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
    public void GameCommandIsWorksWithNotBaseExceptionInCommandsButNotStop()
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
            return quant.Object.Strategy();
        }).Execute();

        var excCmd2 = new Mock<ICommand>();
        excCmd2.Setup(m => m.Execute()).Throws<Exception>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExeptionTree", (object[] args) =>
        {
            return new Dictionary<object, object>();
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "ExceptionHandler.Handle",
            (object[] args) =>
            {
                return new ExceptionHandler((ICommand)args[0], (Exception)args[1]);
            }
        ).Execute();

        queue.Enqueue(excCmd2.Object);

        var gamecmd = new GameCommand(queue, scope);

        Assert.Throws<Exception>(() => gamecmd.Execute());
        excCmd2.Verify(m => m.Execute(), Times.Once());

    }

    [Fact]
    public void GameCommandWorksWithZeroQueue()
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
            return quant.Object.Strategy();
        }).Execute();

        var gamecmd = new GameCommand(queue, scope);
        gamecmd.Execute();

        Assert.Empty(queue);
    }
}
