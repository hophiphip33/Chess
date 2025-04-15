using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLogic
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override Player Color { get; }
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,
            Direction.NorthEast,
            Direction.East,
            Direction.SouthEast,
            Direction.South,
            Direction.SouthWest,
            Direction.West,
            Direction.NorthWest

        };
        public King(Player color)
        {
            Color = color;
        }
        private static bool IsUnMovedRook(Position pos, Board board)
        {
            if(board.IsEmpty(pos))
            {
                return false;
            }
            Piece piece = board[pos];
            return piece.Type == PieceType.Rook && !piece.HasMoved;
        }

        private static bool AllEmpty(IEnumerable<Position> positions, Board board)// kiểm tra các ô trống
        {
           return positions.All(pos => board.IsEmpty(pos));
        }
        public override Piece Copy()// sao chép quân cờ de check nhap thanh
        {
            King copy = new King(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }
        private bool CanCastleKingSide(Position from, Board board)
        {
            if (HasMoved)
            {
                return false;
            }
            Position rookPos = new Position(from.Row,7);
            Position[] betweenPositions = new Position[]
            {
                new (from.Row, 5),
                new (from.Row, 6)
            };

            return IsUnMovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
        }

        private bool CanCastleQueenSide(Position from, Board board)
        {
            if (HasMoved)
            {
                return false;
            }
            Position rookPos = new Position(from.Row, 0);
            Position[] betweenPositions = new Position[]
            {
                new (from.Row, 1),
                new (from.Row, 2),
                new (from.Row, 3)
            };
            return IsUnMovedRook(rookPos, board) && AllEmpty(betweenPositions, board);
        }
        private IEnumerable<Position> MovePositions(Position from, Board board)// ds các ô có thể đi đến
        {
            foreach (Direction dir in dirs)
            {

                Position to = from + dir;
                if (!Board.IsInside(to))
                {
                    continue;
                }
                if (board.IsEmpty(to) || board[to].Color != Color)
                {
                    yield return to;
                }


            }
        }
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            foreach (Position to in MovePositions(from, board))
            {
                yield return new NormalMove(from, to);
            }
            if (CanCastleKingSide(from, board))
            {
                yield return new Castle(MoveType.CastleKS, from);
            }
            if(CanCastleQueenSide(from, board))
            {
                yield return new Castle(MoveType.CastleQS, from);
            }
        }
        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            return GetMoves(from, board).Any(move =>    // xem chieu vua doi thu khong
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.King;
            });
        }
        
    }
}
