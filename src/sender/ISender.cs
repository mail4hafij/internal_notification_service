using src.models;

namespace src.sender
{
    public interface ISender
    {
        void Send(IMessage message, ISettings settings);
    }

}
