namespace SpaceBattle.Lib;

public interface IMessage
{
    public string Type { get;}
    public int GameID { get;}
    public int GameItemID { get;}
    public IDictionary<string, object> Properties { get;}
}
