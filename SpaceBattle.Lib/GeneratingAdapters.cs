using Scriban;

namespace SpaceBattle.Lib;

public class GeneratingAdapters
{
    private Type _pastType;
    private Type _nextType;

    public GeneratingAdapters(Type pastType, Type nextType)
    {
        _pastType = pastType;
        _nextType = nextType;
    }
    public string Adapt()
    {
        var properties = _nextType.GetProperties().ToList();

        var sampleString =
@"public class {{next_type}}Adapter : {{next_type}}
{
    private {{previous_type}} _obj;
    public {{next_type}}Adapter({{previous_type}} obj)
    {
        _obj = obj;
    }
{{for property in (properties)}}
    public {{property.property_type.name}} {{property.name}}
    {
{{if property.can_read}}
        get
        {
            IoC.Resolve<{{property.property_type.name}}>('Get.Property', '{{property.name}}', _obj);
        }
{{end}}
{{if property.can_write}}
        set
        {
            IoC.Resolve<ICommand>('Set.Property', '{{property.name}}', _obj, value).Execute();
        }
{{end}}
    }
{{end}}
}";
        var code = Template.Parse(sampleString);
        var renderedCode = code.Render(new
        {
            previous_type = _pastType.Name,
            next_type = _nextType.Name,
            properties,
        });

        return renderedCode;
    }
}
