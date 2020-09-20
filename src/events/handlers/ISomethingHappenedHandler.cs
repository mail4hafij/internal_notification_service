using System;
using System.Threading.Tasks;

namespace src.events.handlers
{
    public interface ISomethingHappenedHandler
    {
        Task ExecuteAsync(dynamic msg, string messsage, Action ack);
    }
}
