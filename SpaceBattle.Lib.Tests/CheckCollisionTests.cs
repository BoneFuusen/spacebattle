using SpaceBattle.Lib;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace CheckCollisionTests;

public class CheckCollisionTests
{
    public CheckCollisionTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.GetProperty", (object[] args) => new List<int> { 1, 1, 1 }).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "IUObject.CheckCollision", (object[] args) => new CheckCollision((IUObject)args[0], (IUObject)args[1])).Execute();
    }

    [Fact]
    public void NormalCheckCollision()
    {
        var mockICommand = new Mock<SpaceBattle.Lib.ICommand>();
        var mockDict = new Mock<IDictionary<int, object>>();
        var mockObj = new Mock<IUObject>();

        mockICommand.Setup(m => m.Execute()).Verifiable();
        mockDict.SetupGet(m => m[It.IsAny<int>()]).Returns(mockDict.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CheckCollision", (object[] args) => mockDict.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Collision", (object[] args) => mockICommand.Object).Execute();

        var checkCollision = IoC.Resolve<SpaceBattle.Lib.ICommand>("IUObject.CheckCollision", mockObj.Object, mockObj.Object);

        checkCollision.Execute();

        mockICommand.Verify();
    }

    [Fact]
    public void notNormalCheckCollision()
    {
        var mockICommand = new Mock<SpaceBattle.Lib.ICommand>();
        var mockDict = new Mock<IDictionary<int, object>>();
        var mockObj = new Mock<IUObject>();

        mockICommand.Setup(m => m.Execute()).Verifiable();
        mockDict.SetupGet(m => m[It.IsAny<int>()]).Returns(mockDict.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CheckCollision", (object[] args) => mockDict.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Collision", (object[] args) => mockICommand.Object).Execute();

        var checkCollision = IoC.Resolve<SpaceBattle.Lib.ICommand>("IUObject.CheckCollision", mockObj.Object, mockObj.Object);

        checkCollision.Execute();

        mockICommand.Verify();
    }
}
