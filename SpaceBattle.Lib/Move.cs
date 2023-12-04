namespace SpaceBattle.Lib;
public interface IMove
{
    public Vector pos{get; set;}
    public Vector vel{get;}
}

public class MoveCommand : ICommand
{
    private readonly IMove moving;
    public MoveCommand(IMove obj){
        moving = obj;
    }
    public void Execute(){
        moving.pos = Vector.VectorSum(moving.pos, moving.vel);
    }
}
