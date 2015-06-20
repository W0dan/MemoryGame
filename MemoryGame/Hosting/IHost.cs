namespace MemoryGame.Hosting
{
    public interface IHost
    {
        void Start(string player, int port);
        void Stop();
    }
}