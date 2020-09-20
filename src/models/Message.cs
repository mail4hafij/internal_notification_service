using System.Collections.Generic;

namespace src.models
{
    public class Message : IMessage
    {
        public Message(string subject, string text, Status status)
        {
            Subject = subject;
            Text = text;
            Status = status;
        }

        public string Subject { get; }

        public string Text { get; }

        public bool IsEmpty => string.IsNullOrWhiteSpace(Text);

        public Status Status { get; }
    }

    public enum Status
    {
        Positive,
        Negative
    }
}
