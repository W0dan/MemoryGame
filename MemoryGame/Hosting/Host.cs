using System;
using System.ServiceModel;
using MemoryGame.Contracts;
using MemoryGame.Server;

namespace MemoryGame.Hosting
{
    public class Host
    {
        private ServiceHost _serviceHost;

        public void Start(string player, int port)
        {
            var uriString = string.Format("net.tcp://localhost:{0}/MultiplayerServer", port);
            var baseAddress = new Uri(uriString);
            _serviceHost = new ServiceHost(typeof(MultiplayerService), baseAddress);
            var tcpBinding = new NetTcpBinding(SecurityMode.None);
            _serviceHost.AddServiceEndpoint(typeof(IMultiplayerService), tcpBinding, baseAddress);

            _serviceHost.Open();
        }

        public void Stop()
        {
            _serviceHost.Close();
        }
    }
}