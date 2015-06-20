using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace MemoryGame.Client.Extensions
{
    public class IPAddressProvider : IIPAddressProvider
    {
        public IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.
                FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}