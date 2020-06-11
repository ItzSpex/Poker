using PokerBL.Models;
using PokerDL.Models;
using PokerDL.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerBL.ORM
{
    public class Database
    {
        public static UserInfo GetUserByCredentials(UserInfo requstedUser)
        {
            UserInfoDB userInfoDB = new UserInfoDB();
            UserInfo u = userInfoDB.GetByUsername(requstedUser.Username);
            if (u == null)
            {
                throw new Exception("User doesn't exist");
            }
            if (u.Password != requstedUser.Password)
            {
                throw new Exception("Wrong password");
            }
            return u;
        }
        public static UserInfo GetUserById(int userId)
        {
            UserInfoDB userInfoDB = new UserInfoDB();
            UserInfo u = userInfoDB.GetById(userId);
            if (u == null)
            {
                throw new Exception("User doesn't exist");
            }
            return u;
        }
        public static Player GetPlayerById(int PlayerId, int TableId)
        {
            PlayerDB playerDB = new PlayerDB();
            Player p = playerDB.GetPlayerByTableId(TableId, PlayerId);
            if (p == null)
            {
                throw new Exception("Player doesn't exist in table");
            }
            return p;
        }
        public static void InsertUser(UserInfo newUser)
        {
            UserInfoDB userInfoDB = new UserInfoDB();
            UserInfo u = userInfoDB.GetByUsername(newUser.Username);
            if (u != null)
            {
                throw new Exception("User exists");
            }
            userInfoDB.IdentityInsert(newUser);
        }
        public static void UpdateUser(UserInfo updatedUser)
        {
            UserInfoDB userInfoDB = new UserInfoDB();
            UserInfo u = userInfoDB.GetByUsername(updatedUser.Username);
            if (u == null)
            {
                throw new Exception("User doesn't exist");
            }
            userInfoDB.Update(updatedUser);
        }
        public static void UpdateTable(PokerTable updatedPokerTable)
        {
            PokerTableDB pokerTableDB = new PokerTableDB();
            PokerTable pokerTable = pokerTableDB.GetTableById(updatedPokerTable.Id);
            if (pokerTable == null)
            {
                throw new Exception("Table doesn't exist");
            }
            pokerTableDB.Update(updatedPokerTable);
        }
        public static int InsertTable(PokerTable newPokerTable)
        {
            PokerTableDB pokerTableDB = new PokerTableDB();
            PokerTable pokerTable = pokerTableDB.GetTableById(newPokerTable.Id);
            if (pokerTable == null)
            {
                pokerTableDB.IdentityInsert(newPokerTable);
            }
            else
            {
                UpdateTable(newPokerTable);
            }
            return newPokerTable.Id;
        }
        public static void ClearEmptyTables()
        {
            PokerTableDB pokerTableDB = new PokerTableDB();
            PlayerDB playerDB = new PlayerDB();
            List<PokerTable> pokerTables = pokerTableDB.GetEmptyTables();
            if (pokerTables != null)
            {
                foreach (PokerTable pokerTable in pokerTables)
                {
                    playerDB.DeletePlayersByTableId(pokerTable.Id);
                }

                for (int i = 0; i < pokerTables.Count; i++)
                {
                    DeleteTable(pokerTables[i]);
                }
            }
        }
        public static int InsertPlayer(Player newPlayer)
        {
            PlayerDB playerDB = new PlayerDB();
            playerDB.Insert(newPlayer);
            return newPlayer.PlayerId;
        }
        public static void InsertMove(Move newMove)
        {
            MoveDB moveDB = new MoveDB();
            moveDB.Insert(newMove);
        }
        public static List<PokerTable> GetAllTables(int PlayerId)
        {
            PokerTableDB pokerTableDB = new PokerTableDB();
            List<PokerTable> pokerTables = pokerTableDB.GetTables(PlayerId);
            if (pokerTables == null)
            {
                throw new Exception("No tables exist");
            }
            return pokerTables;
        }
        public static void DeleteTable(PokerTable requestedTable)
        {
            PokerTableDB pokerTableDB = new PokerTableDB();
            pokerTableDB.Delete(requestedTable);
        }
        public static List<Move> GetAllMoves(int TableId)
        {
            MoveDB moveDB = new MoveDB();
            List<Move> moves = moveDB.GetMoves(TableId);
            if (moves == null)
            {
                throw new Exception("No moves exist");
            }
            return moves;
        }
    }
}
