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
        public ServerResponse<bool> CloseRoom(int roomId, string username)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<int> CreateRoom(string username)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<List<PokerTable>> GetExistingPokerTables()
        {
            throw new NotImplementedException();
        }

        public ServerResponse<bool> GetUp(int roomId, string username)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<bool> JoinRoom(int roomId, string username)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<bool> LeaveRoom(int roomId, string username)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<int> Login(string username, string password)
        {
            ServerResponse<int> loginResponse =
                new ServerResponse<int>();
            loginResponse.ErrorMsg = null;
            try
            {
                loginResponse.Result = Database.GetUserByCredentials(new UserInfo(username, password));
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

        public ServerResponse<int> SignUp(string username, string password)
        {
            UserInfo user = new UserInfo(username, password);
            ServerResponse<int> signupResponse = new ServerResponse<int>();
            signupResponse.ErrorMsg = null;
            try
            {
                Database.InsertUser(user);
                signupResponse.Result = user.Id;
            }
            catch (Exception e)
            {
                signupResponse.ErrorMsg = e.Message;
            }
            return signupResponse;
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
