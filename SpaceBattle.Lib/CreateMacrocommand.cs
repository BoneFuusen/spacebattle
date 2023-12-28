namespace SpaceBattle.Lib;

using Hwdtech;

public class CreateMacrocommand : IStrategy
{
    public object Strategy(params object[] objects)
    {
        var operationname = (string)objects[0];
        var iuObject = (IUObject)objects[1];
        var commands = IoC.Resolve<IEnumerable<string>>(operationname);
        var addCommand = commands.Select(o => IoC.Resolve<ICommand>(o, iuObject));
        return IoC.Resolve<ICommand>("CreateMacro", addCommand);
    }
}
