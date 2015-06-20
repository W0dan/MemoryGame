using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using MemoryGame.Contracts;
using MemoryGame.Server;

namespace MemoryGame.Hosting
{
    public class Host : IHost
    {
        private bool _isAskedToStop;

        public void Start(string player, int port)
        {
            _isAskedToStop = false;

            Task.Factory.StartNew(() =>
            {
                var uriString = string.Format("net.tcp://localhost:{0}/MultiplayerServer", port);
                var baseAddress = new Uri(uriString);
                var serviceHost = new ServiceHost(typeof(MultiplayerService), baseAddress);
                var tcpBinding = new NetTcpBinding(SecurityMode.None);
                serviceHost.AddServiceEndpoint(typeof(IMultiplayerService), tcpBinding, baseAddress);

                serviceHost.Open();

                while (!_isAskedToStop)
                {
                    Thread.Sleep(100);
                }

                serviceHost.Close();
            });
        }

        public void Stop()
        {
            _isAskedToStop = true;
        }
    }
}