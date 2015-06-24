using System.ServiceModel;

namespace MemoryGame.Contracts
{
    public interface IPlayerCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnChatMessageReceived(string player, string message);

        [OperationContract(IsOneWay = true)]
        void OnPlayerJoined(string player);

        [OperationContract(IsOneWay = true)]
        void OnGameStarted(int rows, int columns);

        [OperationContract(IsOneWay = true)]
        void OnYourTurn();

        [OperationContract(IsOneWay = true)]
        void OnPlayerIsOnTurn(string player);
    }
}