using System;
using System.ServiceModel;
using MemoryGame.Client.Service;
using MemoryGame.Contracts;
using MemoryGame.Extensions;

namespace MemoryGame.Proxy
{
    public class MultiplayerProxy : IMultiplayerService
    {
        public event Action<string, string> ChatMessageReceived;

        private readonly ChannelFactory<IMultiplayerService> _factory;

        public MultiplayerProxy(string host, string port)
        {
            var callbackService = new CallbackService();

            callbackService.ChatMessageReceived += OnChatMessageReceived;

            var callbackInstance = new InstanceContext(callbackService);

            var uri = string.Format("net.tcp://{0}:{1}/MultiplayerServer/", host, port);
            var endpointAddress = new EndpointAddress(new Uri(uri));
            var netTcpBinding = new NetTcpBinding(SecurityMode.None);

            _factory = new DuplexChannelFactory<IMultiplayerService>(callbackInstance, netTcpBinding, endpointAddress);
        }

        private void OnChatMessageReceived(string player, string message)
        {
            ChatMessageReceived.Raise(player, message);
        }

        public string Join(string player)
        {
            var channel = _factory.CreateChannel();

            return channel.Join(player);
        }

        public void SendChatMessage(string playertoken, string message)
        {
            var channel = _factory.CreateChannel();

            channel.SendChatMessage(playertoken, message);
        }
    }
}
