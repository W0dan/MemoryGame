using System.Collections.Generic;
using System.ServiceModel;

namespace MemoryGame.Contracts
{
    [ServiceContract(CallbackContract = typeof(IPlayerCallback))]
    public interface IMultiplayerService
    {
        [OperationContract]
        string Join(string player);

        [OperationContract(IsOneWay = true)]
        void SendChatMessage(string playertoken, string message);

        [OperationContract]
        List<string> GetPlayerList(string playertoken);
    }
}
