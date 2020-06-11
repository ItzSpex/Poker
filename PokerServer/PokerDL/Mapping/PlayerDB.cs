using PokerBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PokerDL.Mapping
{
    public class PlayerDB : BaseDB<Player>
    {
        public Player GetPlayerByTableId(int TableId, int PlayerId)
        {
            command.CommandText = "SELECT * FROM Player WHERE PlayerId = " + PlayerId + " AND PokerTableId = " + TableId;
            var l = Select();
            if (l.Count == 1)
                return l[0];
            return null;
        }
        public void DeletePlayersByTableId(int PokerTableId)
        {
            command.CommandText = "SELECT * FROM Player Where PokerTableId = " + PokerTableId;
            var l = Select();
            if(l.Count > 0)
            {
                for (int i = 0; i < l.Count; i++)
                {
                    Delete(l[i]);
                }
            }
        }
        protected override void GetModelColumns(Player model)
        {
            model.PlayerId = (int)reader["PlayerId"];
            model.PlayerName = reader["PlayerName"].ToString();
            model.ChipsOnTable = (int)reader["ChipsOnTable"];
            model.FirstCard = reader["FirstCard"].ToString();
            model.SecondCard = reader["SecondCard"].ToString();
            model.PokerTableId = (int)reader["PokerTableId"];
        }

        protected override string GetSQLDeleteString(Player model)
        {
            return "DELETE FROM PLAYER Where PlayerId = " + model.PlayerId + " AND PokerTableId = " + model.PokerTableId;
        }

        protected override string GetSQLInsertString()
        {
            return "INSERT INTO PLAYER " +
                "(PlayerId ,PlayerName,ChipsOnTable, FirstCard, SecondCard, PokerTableId) " +
                "Values(@PlayerId ,@PlayerName,@ChipsOnTable, @FirstCard, @SecondCard, @PokerTableId)";
        }

        protected override string GetSQLUpdateString(Player model)
        {
            return "UPDATE PLAYER Set " +
                "PlayerName=@PlayerName, ChipsOnTable=@ChipsOnTable,FirstCard=@FirstCard, SecondCard=@SecondCard " +
                "Where PlayerId = " + model.PlayerId + " AND PokerTableId = " + model.PokerTableId;
        }

        protected override void SetSQLParameters(Player model)
        {
            this.command.Parameters.Add("@PlayerId",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@PlayerName",
                System.Data.SqlDbType.NVarChar);
            this.command.Parameters.Add("@ChipsOnTable",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@FirstCard",
                System.Data.SqlDbType.NVarChar);
            this.command.Parameters.Add("@SecondCard",
                System.Data.SqlDbType.NVarChar);
            this.command.Parameters.Add("@PokerTableId",
                System.Data.SqlDbType.Int);

            this.command.Parameters["@PlayerId"].Value = model.PlayerId;
            this.command.Parameters["@PlayerName"].Value = model.PlayerName;
            this.command.Parameters["@ChipsOnTable"].Value = model.ChipsOnTable;
            this.command.Parameters["@FirstCard"].Value = model.FirstCard;
            this.command.Parameters["@SecondCard"].Value = model.SecondCard;
            this.command.Parameters["@PokerTableId"].Value = model.PokerTableId;
        }

        
    }
}
