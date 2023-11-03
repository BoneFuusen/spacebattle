namespace SpaceBattle.Lib;

public interface IRotation
{
    public int Angle { get; set; }
    public int AngleVelocity { get; }
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
        rotation.Angle = (rotation.Angle + rotation.AngleVelocity) % rotation.Division;
    }
}
