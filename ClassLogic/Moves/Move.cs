using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLogic
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Position FromPos { get; }
        public abstract Position ToPos { get; }
        public abstract void Execute(Board board);
        public virtual bool IsLegal(Board board)/// kiem tra vua bi chieu k de di chuyen
        {
            Player player = board[FromPos].Color;
            Board boardCopy = board.Copy();
            Execute(boardCopy);
            return !boardCopy.IsInCheck(player);
        }
    }
}
