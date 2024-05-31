using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class InitStartStateTests
{
    public InitStartStateTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void CreateEmptyGameObjectsPositive()
    {
        var dict = new Dictionary<int, object>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetUObjects", (object[] args) => dict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Create", (object[] args) => new Mock<IUObject>().Object).Execute();

        new CreateEmptyGameObjects(3).Execute();

        Assert.Equal(3, dict.Count);
    }

    [Fact]
    public void CreateEmptyGameObjectsWhithExceptionInGetDict()
    {
        var exeption = new Mock<IStrategy>();
        exeption.Setup(e => e.Strategy()).Throws<Exception>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetUObjects", (object[] args) => exeption.Object.Strategy()).Execute();

        Assert.Throws<Exception>(new CreateEmptyGameObjects(3).Execute);
    }

    [Fact]
    public void CreateEmptyGameObjectsWhithExceptionInUObjCreate()
    {
        var dict = new Dictionary<int, object>();
        var exeption = new Mock<IStrategy>();
        exeption.Setup(e => e.Strategy()).Throws<Exception>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetUObjects", (object[] args) => dict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.Create", (object[] args) => exeption.Object.Strategy()).Execute();

        Assert.Throws<Exception>(new CreateEmptyGameObjects(3).Execute);
    }

    [Fact]
    public void SetPlacementToShipsPositive()
    {
        var obj = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();
        var obj3 = new Mock<IUObject>();
        var objs = new List<IUObject>() { obj.Object, obj2.Object, obj3.Object };
        var cmd = new Mock<ICommand>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Property.Set", (object[] args) => cmd.Object).Execute();

        var list = new List<int[]>();

        list.Add(new int[] { 1, 2 });
        list.Add(new int[] { 4, 5 });
        list.Add(new int[] { 7, 8 });

        new SetPlacementToShips(objs, list).Execute();

        cmd.Verify(t => t.Execute(), Times.Exactly(3));
    }

    [Fact]
    public void SetPlacementToShipsWhithException()
    {
        var obj = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();
        var obj3 = new Mock<IUObject>();
        var objs = new List<IUObject>() { obj.Object, obj2.Object, obj3.Object };
        var cmd = new Mock<ICommand>();
        cmd.Setup(c => c.Execute()).Throws<Exception>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Property.Set", (object[] args) => cmd.Object).Execute();

        var list = new List<int[]>();

        Assert.Throws<Exception>(new SetPlacementToShips(objs, list).Execute);
    }

    [Fact]
    public void SetFuelToShipsPositive()
    {
        var obj = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();
        var obj3 = new Mock<IUObject>();
        var objs = new List<IUObject>() { obj.Object, obj2.Object, obj3.Object };
        var cmd = new Mock<ICommand>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Property.Set", (object[] args) => cmd.Object).Execute();

        var list = new List<int>() { 1, 2, 3 };

        new SetFuelToShips(objs, list).Execute();

        cmd.Verify(t => t.Execute(), Times.Exactly(3));
    }

    [Fact]
    public void SetFuelToShipsWhithException()
    {
        var obj = new Mock<IUObject>();
        var obj2 = new Mock<IUObject>();
        var obj3 = new Mock<IUObject>();
        var objs = new List<IUObject>() { obj.Object, obj2.Object, obj3.Object };
        var cmd = new Mock<ICommand>();
        cmd.Setup(c => c.Execute()).Throws<Exception>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Property.Set", (object[] args) => cmd.Object).Execute();

        var list = new List<int>();

        Assert.Throws<Exception>(new SetFuelToShips(objs, list).Execute);
    }
}
