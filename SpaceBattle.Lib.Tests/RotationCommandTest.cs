using Moq;
using System.Numerics;

namespace SpaceBattle.Lib.Tests;

public class RotationCommandTest
{

    // Тест первый. Всё работает
    // Тест второй. Не читается начальный угол поворота
    // Тест третий. Не читается то, на сколько нужно поворачиваться
    // Тест четвёртый. Объект не может двигаться

    [Fact]
    public void TheGameObjectCanRotateUniformlyWithoutDeformationAroundItself()
    {
        // pre
        var rotation = new Mock<IRotation>();

        rotation.SetupGet(m => m.Angle).Returns(1).Verifiable();
        rotation.SetupGet(m => m.AngleVelocity).Returns(1).Verifiable();
        rotation.SetupGet(m => m.Division).Returns(8).Verifiable();

        ICommand rotateCommand = new RotateCommand(rotation.Object);

        // action
        rotateCommand.Execute();

        //post
        rotation.VerifySet(m => m.Angle = 2, Times.Once);
        rotation.VerifyAll();
    }

    [Fact]
    public void ThePositionOfTheGameObjectCannotBeConsideredWhenRotateUniformlyWithoutDeformation()
    {
        // pre
        var rotation = new Mock<IRotation>();

        rotation.SetupGet(m => m.Angle).Throws(new Exception()).Verifiable();
        rotation.SetupGet(m => m.AngleVelocity).Returns(1).Verifiable();
        rotation.SetupGet(m => m.Division).Returns(8).Verifiable();

        ICommand rotateCommand = new RotateCommand(rotation.Object);

        //action and post
        Assert.Throws<Exception>(() => rotateCommand.Execute());
    }

    [Fact]
    public void AccelerationCannotBeConsideredForAGameObjectWhenRotateUniformlyWithoutDeformation()
    {
        // pre
        var rotation = new Mock<IRotation>();

        rotation.SetupGet(m => m.Angle).Returns(1).Verifiable();
        rotation.SetupGet(m => m.AngleVelocity).Throws(new Exception()).Verifiable();
        rotation.SetupGet(m => m.Division).Returns(8).Verifiable();

        ICommand rotateCommand = new RotateCommand(rotation.Object);

        //action and post
        Assert.Throws<Exception>(() => rotateCommand.Execute());
    }

    [Fact]
    public void TheGameObjectCannotRotateUniformlyWithoutDeformation()
    {
        // pre
        var rotation = new Mock<IRotation>();

        rotation.SetupGet(m => m.Angle).Returns(1).Verifiable();
        rotation.SetupGet(m => m.AngleVelocity).Returns(1).Verifiable();
        rotation.SetupGet(m => m.Division).Returns(8).Verifiable();

        ICommand rotateCommand = new RotateCommand(rotation.Object);

        // action
        rotateCommand.Execute();

        //post
        rotation.SetupSet(m => m.Angle = 2).Throws(new Exception()).Verifiable();
        Assert.Throws<Exception>(() => rotateCommand.Execute());
        rotation.VerifyAll();
    }
}
