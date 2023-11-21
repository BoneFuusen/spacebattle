namespace SpaceBattle.Lib;

public interface ICommandStart
{
    IUObject Target { get; }
    IDictionary<string, object> Properties { get; }
}