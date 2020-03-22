using PokerBL.Models;
using PokerDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PokerService
{
    [ServiceContract]
    interface IPokerService
    {
        [OperationContract]
        ServerResponse<List<Room>> Login(String username, String password);
        // If the ErrorMsg == "Player has rejoined table" - the user returns to the table he is in

        [OperationContract]
        ServerResponse<List<Room>> SignIn(User user);

        [OperationContract]
        ServerResponse<bool> JoinRoom(int roomId, string username); // fails if room closes by the time the user selects it

        [OperationContract]
        ServerResponse<int> CreateRoom(string username); // returns roomId

        [OperationContract]
        ServerResponse<bool> Sit(int roomId, string username); // fails if room closes or table is full or game started

        [OperationContract]
        ServerResponse<List<Room>> Leave(int roomId, string username);

        [OperationContract]
        ServerResponse<TableStatus> UpdateTableStatus(int roomId, string username);

        [OperationContract]
        ServerResponse<TableStatus> PlayerMove(int roomId, Move move);


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
