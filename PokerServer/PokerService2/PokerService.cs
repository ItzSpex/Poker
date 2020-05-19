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
        private ServiceHandler serviceHandler = new ServiceHandler();
        public ServerResponse<bool> CloseTable(int TableId, int userId)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<bool> CreateTable(string PokerTableName, int NumOfPlayers, int MinBetAmount)
        {
            ServerResponse<bool> serverResponse = new ServerResponse<bool>();
            try
            {
                serverResponse.Result = serviceHandler.CreateTable(PokerTableName, NumOfPlayers, MinBetAmount);
            }
            catch (Exception e)
            {
                serverResponse.ErrorMsg = e.Message;
            }
            return serverResponse;
        }

        public ServerResponse<List<PokerTableBL>> GetExistingTables()
        {
            ServerResponse<List<PokerTableBL>> serverResponse = new ServerResponse<List<PokerTableBL>>();
            try 
            {
                serverResponse.Result = serviceHandler.GetExistingTables();
            }
            catch (Exception e)
            {
                serverResponse.ErrorMsg = e.Message;
            }
            return serverResponse;
        }

        public ServerResponse<bool> GetUp(int TableId, int userId)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<bool> JoinTable(int TableId, int userId)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<bool> LeaveTable(int TableId, int userId)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<int> Login(string username, string password)
        {
            
            ServerResponse<int> loginResponse = new ServerResponse<int>();
            try
            {
                loginResponse.Result = serviceHandler.Login(username,password);  
            }
            catch (Exception e)
            {
                loginResponse.ErrorMsg = e.Message;
            }
            return loginResponse;
        }
        
        public ServerResponse<TableStatus> PlayerMove(int TableId, int Operation)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<int> SignUp(string username, string password)
        {
            ServerResponse<int> signupResponse = new ServerResponse<int>();
            try
            {
                signupResponse.Result = serviceHandler.Signup(username, password); 
            }
            catch (Exception e)
            {
                signupResponse.ErrorMsg = e.Message;
            }
            return signupResponse;
        }

        public ServerResponse<bool> Sit(int TableId, int userId)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<bool> StartGame(int TableId, int userId)
        {
            throw new NotImplementedException();
        }

        public ServerResponse<TableStatus> UpdateTableStatus(int TableId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
