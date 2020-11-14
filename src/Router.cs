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
        private readonly ILogger _logger;

        public Router(ISomethingHappenedHandler somethingHappenedHandler, ILogger<Router> logger)
        {
            _somethingHappenedHandler = somethingHappenedHandler;
            _logger = logger;
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
                    _logger.LogError($"Ignoring message with SourceEnvironment {msg.SourceEnvironment} since this is in ENVIRONMENT_MODE {environmentMode}");
                    return;
                }

                switch (msg.Event.Name.ToString())
                {
                    case "Something Happened":
                        await _somethingHappenedHandler.ExecuteAsync(msg, message, ack);
                        _logger.LogInformation($"Message processed: {message}");
                        break;
                    
                    default:
                        ack();
                        break;
                }
            }
            catch (Exception e)
            {
                ack();
                _logger.LogError($"Message processing failed with exception: {e.Message}\n\nmessage received: {message}");
            }

        }
    }

}