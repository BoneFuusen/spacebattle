using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Numerics;

namespace SpaceBattle.Lib.Tests;

public class ActionCommand : Lib.ICommand
{
    private readonly Action _action;
    public ActionCommand(Action action) => _action = action;
    public void Execute()
    {
        _action();
    }
}

public class StartCommandTests
{
    private Mock<ICommandStart> _commandStartMock;
    private Mock<IUObject> _uObjectMock;
    private StartCommand _startCommand;

    public StartCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        _commandStartMock = new Mock<ICommandStart>();
        _uObjectMock = new Mock<IUObject>();

        _commandStartMock.Setup(m => m.Target).Returns(_uObjectMock.Object);
        _commandStartMock.Setup(m => m.Properties).Returns(new Dictionary<string, object>());

        _startCommand = new StartCommand(_commandStartMock.Object);
    }

    [Fact]
    public void Execute_RegistersTargetsAndAddsMoveCommandToQueue_WhenCalled()
    {
        var movingCommandMock = new Mock<SpaceBattle.Lib.ICommand>();
        var commandMock = new Mock<SpaceBattle.Lib.ICommand>();
        var queueMock = new Mock<IQueue>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Operations.Movement", (object[] args) => movingCommandMock.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "IUObject.Property.Set", (object[] args) => commandMock.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => queueMock.Object).Execute();

        _startCommand.Execute();

        _commandStartMock.Verify(m => m.Properties, Times.Once());
        queueMock.Verify(q => q.Add(It.IsAny<SpaceBattle.Lib.ICommand>()), Times.Once());
    }
}

public class IQueueTests
{
    [Fact]
    public void ChecksThatTheExtractedItemFromTheQueueIsEqualToTheAddedItem()
    {
        var qReal = new Queue<SpaceBattle.Lib.ICommand>();
        var qMock = new Mock<IQueue>();

        qMock.Setup(q => q.Take()).Returns(() =>
        {
            return qReal.Dequeue();
        });

        var cmd = new Mock<SpaceBattle.Lib.ICommand>();

        qReal.Enqueue(cmd.Object);

        Assert.Equal(cmd.Object, qMock.Object.Take());
    }

    [Fact]
    public void CheckTheCorrectnessOfTheImplementationOfTheMethodsOfTheIQueueInterface()
    {
        var qReal = new Queue<SpaceBattle.Lib.ICommand>();
        var qMock = new Mock<IQueue>();

        qMock.Setup(q => q.Take()).Returns(() =>
        {
            return qReal.Dequeue();
        });

        qMock.Setup(q => q.Add(It.IsAny<SpaceBattle.Lib.ICommand>())).Callback(
        (SpaceBattle.Lib.ICommand cmd) =>
        {
            qReal.Enqueue(cmd);
        });

        var cmd = new Mock<SpaceBattle.Lib.ICommand>();

        qMock.Object.Add(cmd.Object);

        Assert.Equal(cmd.Object, qMock.Object.Take());
    }
}

public class IoCTests
{
    public IoCTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
    }

    [Fact]
    public void CheckedThatTheReceivedCommandObjectFromTheContainerIsEqualToTheCreatedMockCommandObject()
    {
        var cmd = new Mock<SpaceBattle.Lib.ICommand>();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Dependency",
            (object[] args) =>
            {
                return cmd.Object;
            }
        ).Execute();

        var cmdFromIoC = IoC.Resolve<SpaceBattle.Lib.ICommand>("Dependency");

        Assert.Equal(cmdFromIoC, cmd.Object);
    }

    [Fact]
    public void TheCommandObjectIsRemovedFromTheQueueAndComparedWithTheCreatedMockCommandObject()
    {
        var qReal = new Queue<SpaceBattle.Lib.ICommand>();
        var qMock = new Mock<IQueue>();

        qMock.Setup(q => q.Take()).Returns(() =>
        {
            return qReal.Dequeue();
        });
        qMock.Setup(q => q.Add(It.IsAny<SpaceBattle.Lib.ICommand>())).Callback(
        (SpaceBattle.Lib.ICommand cmd) =>
        {
            qReal.Enqueue(cmd);
        });

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue",
            (object[] args) =>
            {
                return qMock.Object;
            }
        ).Execute();

        var cmd = new Mock<SpaceBattle.Lib.ICommand>();

        IoC.Resolve<IQueue>("Game.Queue").Add(cmd.Object);

        var cmdFromIoC = IoC.Resolve<IQueue>("Game.Queue").Take();

        Assert.Equal(cmd.Object, cmdFromIoC);

    }

    [Fact]
    public void ChecksTheRegistrationOfTheCommandInTheIoCContainerWithoutCheckingTheExecutionOfTheCommand()
    {
        var ac = new ActionCommand(() =>
        {

        });

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.EmptyCommand",
            (object[] args) =>
            {
                return ac;
            }
        ).Execute();

    }
}
