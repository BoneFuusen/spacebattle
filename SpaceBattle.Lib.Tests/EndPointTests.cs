using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using WebHttp;

namespace SpaceBattle.Lib.Tests;

public class EndpointTests
{
    public EndpointTests()
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
    public void EndpointWorksSuccessfuly()
    {
        var ThreadId = 1;

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "SearchThreadIdByGameId",
            (object[] args) => { return (object)ThreadId; }).Execute();

        var message = new MessageContract()
        {
            TypeCommand = "rotate",
            GameId = "1",
            ObjectId = "1",
            Proprties = new Dictionary<string, object>() { { "AngleVelocity", 1 } }
        };

        var cmd = new Mock<ICommand>();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "ICommand Message",
            (object[] args) =>
            {
                return cmd.Object;
            }
        ).Execute();

        var q = new BlockingCollection<ICommand>();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Endpoint.Commands.SendCommand",
            (object[] args) =>
            {
                var cmd = new ActionCommand(() =>
                {
                    q.Add((ICommand)args[1]);
                });
                return cmd;
            }
        ).Execute();

        var wa = new Endpoint();

        var response = wa.BodyEcho(message);

        Assert.Single(q);
        Assert.Equal("202 Accepted", response);
    }

    [Fact]
    public void EndpointWorksWhithExceptionInJson()
    {
        var ThreadId = 1;

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "SearchThreadIdByGameId",
            (object[] args) => { return (object)ThreadId; }).Execute();

        var cmd = new Mock<ICommand>();
        cmd.Setup(cmd => cmd.Execute()).Throws(new Exception());

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "ICommand Message",
            (object[] args) =>
            {
                return cmd.Object;
            }
        ).Execute();

        var q = new BlockingCollection<ICommand>();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Endpoint.Commands.SendCommand",
            (object[] args) =>
            {
                var cmd = new ActionCommand(() =>
                {
                    throw new Exception();
                });
                return cmd;
            }
        ).Execute();

        var message = new Mock<MessageContract>();

        var wa = new Endpoint();

        var response = wa.BodyEcho(message.Object);

        Assert.Equal("400 Bad Request. System.Exception", response);
    }

    [Fact]
    public void EndpointWorksWhithExceptionInId()
    {
        var ThreadId = new Exception();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "SearchThreadIdByGameId",
            (object[] args) => { return (object)ThreadId; }).Execute();

        var cmd = new Mock<ICommand>();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "ICommand Message",
            (object[] args) =>
            {
                return cmd.Object;
            }
        ).Execute();

        var q = new BlockingCollection<ICommand>();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Endpoint.Commands.SendCommand",
            (object[] args) =>
            {
                var cmd = new ActionCommand(() =>
                {
                    q.Add((ICommand)args[1]);
                });
                return cmd;
            }
        ).Execute();

        var message = new Mock<MessageContract>();

        var wa = new Endpoint();

        var response = wa.BodyEcho(message.Object);

        Assert.Equal("400 Bad Request. System.InvalidCastException", response);
    }
}
