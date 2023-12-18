using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Numerics;

namespace SpaceBattle.Lib.Tests;

public class MacrocommandTest
{
    public MacrocommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void MacroCommandExecuteExecutesAllCommandsInOrder()
    {
        var command1Mock = new Mock<SpaceBattle.Lib.ICommand>();
        var command2Mock = new Mock<SpaceBattle.Lib.ICommand>();
        var command3Mock = new Mock<SpaceBattle.Lib.ICommand>();

        var commands = new List<SpaceBattle.Lib.ICommand>
        {
            command1Mock.Object,
            command2Mock.Object,
            command3Mock.Object
        };

        var macroCommand = new Macrocommand(commands);

        macroCommand.Execute();

        command1Mock.Verify(c => c.Execute(), Times.Once);
        command2Mock.Verify(c => c.Execute(), Times.Once);
        command3Mock.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void StrategyThrowsExceptionWhenOperationNameNotRegistered()
    {
        var mockCommand = new Mock<SpaceBattle.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Throws(new Exception());

        var commands = new List<SpaceBattle.Lib.ICommand> { mockCommand.Object };

        Assert.Throws<Exception>(() => new Macrocommand(commands).Execute());
    }

    [Fact]
    public void CreateMacroCommandStrategyResolvesCommandsAndCreatesMacroCommand()
    {
        var mockCommand = new Mock<SpaceBattle.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();

        string operationName = "Operation";
        var mockUObject = new Mock<IUObject>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", operationName, (object[] args) => new List<string> { "Game.Command.Operation" }).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateMacro.Create", (object[] args) => new CreateMacrocommand().Strategy(args[0], args[1])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Operation", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateMacro", (object[] args) => mockCommand.Object).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("CreateMacro.Create", operationName, mockUObject.Object).Execute();

        mockCommand.VerifyAll();
    }

    [Fact]
    public void CreateMacroCommandStrategyResolvesCommandsAndCreatesMacroCommandThrowsException()
    {
        var mockCommand = new Mock<SpaceBattle.Lib.ICommand>();
        mockCommand.Setup(x => x.Execute()).Throws(new Exception("ExpectedErrorMessage"));

        string operationName = "Operation";
        var mockUObject = new Mock<IUObject>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", operationName, (object[] args) => new List<string> { "Game.Command.Operation" }).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateMacro.Create", (object[] args) => new CreateMacrocommand().Strategy(args[0], args[1])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Operation", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateMacro", (object[] args) => mockCommand.Object).Execute();

        var exception = Assert.Throws<Exception>(() => { IoC.Resolve<SpaceBattle.Lib.ICommand>("CreateMacro.Create", operationName, mockUObject.Object).Execute(); });
        Assert.Equal("ExpectedErrorMessage", exception.Message);
    }
}
