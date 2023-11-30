using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Numerics;

namespace SpaceBattle.Lib.Tests;

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
