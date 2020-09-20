using System;
using System.Threading.Tasks;

namespace src 
{
    public interface IRouter 
    {
        Task RouteAsync(string message, Action done);
    }

}