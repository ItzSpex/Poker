using PokerBL.Models;
using PokerDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PokerService
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IPokerService
    {
        [OperationContract]
        ServerResponse<int> SignUp(string username, string password);
        [OperationContract]
        ServerResponse<int> Login(string username, string password);
        // If the ErrorMsg == "Player has rejoined table" - the user returns to the table he is in
        [OperationContract]
        ServerResponse<List<PokerTableBL>> GetExistingTables();
        [OperationContract]
        ServerResponse<bool> CreateTable(string PokerTableName, int NumOfPlayers, int MinBetAmount); 
        [OperationContract]
        ServerResponse<bool> LeaveTable();
        [OperationContract]
        ServerResponse<bool> CloseTable();
        [OperationContract]
        ServerResponse<bool> JoinTable(int TableId); // fails if Table closes by the time the user selects it
        [OperationContract]
        ServerResponse<bool> StartGame();
        [OperationContract]
        ServerResponse<List<string>> GetCurrPlayerNames();
        [OperationContract]
        ServerResponse<List<Move>> GetTableStatus();

        [OperationContract]
        ServerResponse<bool> ExecuteMove(Operation Operation, int BidAmount);


    }
    [DataContract]
    public class ServerResponse<T>
    {
        [DataMember]
        public string ErrorMsg { get; set; }
        [DataMember]
        public T Result { get; set; }
    }
}
