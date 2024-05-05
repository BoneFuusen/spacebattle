using System.Net;
using CoreWCF;
using CoreWCF.OpenApi.Attributes;
using CoreWCF.Web;

namespace WebHttp
{
    [ServiceContract]
    [OpenApiBasePath("/api")]
    public interface IWebApi
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/body")]
        [OpenApiTag("Tag")]
        [OpenApiResponse(ContentTypes = new[] { "application/json", "text/xml" }, Description = "Success", StatusCode = HttpStatusCode.Accepted, Type = typeof(MessageContract))]
        void BodyEcho(
            [OpenApiParameter(ContentTypes = new[] { "application/json", "text/xml" }, Description = "param description.")] MessageContract param);
    }
}
