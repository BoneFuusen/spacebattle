using System.Linq;

namespace SpaceBattle.Lib;

public class Vector
{
    private double[] coords;
    public int dim;
    public Vector(params double[] args)
    {
        coords = args;
        dim = args.Length;
    }

    public static Vector operator +(Vector a, Vector b)
    {
        if (a.dim != b.dim) throw new System.ArgumentException();

        var c = new Vector
        {
            coords = a.coords.Zip(b.coords, (x, y) => x + y).ToArray()
        };
        return c;
    }
    public override bool Equals(object? obj)
    {
        return obj is Vector v && coords.SequenceEqual(v.coords);
    }

    public override int GetHashCode() => coords.Aggregate(0, (total, next) => HashCode.Combine(total, next));  
}
