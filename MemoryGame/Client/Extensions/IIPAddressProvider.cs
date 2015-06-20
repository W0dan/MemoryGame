using System.Net;

namespace MemoryGame.Client.Extensions
{
    public interface IIPAddressProvider
    {
        IPAddress GetLocalIPAddress();
    }
}