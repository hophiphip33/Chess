using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLogic
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;
        public override Player Color { get; }
        public Knight(Player color)
        {
            Color = color;
        }
        public override Piece Copy()
        {
            Knight copy = new Knight(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }
        // ma di chuyen chu L
     private static IEnumerable<Position> PotentialToPositions(Position from)
        {
           foreach (Direction vdir in new Direction[] { Direction.North, Direction.South })
            {
                foreach (Direction hdir in new Direction[] { Direction.East, Direction.West })
                {
                    yield return from + 2*vdir + hdir;
                    yield return from + 2 * hdir + vdir;
                }
            }
        }
        // lay vi tri co the di chuyen
        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            return PotentialToPositions(from)
                .Where(pos => Board.IsInside(pos) && (board.IsEmpty(pos) || board[pos].Color != Color));
        }
        // lay ra cac nuoc di
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositions(from, board)
                .Select(pos => new NormalMove(from, pos));
        }
    }
}
