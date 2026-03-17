using Hwdtech;

namespace SpaceBattle.Lib
{
    public class InterpreterCommand : ICommand
    {
        private readonly IMessage _customMessage;

        public InterpreterCommand(IMessage message)
        {
            _customMessage = message;
        }
        public void Execute()
        {
            var id = _customMessage.GameID;
            var cmd = IoC.Resolve<object>("Game.CreateCommand", _customMessage);
            IoC.Resolve<Hwdtech.ICommand>("Game.Queue.Push", id, cmd).Execute();
        }
    }
}
