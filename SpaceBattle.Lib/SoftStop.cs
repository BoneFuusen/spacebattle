namespace SpaceBattle.Lib;

public class SoftStopCommand : ICommand
{
    private ServerThread _t;
    private Action _a;
    public SoftStopCommand(ServerThread t)
    {
        _t = t;
    }

    public SoftStopCommand(ServerThread t, Action a)
    {
        _t = t;
        _a = a;
    }

    public void Execute()
    {
        if (_t.IsCurrent())
        {
            var obehaviour = _t.GetBehaviour();
            _t.SetBehaviour(() =>
            {
                if (_t.QCount() != 0)
                {
                    obehaviour();
                }
                else
                {
                    _t.Stop();
                    _a?.Invoke();
                }
            });
        }
        else
        {
            throw new Exception("ExceptionSoftStop");
        }
    }
}
