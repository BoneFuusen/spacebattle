using Hwdtech;

namespace SpaceBattle.Lib;

public class BuildCollisionTree : ICommand
{
    private readonly string file_train;

    public BuildCollisionTree(string file_train)
    {
        this.file_train = file_train;
    }
    public void Execute()
    {
        var list = File.ReadAllLines(file_train).ToList().Select(line => line.Split(" ").Select(int.Parse).ToList()).ToList();

        var collision_tree = IoC.Resolve<Dictionary<int, object>>("Game.GetCollisionTree");

        list.ForEach(array =>
        {
            var tree = collision_tree;
            array.ForEach(num =>
            {
                tree.TryAdd(num, new Dictionary<int, object>());
                tree = (Dictionary<int, object>)tree[num];
            });
        });
    }
}
