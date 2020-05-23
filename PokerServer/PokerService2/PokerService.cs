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
        public ServerResponse<bool> CloseTable()
        {
            ServerResponse<bool> serverResponse = new ServerResponse<bool>();
            try
            {
                serverResponse.Result = serviceHandler.CloseTable();
            }
            catch (Exception e)
            {
                serverResponse.ErrorMsg = e.Message;
            }
            return serverResponse;
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

        public ServerResponse<bool> ExecuteMove(Operation Operation, int BidAmount)
        {
            ServerResponse<bool> serverResponse = new ServerResponse<bool>();
            try
            {
                serverResponse.Result = serviceHandler.ExecuteMove(Operation, BidAmount);
            }
            catch (Exception e)
            {
                serverResponse.ErrorMsg = e.Message;
            }
            return serverResponse;
        }

        public ServerResponse<List<string>> GetCurrPlayerNames()
        {
            ServerResponse<List<string>> serverResponse = new ServerResponse<List<string>>();
            try
            {
                serverResponse.Result = serviceHandler.GetCurrPlayerNames();
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

        public ServerResponse<List<Move>> GetTableStatus()
        {
            ServerResponse<List<Move>> serverResponse = new ServerResponse<List<Move>>();
            try
            {
                serverResponse.Result = serviceHandler.GetExistingMoves();
            }
            catch (Exception e)
            {
                serverResponse.ErrorMsg = e.Message;
            }
            return serverResponse;
        }

        public ServerResponse<bool> JoinTable(int TableId)
        {
            ServerResponse<bool> serverResponse = new ServerResponse<bool>();
            try
            {
                serverResponse.Result = serviceHandler.JoinTable(TableId);
            }
            catch (Exception e)
            {
                serverResponse.ErrorMsg = e.Message;
            }
            return serverResponse;
        }

        public ServerResponse<bool> LeaveTable()
        {
            ServerResponse<bool> serverResponse = new ServerResponse<bool>();
            try
            {
                serverResponse.Result = serviceHandler.LeaveTable();
            }
            catch (Exception e)
            {
                serverResponse.ErrorMsg = e.Message;
            }
            return serverResponse;
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
        public ServerResponse<bool> StartGame()
        {
            throw new NotImplementedException();
        }

        
    }
}
