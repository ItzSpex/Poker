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
        public PokerTable GetTableByName(string TableName)
        {
            command.CommandText = "SELECT * FROM POKERTABLE WHERE PokerTableName ='" + TableName + "'";
            var l = Select();
            if(l.Count == 1)
                return l[0];
            return null;
        }
        public List<PokerTable> GetTables()
        {
            command.CommandText = "SELECT * FROM POKERTABLE";
            var l = Select();
            if (l.Count > 0)
                return l;
            return null;
        }
        protected override void GetModelColumns(PokerTable model)
        {
            model.Id = (int)reader["PokerTableId"];
            model.PokerTableName = reader["PokerTableName"].ToString();
            model.TablePot = (int)reader["TablePot"];
            model.MinBet = (int)reader["MinBet"];
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
            model.ThirdCard = null;
            if (reader["Card3"] != DBNull.Value)
            {
                model.ThirdCard = reader["Card3"].ToString();
            }
            model.FourthCard = null;
            if (reader["Card4"] != DBNull.Value)
            {
                model.FourthCard = reader["Card4"].ToString();
            }
            model.FifthCard = null;
            if (reader["Card5"] != DBNull.Value)
            {
                model.FifthCard = reader["Card5"].ToString();
            }
        }

        protected override string GetSQLDeleteString(PokerTable model)
        {
            return "DELETE FROM POKERTABLE Where PokerTableId = " + model.Id;
        }

        protected override string GetSQLInsertString()
        {
            return "INSERT INTO POKERTABLE " +
                "(PokerTableName, TablePot, MinBet, FirstCard, SecondCard, ThirdCard, FourthCard, FifthCard)" +
                "Values(@PokerTableName, @TablePot, @MinBet, @FirstCard, @SecondCard, @ThirdCard, @FourthCard, @FifthCard)";
        }

        protected override string GetSQLUpdateString(PokerTable model)
        {
            return "UPDATE POKERTABLE Set " +
                   "PokerTableName=@PokerTableName, TablePot=@TablePot, MinBet=@MinBet ,FirstCard=@FirstCard, SecondCard=@SecondCard,ThirdCard=@ThirdCard," +
                   "FourthCard=@FourthCard, FifthCard=@FifthCard" +
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
                System.Data.SqlDbType.Bit);
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

            this.command.Parameters["@PokerTableId"].Value = model.Id;
            this.command.Parameters["@PokerTableName"].Value = model.PokerTableName;
            this.command.Parameters["@TablePot"].Value = model.TablePot;
            this.command.Parameters["@MinBet"].Value = model.MinBet;
            if (model.FirstCard == null)
                this.command.Parameters["@Card1"].Value = DBNull.Value;
            else
                this.command.Parameters["@Card1"].Value = model.FirstCard;
            if (model.SecondCard == null)
                this.command.Parameters["@Card2"].Value = DBNull.Value;
            else
                this.command.Parameters["@Card2"].Value = model.SecondCard;
            if (model.ThirdCard == null)
                this.command.Parameters["@Card3"].Value = DBNull.Value;
            else
                this.command.Parameters["@Card3"].Value = model.ThirdCard;
            if (model.FourthCard == null)
                this.command.Parameters["@Card4"].Value = DBNull.Value;
            else
                this.command.Parameters["@Card4"].Value = model.FourthCard;
            if (model.FifthCard == null)
                this.command.Parameters["@Card5"].Value = DBNull.Value;
            else
                this.command.Parameters["@Card5"].Value = model.FifthCard;
        }
    }
}
