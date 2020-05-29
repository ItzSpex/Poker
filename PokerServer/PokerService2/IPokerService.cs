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
        [OperationContract]
        ServerResponse<bool> Logout();
        [OperationContract]
        ServerResponse<List<PokerTableBL>> GetExistingTables();
        [OperationContract]
        ServerResponse<bool> CreateTable(string PokerTableName, int NumOfPlayers, int MinBetAmount); 
        [OperationContract]
        ServerResponse<bool> LeaveTable();
        [OperationContract]
        ServerResponse<bool> JoinTable(int TableId); 
        [OperationContract]
        ServerResponse<bool> StartGame();
        [OperationContract]
        ServerResponse<string> GetCurrPlayerNames();
        [OperationContract]
        ServerResponse<TableStatus> GetTableStatus();
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
