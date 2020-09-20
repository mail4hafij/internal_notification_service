using System.Collections.Generic;

namespace src.models
{
    public interface ISettingsRepository
    {
        ISettings GetSettings(string name);
        IList<Notifier> Notifiers { get; set; }
        void Read(string filename);
    }
}