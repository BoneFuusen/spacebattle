using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class GeneratingAdaptersTests
{
    public GeneratingAdaptersTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void GeneratingAdaptersPositive()
    {
        var adapter = new GeneratingAdapters(typeof(IUObject), typeof(IRotation));
        var output = adapter.Adapt();
        var exoutput =
@"public class IRotationAdapter : IRotation
{
    private IUObject _obj;
    public IRotationAdapter(IUObject obj)
    {
        _obj = obj;
    }

    public Int32 Angle
    {

        get
        {
            IoC.Resolve<Int32>('Get.Property', 'Angle', _obj);
        }


        set
        {
            IoC.Resolve<ICommand>('Set.Property', 'Angle', _obj, value).Execute();
        }

    }

    public Int32 AngleVelocity
    {

        get
        {
            IoC.Resolve<Int32>('Get.Property', 'AngleVelocity', _obj);
        }


    }

    public Int32 Division
    {

        get
        {
            IoC.Resolve<Int32>('Get.Property', 'Division', _obj);
        }


    }

}";

        Assert.Equal(exoutput, output);
    }
}
