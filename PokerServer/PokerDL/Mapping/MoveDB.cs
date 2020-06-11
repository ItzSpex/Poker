using PokerDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerDL.Mapping
{
    public class MoveDB : BaseDB<Move>
    {
        public List<Move> GetMoves(int PokerTableId)
        {
            command.CommandText = "SELECT * FROM MOVE WHERE PokerTableId = " + PokerTableId;
            var l = Select();
            return l;
        }
        protected override void GetModelColumns(Move model)
        {
            model.PlayerId = (int)reader["PlayerId"];
            model.PokerTableId = (int)reader["PokerTableId"];
            model.MoveNumber = (int)reader["MoveNumber"];
            model.BidAmount = (int)reader["BidAmount"];
            model.Operation = (Operation)(int)reader["Operation"];
        }

        protected override string GetSQLDeleteString(Move model)
        {
            return null;
        }

        protected override string GetSQLInsertString()
        {
            return "INSERT INTO MOVE " +
                "(PlayerId, PokerTableId, MoveNumber, BidAmount, Operation)" +
                "Values(@PlayerId, @PokerTableId, @MoveNumber, @BidAmount, @Operation)";
        }

        protected override string GetSQLUpdateString(Move model)
        {
            return null;

        }

        protected override void SetSQLParameters(Move model)
        {
            this.command.Parameters.Add("@PlayerId",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@PokerTableId",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@MoveNumber",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@BidAmount",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@Operation",
                System.Data.SqlDbType.Int);

            this.command.Parameters["@PlayerId"].Value = model.PlayerId;
            this.command.Parameters["@PokerTableId"].Value = model.PokerTableId;
            this.command.Parameters["@MoveNumber"].Value = model.MoveNumber;
            this.command.Parameters["@BidAmount"].Value = model.BidAmount;
            this.command.Parameters["@Operation"].Value = model.Operation;
        }
    }
}
