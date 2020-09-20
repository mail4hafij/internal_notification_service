using System;
using System.Collections.Generic;

namespace src.models
{
    public interface ISettings
    {
        string Get(string name);
        void Set(string name, string value);
        IList<string> GetNames();
        DateTime Now { get; }
    }
}