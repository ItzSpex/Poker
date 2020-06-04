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
        private readonly ServiceHandler serviceHandler = new ServiceHandler();

        public ServerResponse<PokerTableBL> CreateTable(string PokerTableName, int NumOfPlayers, int MinBetAmount)
        {
            ServerResponse<PokerTableBL> serverResponse = new ServerResponse<PokerTableBL>();
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

        public ServerResponse<TableStatus> GetTableStatus()
        {
            ServerResponse<TableStatus> serverResponse = new ServerResponse<TableStatus>();
            try
            {
                serverResponse.Result = serviceHandler.GetTableStatus();
            }
            catch (Exception e)
            {
                serverResponse.ErrorMsg = e.Message;
            }
            return serverResponse;
        }

        public ServerResponse<bool> HasGameStarted()
        {
            ServerResponse<bool> serverResponse = new ServerResponse<bool>();
            try
            {
                serverResponse.Result = serviceHandler.HasGameStarted();
            }
            catch (Exception e)
            {
                serverResponse.ErrorMsg = e.Message;
            }
            return serverResponse;
        }

        public ServerResponse<PokerTableBL> JoinTable(int TableId)
        {
            ServerResponse<PokerTableBL> serverResponse = new ServerResponse<PokerTableBL>();
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

        public ServerResponse<bool> Logout()
        {
            ServerResponse<bool> logoutResponse = new ServerResponse<bool>();
            try
            {
                logoutResponse.Result = serviceHandler.Logout();
            }
            catch (Exception e)
            {
                logoutResponse.ErrorMsg = e.Message;
            }
            return logoutResponse;
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
        public ServerResponse<StartGameStatus> StartGame()
        {
            ServerResponse<StartGameStatus> serverResponse = new ServerResponse<StartGameStatus>();
            try
            {
                serverResponse.Result = serviceHandler.StartGame();
            }
            catch (Exception e)
            {
                serverResponse.ErrorMsg = e.Message;
            }
            return serverResponse;
        }

        public ServerResponse<List<PlayerBL>> UpdatePlayers()
        {
            ServerResponse<List<PlayerBL>> serverResponse = new ServerResponse<List<PlayerBL>>();
            try
            {
                serverResponse.Result = serviceHandler.GetPlayers();
            }
            catch(Exception e)
            {
                serverResponse.ErrorMsg = e.Message;
            }
            return serverResponse;
        }
    }
}
