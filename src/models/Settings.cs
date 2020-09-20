using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace src.models
{
    public class Settings : ISettings
    {
        private readonly ConcurrentDictionary<string, string> _values = new ConcurrentDictionary<string, string>();

        public string Get(string name)
        {
            return _values.ContainsKey(name) ? _values[name] : null;
        }

        public void Set(string name, string value)
        {
            _values[name] = value;
        }

        public DateTime Now => DateTime.Now;

        public IList<string> GetNames()
        {
            return _values.Keys.ToList();
        }
    }
}
