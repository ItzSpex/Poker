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
        protected override void GetModelColumns(PokerTable model)
        {
            model.Id = (int)reader["PokerTableId"];
            model.NumOfPlayers = (int)reader["numOfPlayers"];
            model.TablePot = (int)reader["tablePot"];
            model.CurrTurn = (int)reader["currentTurn"];
            model.FirstCard = (Card)reader["Card1"];
            model.SecondCard = (Card)reader["Card2"];
            model.ThirdCard = (Card)reader["Card3"];
            model.FourthCard = (Card)reader["Card4"];
            model.FifthCard = (Card)reader["Card5"];
        }

        protected override string GetSQLDeleteString(PokerTable model)
        {
            return "DELETE FROM POKERTABLE Where PokerTableId = " + model.Id;
        }

        protected override string GetSQLInsertString()
        {
            return "INSERT INTO POKERTABLE " +
                "(NumOfPlayers, TablePot, CurrTurn, FirstCard, SecondCard, ThirdCard, FourthCard, FifthCard)" +
                "Values(@NumOfPlayers, @TablePot, @CurrTurn, @FirstCard, @SecondCard, @ThirdCard, @FourthCard, @FifthCard)";
        }

        protected override string GetSQLUpdateString(PokerTable model)
        {
            return "UPDATE POKERTABLE Set " +
                   "NumOfPlayers=@NumOfPlayers, TablePot=@TablePot, CurrTurn=@CurrTurn, FirstCard=@FirstCard, SecondCard=@SecondCard, " +
                   "ThirdCard=@ThirdCard, FourthCard=@FourthCard, FifthCard=@FifthCard" +
                   "Where PokerTableId = " + model.Id;
        }

        protected override void SetSQLParameters(PokerTable model)
        {
            this.command.Parameters.Add("@PokerTableId",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@NumOfPlayers",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@TablePot",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@CurrTurn",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@FirstCard",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@SecondCard",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@ThirdCard",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@FourthCard",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@FifthCard",
                System.Data.SqlDbType.Int);
        }
    }
}
