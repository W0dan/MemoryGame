using System;
using System.Diagnostics;
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
        private bool _isStopped;

        public void Start(string player, int port)
        {
            _isAskedToStop = false;
            _isStopped = false;

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

                try
                {
                    serviceHost.Close();

                    _isStopped = true;
                }
                catch
                {
                    _isStopped = true;
                }
            });
        }

        public void Stop()
        {
            _isAskedToStop = true;

            while (!_isStopped)
            {
                Thread.Sleep(100);
            }
        }
    }
}