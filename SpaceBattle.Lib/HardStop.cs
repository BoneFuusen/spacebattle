namespace SpaceBattle.Lib;

public class HardStopCommand : ICommand
{
    private ServerThread _t;
    public HardStopCommand(ServerThread t)
    {
        _t = t;
    }

    public void Execute()
    {
        if (_t.IsCurrent())
        {
            _t.Stop();
        }
        else
        {
            throw new Exception("ExceptionHardStop");
        }
    }
}
