using PokerBL.Models;
using PokerBL.ORM;
using PokerDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PokerService
{
    public class PokerService : IPokerService
    {
        public ServerResponse<int> CreateRoom(string username)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<bool> JoinRoom(int roomId, string username)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<List<Room>> Leave(int roomId, string username)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<int> Login(string Username, string Password)
        {
            ServerResponse<int> loginResponse =
                new ServerResponse<int>();
            loginResponse.ErrorMsg = null;
            try
            {
                loginResponse.Result = Database.GetUserByCredentials(new User(Username, Password));
            }
            catch (Exception e)
            {
                loginResponse.ErrorMsg = e.Message;
            }
            return loginResponse;
        }

        public ServerResponse<TableStatus> PlayerMove(int roomId, Move move)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<List<Room>> SignUp(User user)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<bool> Sit(int roomId, string username)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<TableStatus> UpdateTableStatus(int roomId, string username)
        {
            throw new NotImplementedException();
        }
    }
}
