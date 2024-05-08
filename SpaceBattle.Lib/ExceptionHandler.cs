using Hwdtech;

namespace SpaceBattle.Lib;

public class ExceptionHandler : ICommand
{
    private ICommand _Command;
    private Exception _Exeption;

    public ExceptionHandler(ICommand Command, Exception Exception)
    {
        _Command = Command;
        _Exeption = Exception;
    }
    public void Execute()
    {
        var Dict = IoC.Resolve<Dictionary<object, object>>("ExeptionTree");
        if (Dict.ContainsKey(_Command))
        {
            IoC.Resolve<ICommand>("BaseStrategy").Execute();
        }
        else
        {
            _Exeption.Data["Command"] = _Command;
            throw _Exeption;
        }
    }
}
