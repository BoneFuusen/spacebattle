using Hwdtech;

namespace SpaceBattle.Lib;

public class ExceptionHandlerInitioalization : ICommand
{
    private ICommand _handleCommand;

    public ExceptionHandlerInitioalization(ICommand handleCommand)
    {
        _handleCommand = handleCommand;
    }
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "ExceptionHandler.Handle",
            (object[] args) =>
            {
                return _handleCommand;
            }
        ).Execute();
    }
}
