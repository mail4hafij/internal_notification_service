using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace src.models
{
    public class SettingsRepository : ISettingsRepository
    {
        public IList<Notifier> Notifiers { get; set; }

        public SettingsRepository()
        {
            Notifiers = new List<Notifier>();
        }

        public void Read(string filename)
        {
            Notifiers = new List<Notifier>();
            
            if (!File.Exists(filename))
            {
                return;
            }

            var level0 = JObject.Parse(File.ReadAllText(filename));

            foreach (var level1 in level0.Properties())
            {
                if (level1.Name == "notifiers")
                {
                    foreach (var level2 in level1.Value.Children<JProperty>())
                    {
                        var notifier = new Notifier
                        {
                            Name = level2.Name,
                            Settings = new ConcurrentDictionary<string, string>()
                        };

                        foreach (var level3 in level2.Value.Children<JProperty>())
                        {
                            if (level3.Name == "type")
                            {
                                notifier.Type = level3.Value.ToObject<string>();
                            }
                            else
                            {
                                notifier.Settings[level3.Name] = level3.Value.ToObject<string>();
                            }
                        }
                        Notifiers.Add(notifier);
                    }
                }
            }
            
        }

        public ISettings GetSettings(string name)
        {
            var notifier = Notifiers.FirstOrDefault(o => o.Name == name);
            if (notifier == null) return null;

            var ret = new Settings();
            ret.Set("name", notifier.Name);
            ret.Set("type", notifier.Type);
            if (notifier.Settings != null)
            {
                foreach (var key in notifier.Settings.Keys)
                {
                    ret.Set(key, notifier.Settings[key]);
                }
            }

            return ret;
        }
    }
}
