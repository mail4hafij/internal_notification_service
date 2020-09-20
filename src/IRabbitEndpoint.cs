namespace src 
{
    public interface IRabbitEndpoint
    {
        void StartListening();
        void StopListening();
        bool IsConnectionsOpen();
    }
}