using Moq;
namespace SpaceBattle.Lib.Tests;

public class VectorTest
{
    [Fact]
    public void VectorTestPositivePlus()
    {
        Vector a = new Vector(2, 3);
        Vector b = new Vector(4, 5);
        Assert.True(Vector.Equals(new Vector(6, 8), a + b));
    }

    [Fact]
    public void VectorDifferentSizezSumTest()
    {
        Vector a = new Vector(2, 3);
        Vector b = new Vector(4, 5, 6);
        Assert.Throws<ArgumentException>(() => a + b);
    }

    [Fact]
    public void VectorDifferentSizezEqualityTest()
    {
        Vector a = new Vector(2, 3);
        Vector b = new Vector(4, 5, 6);
        Assert.False(Vector.Equals(a, b));
    }

    [Fact]
    public void VectorTestPositiveEquality()
    {
        Vector a = new Vector(2, 2);
        Vector b = new Vector(2, 2);
        Assert.True(Vector.Equals(a, b));
    }

    [Fact]
    public void VectorNegativeEqualTest()
    {
        Vector a = new Vector(5, 5);
        Vector b = new Vector(25, 5);
        Assert.False(Vector.Equals(a, b));
    }

    [Fact]
    public void VectorPositiveeEqvals()
    {
        Vector a = new Vector(5, 5);
        Vector b = new Vector(5, 5);
        Assert.True(Vector.Equals(a, b));
    }

    [Fact]
    public void GetHashCodeVectorTest()
    {
        Vector a = new Vector(1, 2);
        Vector b = new Vector(1, 2);
        Assert.Equal(a.GetHashCode(), a.GetHashCode());
    }

    [Fact]
    public void AttemptToCompareNonVectorObjects()
    {
        Vector a = new Vector(1, 2);
        int b = 2;
        Assert.False(Vector.Equals(a, b));
    }
}
