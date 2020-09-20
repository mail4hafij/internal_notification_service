using System.Collections.Generic;

namespace src.models
{
    public interface IMessage
    {
        bool IsEmpty { get; }
        string Subject { get; }
        string Text { get; }
        Status Status { get; }
    }
}