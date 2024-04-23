using CoreWCF;
using Hwdtech;
using System;

namespace WebHttp
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Endpoint : IWebApi
    {
        public string BodyEcho(MessageContract message)
        {
            try
            {
                var ThreadId = (int)IoC.Resolve<object>("SearchThreadIdByGameId", message.GameId);
                IoC.Resolve<SpaceBattle.Lib.ICommand>("Endpoint.Commands.SendCommand", ThreadId, IoC.Resolve<SpaceBattle.Lib.ICommand>("ICommand Message", message)).Execute();
                return "202 Accepted";
            }
            catch (Exception e)
            {
                var exp = e.GetType();
                return $"400 Bad Request. {exp}";
            }
        }
    }
}
