using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoryGame.Common;

namespace MemoryGame.Server
{
    public class SubscriberCollection<TSubscriberCallback>
    {
        private readonly List<Subscriber<TSubscriberCallback>> _subscribers = new List<Subscriber<TSubscriberCallback>>();

        public Subscriber<TSubscriberCallback> Add(string name, TSubscriberCallback callback)
        {
            try
            {
                if (_subscribers.Any(s => s.Name == name))
                {
                    return null;
                }

                var subscriber = new Subscriber<TSubscriberCallback>
                {
                    Token = Guid.NewGuid().ToString(),
                    Name = name,
                    Callback = callback
                };
                _subscribers.Add(subscriber);
                return subscriber;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Send(string playerTokenFrom, Action<string, TSubscriberCallback> action)
        {
            Task.Factory.StartNew(() =>
            {
                var subscriberFrom = _subscribers.Single(s => s.Token == playerTokenFrom);

                var subscribersToRemove = new List<Subscriber<TSubscriberCallback>>();

                foreach (var subscriber in _subscribers.Where(sub => sub.Token != playerTokenFrom))
                {
                    try
                    {
                        action(subscriberFrom.Name, subscriber.Callback);
                    }
                    catch
                    {
                        subscribersToRemove.Add(subscriber);
                    }
                }

                foreach (var subscriber in subscribersToRemove)
                {
                    _subscribers.Remove(subscriber);
                }
            });
        }

        public List<string> GetPlayerNames()
        {
            return _subscribers.Select(subscriber => subscriber.Name).ToList();
        }

        public RoundRobin<string> GetPlayersRoundRobin()
        {
            var tokens = _subscribers.Select(subscriber => subscriber.Token);
            return new RoundRobin<string>(tokens);
        }
    }
}