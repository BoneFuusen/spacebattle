using System.Collections.Generic;
using System.Runtime.Serialization;
using CoreWCF.OpenApi.Attributes;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace WebHttp
{
    [DataContract(Name = "ExampleContract", Namespace = "http://example.com")]
    [ExcludeFromCodeCoverage]
    public class MessageContract
    {
        [DataMember(Name = "Type of command", Order = 1)]
        [OpenApiProperty(Description = "Type of command description.")]
        public required string TypeCommand { get; set; }

        [DataMember(Name = "Game Id", Order = 2)]
        [OpenApiProperty(Description = "Game Id description.")]
        public required string GameId { get; set; }

        [DataMember(Name = "Id of object in game", Order = 3)]
        [OpenApiProperty(Description = "Id of object in game description.")]
        public required string ObjectId { get; set; }

        [DataMember(Name = "Proprties", Order = 4)]
        [OpenApiProperty(Description = "Proprties description.")]
        public IDictionary<string, object> Proprties { get; set; }
    }
}
