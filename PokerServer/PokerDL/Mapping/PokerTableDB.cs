using PokerBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerDL.Mapping
{
    public class PokerTableDB : BaseDB<PokerTable>
    {
        public PokerTable GetTableById(int PokerTableId)
        {
            command.CommandText = "SELECT * FROM POKERTABLE WHERE PokerTableId = " + PokerTableId;
            var l = Select();
            if (l.Count == 1)
                return l[0];
            return null;
        }
        public List<PokerTable> GetTables(int PlayerId)
        {
            command.CommandText = "SELECT * FROM POKERTABLE WHERE Player1Id = " + PlayerId + " OR Player2Id = " + PlayerId + " OR Player3Id = " + PlayerId
            + " OR Player4Id = " + PlayerId + " OR Player5Id = " + PlayerId;
            var l = Select();
            return l;
        }
        protected override void GetModelColumns(PokerTable model)
        {
            model.Id = (int)reader["PokerTableId"];
            model.PokerTableName = reader["PokerTableName"].ToString();
            model.TablePot = (int)reader["TablePot"];
            model.MinBet = (int)reader["MinBet"];
            model.DealerId = (int)reader["DealerId"];
            model.FirstCard = reader["FirstCard"].ToString();
            model.SecondCard = reader["SecondCard"].ToString();
            model.ThirdCard = reader["ThirdCard"].ToString();
            model.FourthCard = reader["FourthCard"].ToString();
            model.FifthCard = reader["FifthCard"].ToString();
            model.Player1Id = (int)reader["Player1Id"];
            model.Player2Id = (int)reader["Player2Id"];
            model.Player3Id = (int)reader["Player3Id"];
            model.Player4Id = (int)reader["Player4Id"];
            model.Player5Id = (int)reader["Player5Id"];
        }

        protected override string GetSQLDeleteString(PokerTable model)
        {
            return "DELETE FROM POKERTABLE Where PokerTableId = " + model.Id;
        }

        protected override string GetSQLInsertString()
        {
            return "INSERT INTO POKERTABLE " +
                "(PokerTableName, TablePot, MinBet, DealerId, FirstCard, SecondCard, ThirdCard, FourthCard, FifthCard, Player1Id, Player2Id, Player3Id, Player4Id, Player5Id)" +
                "Values(@PokerTableName, @TablePot, @MinBet, @DealerId, @FirstCard, @SecondCard, @ThirdCard, @FourthCard, @FifthCard, @Player1Id, @Player2Id, @Player3Id, @Player4Id, @Player5Id)";
        }

        protected override string GetSQLUpdateString(PokerTable model)
        {
            return "UPDATE POKERTABLE Set " +
                   "PokerTableName=@PokerTableName, TablePot=@TablePot, MinBet=@MinBet, DealerId=@DealerId ,FirstCard=@FirstCard, SecondCard=@SecondCard," +
                   "ThirdCard=@ThirdCard,FourthCard=@FourthCard, FifthCard=@FifthCard, Player1Id=@Player1Id, Player2Id=@Player2Id, Player3Id=@Player3Id," +
                   "Player4Id=@Player4Id, Player5Id=@Player5Id " +
                   "Where PokerTableId = " + model.Id;
        }

        protected override void SetSQLParameters(PokerTable model)
        {
            this.command.Parameters.Add("@PokerTableId",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@PokerTableName",
                System.Data.SqlDbType.NVarChar);
            this.command.Parameters.Add("@TablePot",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@MinBet",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@DealerId",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@FirstCard",
                System.Data.SqlDbType.NVarChar);
            this.command.Parameters.Add("@SecondCard",
                System.Data.SqlDbType.NVarChar);
            this.command.Parameters.Add("@ThirdCard",
                System.Data.SqlDbType.NVarChar);
            this.command.Parameters.Add("@FourthCard",
                System.Data.SqlDbType.NVarChar);
            this.command.Parameters.Add("@FifthCard",
                System.Data.SqlDbType.NVarChar);
            this.command.Parameters.Add("@Player1Id",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@Player2Id",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@Player3Id",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@Player4Id",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@Player5Id",
                System.Data.SqlDbType.Int);

            this.command.Parameters["@PokerTableId"].Value = model.Id;
            this.command.Parameters["@PokerTableName"].Value = model.PokerTableName;
            this.command.Parameters["@TablePot"].Value = model.TablePot;
            this.command.Parameters["@MinBet"].Value = model.MinBet;
            this.command.Parameters["@DealerId"].Value = model.DealerId;
            this.command.Parameters["@FirstCard"].Value = model.FirstCard;
            this.command.Parameters["@SecondCard"].Value = model.SecondCard;
            this.command.Parameters["@ThirdCard"].Value = model.ThirdCard;
            this.command.Parameters["@FourthCard"].Value = model.FourthCard;
            this.command.Parameters["@FifthCard"].Value = model.FifthCard;
            this.command.Parameters["@Player1Id"].Value = model.Player1Id;
            this.command.Parameters["@Player2Id"].Value = model.Player2Id;
            this.command.Parameters["@Player3Id"].Value = model.Player3Id;
            this.command.Parameters["@Player4Id"].Value = model.Player4Id;
            this.command.Parameters["@Player5Id"].Value = model.Player5Id;
        }
    }
}
