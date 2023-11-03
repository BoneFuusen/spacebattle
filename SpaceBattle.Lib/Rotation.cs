namespace SpaceBattle.Lib;

public interface IRotation
{
    public int Position { get; set; }
    public int Velocity { get; }
    public int Division { get; }
}

public class RotateCommand : ICommand
{
    private readonly IRotation rotation;
    public RotateCommand(IRotation rotation)
    {
        this.rotation = rotation;
    }
    public void Execute()
    {
        rotation.Position = (rotation.Position + rotation.Velocity) % rotation.Division;
    }
}
