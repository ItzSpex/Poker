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
    [ServiceContract(SessionMode=SessionMode.Required)]
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
        ServerResponse<bool> CreateTable(string PokerTableName, int NumOfPlayers, int MinBetAmount); // returns TableId
        [OperationContract]
        ServerResponse<bool> LeaveTable(int TableId, int userId);
        [OperationContract]
        ServerResponse<bool> CloseTable(int TableId, int userId);
        [OperationContract]
        ServerResponse<bool> JoinTable(int TableId, int userId); // fails if Table closes by the time the user selects it
        [OperationContract]
        ServerResponse<bool> GetUp(int TableId, int userId); //fails if Table closes or game started.
        [OperationContract]
        ServerResponse<bool> Sit(int TableId, int userId); // fails if Table closes or table is full or game started
        [OperationContract]
        ServerResponse<bool> StartGame(int TableId, int userId);
        [OperationContract]
        ServerResponse<TableStatus> UpdateTableStatus(int TableId, int userId);

        [OperationContract]
        ServerResponse<TableStatus> PlayerMove(int TableId, int Operation);
        

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
