using System.Collections;
using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateEmptyGameObjects : ICommand
{
    private int _quantity;
    public CreateEmptyGameObjects(int quantity)
    {
        _quantity = quantity;
    }

    public void Execute()
    {
        var uObjects = IoC.Resolve<IDictionary>("Game.GetUObjects");

        Enumerable.Range(0, _quantity).ToList().ForEach(i =>
        {
            uObjects.Add(i, IoC.Resolve<IUObject>("Game.UObject.Create"));
        });
    }
}
