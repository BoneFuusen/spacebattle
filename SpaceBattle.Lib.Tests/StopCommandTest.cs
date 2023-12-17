using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Lib.Test;

public class StopCommandTests
{
    public StopCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var mockCommand = new Mock<SpaceBattle.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute());

        var mockInjecting = new Mock<IInjectable>();
        mockInjecting.Setup(x => x.Inject(It.IsAny<SpaceBattle.Lib.ICommand>()));

        var mockStrategyReturnIInjectable = new Mock<IStrategy>();
        mockStrategyReturnIInjectable.Setup(x => x.RunStrategy(It.IsAny<object[]>())).Returns(mockInjecting.Object);

        var mockStrategyReturnsCommand = new Mock<IStrategy>();
        mockStrategyReturnsCommand.Setup(x => x.RunStrategy(It.IsAny<object[]>())).Returns(mockCommand.Object);

        var mockStrategyReturnEmpty = new Mock<IStrategy>();
        mockStrategyReturnEmpty.Setup(x => x.RunStrategy()).Returns(mockCommand.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.RemoveProperty", (object[] args) => mockStrategyReturnsCommand.Object.RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.SetProperty", (object[] args) => mockStrategyReturnIInjectable.Object.RunStrategy(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Empty", (object[] args) => mockStrategyReturnEmpty.Object.RunStrategy(args)).Execute();
    }

    [Fact]
    public void SuccessofStopCommandExecute()
    {
        var stopable = new Mock<ICommandStop>();
        var obj = new Mock<IUObject>();
        stopable.SetupGet(a => a.Target).Returns(obj.Object).Verifiable();
        stopable.SetupGet(a => a.Properties).Returns(new List<string>() { "Velocity" }).Verifiable();

        ICommand stopMove = new StopCommand(stopable.Object);

        stopMove.Execute();

        stopable.Verify();
    }

    [Fact]
    public void TargetReturnsExceptionInStopCommand()
    {
        var stopable = new Mock<ICommandStop>();

        stopable.SetupGet(a => a.Target).Throws<Exception>().Verifiable();
        stopable.SetupGet(a => a.Properties).Returns(new List<string>() { "Velocity" }).Verifiable();

        ICommand stopMove = new StopCommand(stopable.Object);

        Assert.Throws<Exception>(() => stopMove.Execute());
    }

    [Fact]
    public void VelocityMethodReturnsExceptionInStopCommand()
    {
        var stopable = new Mock<ICommandStop>();
        var obj = new Mock<IUObject>();

        stopable.SetupGet(a => a.Target).Returns(obj.Object).Verifiable();
        stopable.SetupGet(a => a.Properties).Throws<Exception>().Verifiable();

        ICommand stopMove = new StopCommand(stopable.Object);
        Assert.Throws<Exception>(() => stopMove.Execute());
    }
}
