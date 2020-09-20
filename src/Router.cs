using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using src.events.handlers;

namespace src
{
    public class Router : IRouter
    {
        private readonly ISomethingHappenedHandler _somethingHappenedHandler;

        public Router(ISomethingHappenedHandler somethingHappenedHandler)
        {
            _somethingHappenedHandler = somethingHappenedHandler;
        }

        public async Task RouteAsync(string message, Action ack)
        {
            try
            {
                dynamic msg = JsonConvert.DeserializeObject(message);
                var environmentMode = Infokeeper.GetEnvironmentMode();
                if (msg.SourceEnvironment != environmentMode)
                {
                    ack();
                    return;
                }

                switch (msg.Event.Name.ToString())
                {
                    case "Something Happened":
                        await _somethingHappenedHandler.ExecuteAsync(msg, message, ack);
                        break;
                    
                    default:
                        ack();
                        break;
                }
            }
            catch (Exception e)
            {
                ack();
            }

        }
    }

}