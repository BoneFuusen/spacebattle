namespace SpaceBattle.Lib;

public interface ICommandStop
{
    IEnumerable<string> Properties { get; }
    IUObject Target { get; }
}
