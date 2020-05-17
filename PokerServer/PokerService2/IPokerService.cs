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
    [ServiceContract]
    public interface IPokerService
    {
        [OperationContract]
        ServerResponse<int> SignUp(string username, string password);
        [OperationContract]
        ServerResponse<int> Login(string username, string password);
        // If the ErrorMsg == "Player has rejoined table" - the user returns to the table he is in
        [OperationContract]
        ServerResponse<List<PokerTable>> GetExistingPokerTables();
        [OperationContract]
        ServerResponse<int> CreateTable(string username); // returns TableId
        [OperationContract]
        ServerResponse<bool> LeaveTable(int TableId, string username);
        [OperationContract]
        ServerResponse<bool> CloseTable(int TableId, string username);
        [OperationContract]
        ServerResponse<bool> JoinTable(int TableId, string username); // fails if Table closes by the time the user selects it
        [OperationContract]
        ServerResponse<bool> GetUp(int TableId, string username); //fails if Table closes or game started.
        [OperationContract]
        ServerResponse<bool> Sit(int TableId, string username); // fails if Table closes or table is full or game started
        [OperationContract]
        ServerResponse<bool> StartGame(int TableId, string username);
        [OperationContract]
        ServerResponse<TableStatus> UpdateTableStatus(int TableId, string username);

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
