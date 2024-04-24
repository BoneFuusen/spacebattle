/* using Moq;
using Hwdtech;
using Hwdtech.Ioc;
namespace SpaceBattle.Lib.Tests;


public class CommandInterpreterTests
{
    [Fact]
    public void CommandInterpreterTest()
    {
        var commandMockInterpreter = new Mock<CommandInterpreter>();
        var commandMock = new Mock<ICommand>();
        var obj = new Mock<IUObject>();

        var SetPropertiesTest = new Mock<ICommand>();
        SetPropertiesTest.Setup(x => x.Execute()).Verifiable();

        var getUObjFromMapStrategy = new Mock<IStrategy>();
        getUObjFromMapStrategy.Setup(s => s.Strategy(It.IsAny<int>())).Returns(obj.Object).Verifiable();
        
        var messageMock = new Mock<IMessage>();
        messageMock.Setup(x => x.Type).Returns("Test");
        messageMock.Setup(x => x.GameID).Returns(1);
        messageMock.Setup(x => x.GameItemID).Returns(2);
        messageMock.Setup(x => x.Properties).Returns(new Dictionary<string, object> { 
            { "Test", "Test" },
            { "Test2", "Test2" }
            });

        commandMockInterpreter.Setup(x => x.Execute()).Verifiable();

        IoC.Resolve<ICommand>("Game.SetProperties", obj.Object, "Test", "Test").Execute();

        
    }
}
 */