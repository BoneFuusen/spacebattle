namespace SpaceBattle.Lib;

using Hwdtech;
using System;

public class SetProperties : ICommand
{
    private IUObject uobject;
    private string key;
    private object value;

    public SetProperties(IUObject uobject, string key, object value)
    {
        this.uobject = uobject;
        this.key = key;
        this.value = value;
    }

    public void Execute()
    {
        SetProperty();
    }

    public void SetProperty()
    {
        uobject.setProperty(key, value);
    }
}
