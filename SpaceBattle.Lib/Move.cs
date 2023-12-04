namespace SpaceBattle.Lib;

public class Vector
{
    private double[] coords;
    public int dim;
    public Vector(params double[] args){
        coords = args;
        dim = args.Length;
    }

    public static Vector VectorSum(Vector a, Vector b){
        if (a.dim != b.dim) throw new System.ArgumentException();

        var c = a;
        for(int i = 0; i<a.dim; i++){
            c.coords[i] += b.coords[i];
        }
        return c;
    }
    public static bool VectorEquality(Vector a, Vector b){
        if (a.dim != b.dim) throw new System.ArgumentException();

        bool pass = true;

        for(int i = 0; i<a.dim; i++){
            if(a.coords[i] != b.coords[i]){
                pass = false;
                break;
            }
            else continue;
        }
        return pass;
    }
}

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