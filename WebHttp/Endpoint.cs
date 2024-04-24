using CoreWCF;
using Hwdtech;
using System;

namespace WebHttp
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Endpoint : IWebApi
    {
        public void BodyEcho(MessageContract message)
        {
            var ThreadId = (int)IoC.Resolve<object>("SearchThreadIdByGameId", message.GameId);
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Endpoint.Commands.SendCommand", ThreadId, IoC.Resolve<SpaceBattle.Lib.ICommand>("ICommand Message", message)).Execute();
        }
    }
}
