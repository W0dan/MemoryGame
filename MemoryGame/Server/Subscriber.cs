namespace MemoryGame.Server
{
    public class Subscriber<TCallback>
    {
        public string Token { get; set; }
        public string Name { get; set; }
        public TCallback Callback { get; set; }
    }
}