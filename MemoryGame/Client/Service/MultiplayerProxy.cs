using System;
using System.Collections.Generic;
using System.ServiceModel;
using MemoryGame.Contracts;
using MemoryGame.Extensions;

namespace MemoryGame.Client.Service
{
    public class MultiplayerProxy : IMultiplayerService
    {
        public event Action<string, string> ChatMessageReceived;
        public event Action<string> PlayerJoined;
        public event Action<int, int> GameStarted;
        public event Action YourTurn;
        public event Action<string> PlayerIsOnTurn;

        private readonly ChannelFactory<IMultiplayerService> _factory;

        public MultiplayerProxy(string host, string port)
        {
            var callbackService = new CallbackService();

            callbackService.ChatMessageReceived += (player, message) => ChatMessageReceived.Raise(player, message);
            callbackService.PlayerJoined += player => PlayerJoined.Raise(player);
            callbackService.GameStarted += (rows, columns) => GameStarted.Raise(rows, columns);
            callbackService.YourTurn += () => YourTurn.Raise();
            callbackService.PlayerIsOnTurn += player => PlayerIsOnTurn.Raise(player);

            var callbackInstance = new InstanceContext(callbackService);

            var uri = string.Format("net.tcp://{0}:{1}/MultiplayerServer/", host, port);
            var endpointAddress = new EndpointAddress(new Uri(uri));
            var netTcpBinding = new NetTcpBinding(SecurityMode.None);

            _factory = new DuplexChannelFactory<IMultiplayerService>(callbackInstance, netTcpBinding, endpointAddress);
        }

        public string Join(string player)
        {
            return Execute(channel => channel.Join(player));
        }

        public void SendChatMessage(string playertoken, string message)
        {
            Execute(channel => channel.SendChatMessage(playertoken, message));
        }

        public List<string> GetPlayerList(string playertoken)
        {
            return Execute(channel => channel.GetPlayerList(playertoken));
        }

        public void ReadyToRumble(string playertoken)
        {
            Execute(channel => channel.ReadyToRumble(playertoken));
        }

        public void StartGame(string playertokenFrom, int rows, int columns)
        {
            Execute(channel => channel.StartGame(playertokenFrom, rows, columns));
        }

        public void CardClicked(string playertoken, int row, int column)
        {
            Execute(channel => channel.CardClicked(playertoken, row, column));
        }

        private TResult Execute<TResult>(Func<IMultiplayerService, TResult> func)
        {
            var channel = _factory.CreateChannel();

            return func(channel);

            //var result = defaultResult;

            //Task.Factory.StartNew(() => result = func(channel));

            //while (EqualityComparer<TResult>.Default.Equals(result, defaultResult))
            //{
            //    Thread.Sleep(100);
            //}

            //return result;
        }

        private void Execute(Action<IMultiplayerService> action)
        {
            var channel = _factory.CreateChannel();

            action(channel);

            //Task.Factory.StartNew(() => action(channel));
        }
    }
}
