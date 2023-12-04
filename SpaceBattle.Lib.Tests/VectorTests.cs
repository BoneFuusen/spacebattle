using Moq;
namespace SpaceBattle.Lib.Tests;

public class VectorTest
{
    [Fact]
    public void VectorTestPositivePlus()
    {
        Vector a = new Vector(2, 3);
        Vector b = new Vector(4, 5);
        Assert.True(Vector.VectorEquality(new Vector(6, 8), Vector.VectorSum(a, b)));
    }

    [Fact]
    public void VectorDifferentSizezSumTest()
    {
        Vector a = new Vector(2, 3);
        Vector b = new Vector(4, 5, 6);
        Assert.Throws<ArgumentException>(() => Vector.VectorSum(a, b));
    }
    [Fact]
    public void VectorDifferentSizesEqualityTest()
    {
        Vector a = new Vector(2, 3);
        Vector b = new Vector(4, 5, 6);
        Assert.Throws<ArgumentException>(() => Vector.VectorEquality(a, b));
    }

    [Fact]
    public void VectorTestPositiveEquality()
    {
        Vector a = new Vector(2, 2);
        Vector b = new Vector(2, 2);
        Assert.True(Vector.VectorEquality(a, b));
    }

    [Fact]
    public void VectorNegativeEqualTest()
    {
        Vector a = new Vector(5, 5);
        Vector b = new Vector(25, 5);
        Assert.False(Vector.VectorEquality(a, b));
    }

    [Fact]
    public void VectorPositiveeEqvals()
    {
        Vector a = new Vector(5, 5);
        Vector b = new Vector(5, 5);
        Assert.True(Vector.VectorEquality(a, b));
    }

    [Fact]
    public void GetHashCodeVectorTest()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(1, 2);
        Assert.Equal(Vector.VectorHashCode(a), Vector.VectorHashCode(b));
    }
}
