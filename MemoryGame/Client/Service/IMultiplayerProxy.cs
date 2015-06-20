using System;
using MemoryGame.Contracts;

namespace MemoryGame.Client.Service
{
    public interface IMultiplayerProxy : IMultiplayerService
    {
        event Action<string, string> ChatMessageReceived;
        void Initialise(string host, string port);
    }
}