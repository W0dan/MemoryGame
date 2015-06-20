using System.ServiceModel;

namespace MemoryGame.Contracts
{
    public interface IPlayerCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnChatMessageReceived(string player, string message);

        [OperationContract(IsOneWay = true)]
        void OnPlayerJoined(string player);
    }
}