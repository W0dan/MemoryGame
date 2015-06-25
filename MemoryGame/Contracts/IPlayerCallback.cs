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

        [OperationContract(IsOneWay = true)]
        void OnFirstCardSelected(SelectedCard card);

        [OperationContract(IsOneWay = true)]
        void OnSecondCardSelected(SelectedCard card);

        [OperationContract(IsOneWay = true)]
        void OnSecondCardMatches(SelectedCard firstCard, SelectedCard secondCard);

        [OperationContract(IsOneWay = true)]
        void OnSecondCardDoesntMatch(SelectedCard firstCard, SelectedCard secondCard);
    }
}