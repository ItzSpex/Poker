using PokerBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerDL.Mapping
{
    public class PlayerDB : BaseDB<Player>
    {
        protected override void GetModelColumns(Player model)
        {
            model.Id = (int)reader["PlayerId"];
            model.PlayerName = reader["PlayerName"].ToString();
            model.PokerTableId = (int)reader["PokerTableId"];
            model.ChipsOnTable = (int)reader["ChipsOnTable"];
            model.FirstCard = null;
            if (reader["Card1"] != DBNull.Value)
            {
                model.FirstCard = reader["Card1"].ToString();
            }
            model.SecondCard = null;
            if (reader["Card2"] != DBNull.Value)
            {
                model.SecondCard = reader["Card2"].ToString();
            }
        }

        protected override string GetSQLDeleteString(Player model)
        {
            return "DELETE FROM PLAYER Where PlayerId = " + model.Id;
        }

        protected override string GetSQLInsertString()
        {
            return "INSERT INTO PLAYER " +
                "(PlayerId, PlayerName, PokerTableId,ChipsOnTable, Card1, Card2)" +
                "Values(@PlayerId, @PlayerName, @PokerTableId,@ChipsOnTable, @Card1, @Card2)";
        }

        protected override string GetSQLUpdateString(Player model)
        {
            return "UPDATE PLAYER Set " +
                "PlayerName=@PlayerName, PokerTableId=@PokerTableId, ChipsOnTable=@ChipsOnTable,Card1=@Card1, Card2=@Card" +
                "Where PlayerId = " + model.Id;
        }

        protected override void SetSQLParameters(Player model)
        {
            this.command.Parameters.Add("@PlayerId",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@PlayerName",
                System.Data.SqlDbType.NVarChar);
            this.command.Parameters.Add("@PokerTableId",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@ChipsOnTable",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@Card1",
                System.Data.SqlDbType.NVarChar);
            this.command.Parameters.Add("@Card2",
                System.Data.SqlDbType.NVarChar);

            this.command.Parameters["@PlayerId"].Value = model.Id;
            this.command.Parameters["@PlayerName"].Value = model.PlayerName;
            this.command.Parameters["@PokerTableId"].Value = model.PokerTableId;
            this.command.Parameters["@ChipsOnTable"].Value = model.ChipsOnTable;
            if (model.FirstCard == null)
                this.command.Parameters["@Card1"].Value = DBNull.Value;
            else
                this.command.Parameters["@Card1"].Value = model.FirstCard;
            if (model.SecondCard == null)
                this.command.Parameters["@Card2"].Value = DBNull.Value;
            else
                this.command.Parameters["@Card2"].Value = model.SecondCard;
        }
    }
}
