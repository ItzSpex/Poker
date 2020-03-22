using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerDL
{
    public class DL
    {
        public class Move
        {
            int PId { get; set; }
            Operation Operation { get; set; }
        }
        public class Login
        {
            int PId { get; set; }
            string Username { get; set; }
            string Password { get; set; }


        }
        public enum Operation
        {
            Spectate = 0,
            Call,
            Raise,
            Fold,
            Check
        };
    }
}
