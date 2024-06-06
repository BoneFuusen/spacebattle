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
    public string Build()
    {
        var properties = _nextType.GetProperties().ToList();

        var sampleString = @"
        public class {{nextType_name}}Adapter : {{nextType_name}} 
        {
            private {{pastType_name}} _obj;
    
            public {{nextType_name}}Adapter({{pastType_name}} obj)
            {
                _obj = obj;
            }

            {{for property in (next_properties)}}
            public {{property.property_type.name}} {{property.name}}
            {
                {{if property.can_read}}
                get
                {
                    return IoC.Resolve<{{property.property_type.name}}>('Get.Property', '{{property.name}}', _obj);
                }
                {{if property.can_write}}
                set
                {
                    return IoC.Resolve<ICommand>('Set.Property', '{{property.name}}', _obj, value).Execute();
                }
            }
        }";
        var sample = Template.Parse(sampleString);
        var sampledString = sample.Render(new
        {
            nextType_name = _nextType.Name,
            pastType_name = _pastType.Name,
            next_properties = properties,
        });
        return sampledString;
    }
}
