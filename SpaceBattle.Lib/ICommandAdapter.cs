namespace SpaceBattle.Lib;
using Hwdtech;

public class ICommandAdapter : ICommand
{
    private Hwdtech.ICommand _cmd;

    public ICommandAdapter(Hwdtech.ICommand cmd)
    {
        _cmd = cmd;
    }

    public void Execute()
    {
        _cmd.Execute();
    }
}
