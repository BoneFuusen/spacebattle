using Hwdtech;

namespace SpaceBattle.Lib;

public class CheckCollision : ICommand
{
    private readonly IUObject obj1, obj2;
    private bool collision = true;

    public CheckCollision(IUObject uobj1, IUObject uobj2)
    {
        obj1 = uobj1;
        obj2 = uobj2;
    }

    public void Execute()
    {
        var builddecisiontree = IoC.Resolve<IDictionary<int, object>>("Game.CheckCollision");

        var object1 = IoC.Resolve<List<int>>("Game.Commands.GetProperty", obj1);
        var object2 = IoC.Resolve<List<int>>("Game.Commands.GetProperty", obj2);

        var list = object1.Zip(object2, (a, b) => a - b).ToList();

        // list.ForEach(n => builddecisiontree = (IDictionary<int, object>)builddecisiontree[n]);

        foreach (var item in list)
        {
            if (builddecisiontree.TryGetValue(item, out var value))
            {
                builddecisiontree = (IDictionary<int, object>)value;
            }
            else
            {
                builddecisiontree = new Dictionary<int, object>();
            }
        }

        IoC.Resolve<ICommand>("Game.Collision", obj1, obj2).Execute();
    }
}
