using Moq;

namespace SpaceBattle.Lib.Tests;
public class MoveCommandTest
{
    [Fact]
    public void TheGameObjectMoveStraightlyWithoutDeformationStraightMovementWithoutDeformation(){
        Mock<IMove> move = new Mock<IMove>();
        move.SetupGet(a => a.pos).Returns(new Vector(12, 5));
        move.SetupGet(b => b.vel).Returns(new Vector(-7, 3));
        ICommand movecommand = new MoveCommand(move.Object);

        movecommand.Execute();
        var husk = new Vector(5, 8);
        bool pass = Vector.VectorEquality(move.Object.pos, husk);

        Assert.True(pass);
    }

    [Fact]
    public void ThePositionOfTheGameObjectCannotBeConsideredWhenMovingStraightlyWithoutDeformation(){
        Mock<IMove> move = new Mock<IMove>();
        move.SetupGet(a => a.pos).Throws<ArgumentException>();
        move.SetupGet(b => b.vel).Returns(new Vector(-7, 3));
        ICommand movecommand = new MoveCommand(move.Object);

        Assert.Throws<ArgumentException>(() => movecommand.Execute());
    }

    [Fact]
    public void VelocityCannotBeConsideredForAGameObjectWhenMovingStraightlyWithoutDeformation(){
        Mock<IMove> move = new Mock<IMove>();
        move.SetupGet(a => a.pos).Returns(new Vector(12, 5));
        move.SetupGet(b => b.vel).Throws<ArgumentException>();
        ICommand movecommand = new MoveCommand(move.Object);

        Assert.Throws<ArgumentException>(() => movecommand.Execute());
    }

    [Fact]
    public void MovingAnObjectWhichPositionCannotBeChanged(){
        Mock<IMove> move = new Mock<IMove>();
        move.SetupGet(a => a.pos).Returns(new Vector(12, 5));
        move.SetupSet(a => a.pos = It.IsAny<Vector>()).Throws<ArgumentException>();
        move.SetupGet(b => b.vel).Returns(new Vector(-7, 3));
        ICommand movecommand = new MoveCommand(move.Object);

        Assert.Throws<ArgumentException>(() => movecommand.Execute());
    }
}
