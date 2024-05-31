using Hwdtech;

namespace SpaceBattle.Lib;

public class SetFuelToShips : ICommand
{
    private List<IUObject> _uObjects;
    private List<int> _fuel;
    public SetFuelToShips(List<IUObject> uObjects, List<int> fuel)
    {
        _uObjects = uObjects;
        _fuel = fuel;
    }

    public void Execute()
    {
        IEnumerator<int> fuelEnumerator = _fuel.GetEnumerator();

        _uObjects.ForEach(obj =>
        {
            IoC.Resolve<ICommand>("Game.Property.Set", obj, "Fuel", fuelEnumerator.Current).Execute();
            fuelEnumerator.MoveNext();
        });

        fuelEnumerator.Reset();
    }
}
