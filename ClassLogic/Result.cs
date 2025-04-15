using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLogic
{
    public class Result
    {
        public EndReason EndReason { get; }
        public Player Winner { get; }
        public Result( Player winner, EndReason endReason)
        {
            EndReason = endReason;
            Winner = winner;
        }
        public static Result Win(Player winner)
        {
            return new Result( winner, EndReason.Checkmate);
        }
        public static Result Draw(EndReason endReason)
        {
            return new Result(Player.None, endReason);
        }
    }
}
