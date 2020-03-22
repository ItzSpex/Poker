using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static PokerBL.BL;
using static PokerDL.DL;

namespace PokerService
{
    public class PokerService : IPokerService
    {
        public ServerResponse<int> CreateRoom(string login)
        {
            
            throw new NotImplementedException();
        }

        public ServerResponse<bool> JoinRoom(int roomId, string login)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<List<Room>> Leave(int roomId, string login)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<List<Room>> Login(string login, string password)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<TableStatus> PlayerMove(int roomId, Move move)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<List<Room>> SignIn(Login login)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<bool> Sit(int roomId, string login)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<TableStatus> UpdateTableStatus(int roomId, string login)
        {
            throw new NotImplementedException();
        }
    }
}
