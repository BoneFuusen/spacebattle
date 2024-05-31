using Hwdtech;

namespace SpaceBattle.Lib;

public class SetPlacementToShips : ICommand
{
    private List<IUObject> _uObjects;
    private List<int[]> _positions;
    public SetPlacementToShips(List<IUObject> uObjects, List<int[]> positions)
    {
        _uObjects = uObjects;
        _positions = positions;
    }

    public void Execute()
    {
        IEnumerator<int[]> positionsEnumerator = _positions.GetEnumerator();

        _uObjects.ForEach(obj =>
        {
            IoC.Resolve<ICommand>("Game.Property.Set", obj, "Position", positionsEnumerator.Current).Execute();
            positionsEnumerator.MoveNext();
        });

        positionsEnumerator.Reset();
    }
}
