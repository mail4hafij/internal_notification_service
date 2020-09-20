using System.Collections.Concurrent;


namespace src.models
{
    public class Notifier
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public ConcurrentDictionary<string, string> Settings { get; set; }
    }
}
