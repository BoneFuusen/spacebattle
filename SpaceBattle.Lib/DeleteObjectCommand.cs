using Hwdtech;

namespace SpaceBattle.Lib;

public class DeleteObjectCommand : ICommand
{
    private readonly int objectId;

    public DeleteObjectCommand(int objectId)
    {
        this.objectId = objectId;
    }
    public void Execute()
    {
        IoC.Resolve<IDictionary<int, IUObject>>("Game.IUObject.List").Remove(objectId);
    }
}
