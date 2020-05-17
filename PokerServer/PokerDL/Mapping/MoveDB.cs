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
        protected override void GetModelColumns(Move model)
        {
            model.Id = (int)reader["MoveId"];
            model.PlayerId = (int)reader["PlayerId"];
            model.Operation = (Operation)(int)reader["Operation"];
        }

        protected override string GetSQLDeleteString(Move model)
        {
            return "DELETE FROM MOVE Where MoveId = " + model.Id;
        }

        protected override string GetSQLInsertString()
        {
            return "INSERT INTO MOVE " +
                "(PlayerId, Operation)" +
                "Values(@PlayerId, @Operation)";
        }

        protected override string GetSQLUpdateString(Move model)
        {
            return "UPDATE MOVE Set " +
                   "PlayerId=@PlayerId,Operation=@Operation" +
                   "Where MoveId = " + model.Id;

        }

        protected override void SetSQLParameters(Move model)
        {
            this.command.Parameters.Add("@PlayerId",
                System.Data.SqlDbType.Int);
            this.command.Parameters.Add("@Operation",
                System.Data.SqlDbType.Int);

            this.command.Parameters["@PlayerId"].Value = model.PlayerId;
            this.command.Parameters["@Operation"].Value = model.Operation;
        }
    }
}
