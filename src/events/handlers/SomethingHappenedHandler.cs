using System;
using System.IO;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using src.models;
using src.sender;

namespace src.events.handlers
{
    public class SomethingHappenedHandler : ISomethingHappenedHandler
    {
        private ISettingsRepository _settingsRepository;
        private ISenderFactory _senderFactory; 
        
        public SomethingHappenedHandler(ISettingsRepository settingsRepository, ISenderFactory senderFactory)
        {
            _settingsRepository = settingsRepository;
            _senderFactory = senderFactory;
        }

        public Task ExecuteAsync(dynamic msg, string message, Action ack)
        {
            var somethingHappened = MapMessageToObject(msg, message);
            var validateResults = new SomethingHappenedValidator().Validate(somethingHappened);

            if (!validateResults.IsValid)
            {
                throw new Exception("Event validation failed");
            }
            

            var environmentMode = Infokeeper.GetEnvironmentMode();
            var filename = Infokeeper.GetSettingsFilename();

            if (!File.Exists(filename))
            {
                throw new Exception("File not found settings." + environmentMode + ".json");
            }

            _settingsRepository.Read(filename);
            foreach (var notifier in _settingsRepository.Notifiers)
            {
                ISender sender = null;
                switch (notifier.Type.ToLowerInvariant())
                {
                    case "email":
                        sender = _senderFactory.Email();
                        break;
                    case "slack":
                        sender = _senderFactory.Slack();
                        break;
                }

                if (sender != null)
                {
                    var settings = _settingsRepository.GetSettings(notifier.Name);
                    sender.Send(new Message("SUBJECT", "MESSAGE", Status.Positive), settings);
                }
            }
            ack();
            return Task.CompletedTask;
        }

        private SomethingHappened MapMessageToObject(dynamic msg, string message)
        {
            return new SomethingHappened
            {
                Event = message
            };
        }
    }
}
