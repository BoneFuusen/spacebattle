using Hwdtech;

namespace SpaceBattle.Lib;

public class CommandInterpreter : ICommand 
{
    private IMessage _message;

    public CommandInterpreter(IMessage message)
    {
        this._message = message;
    }

    public void Execute() 
    {
        var message = this._message;
        var type = message.Type;
        var gameitemid = message.GameItemID;
        var properties = message.Properties;

        var uobject = IoC.Resolve<IUObject>("Game.GetUObj", gameitemid);

        properties.ToList().ForEach(x => IoC.Resolve<ICommand>("Game.SetProperties", uobject, x.Key, x.Value).Execute());

        var cmd = IoC.Resolve<ICommand>("Game.CreateCommand", uobject);
        var id = message.GameID;

        IoC.Resolve<ICommand>("Game.Queue.Push", id, cmd).Execute();
    }
}

public class GetUObjStrategy : IStrategy
{
    public object Strategy(params object[] args)
    {
        var id = (int)args[0];

        var UObjDict = IoC.Resolve<Dictionary<int, IUObject>>("GetUObj");
        if (!UObjDict.TryGetValue(id, out IUObject? obj))
            throw new Exception("UObject not found");
        return obj;
    }
}
