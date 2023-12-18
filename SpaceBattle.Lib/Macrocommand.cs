namespace SpaceBattle.Lib;

public class Macrocommand : ICommand
{
    private List<ICommand> commands;

    public Macrocommand(List<ICommand> commands)
    {
        this.commands = commands;
    }

    public void Execute()
    {
        commands.ToList().ForEach(command => command.Execute());
    }
}
