using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLogic
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;
        public override Player Color { get; }
        private readonly Direction forward;
        public Pawn(Player color)
        {
            Color = color;
            if (color == Player.White)
            {
                forward = Direction.North;
            }
            else if (color == Player.Black)
            {
                forward = Direction.South;
            }
        }
        public override Piece Copy()
        {
            Pawn copy = new Pawn(Color);
            copy.HasMoved = HasMoved;
            return copy;
        }
        private static bool CanMoveTo(Position pos, Board board)
        {
            return Board.IsInside(pos) && board.IsEmpty(pos);
        }
        private bool CanCaptureAt(Position pos, Board board)// kiểm tra ăn đc k
        {
            if (!Board.IsInside(pos) || board.IsEmpty(pos))
            {
                return false;
            }
            return board[pos].Color != Color;
        }

        private IEnumerable<Move> PromotionMoves(Position from, Position to)
        {
            yield return new PawnPromotion(from, to, PieceType.Queen);
            yield return new PawnPromotion(from, to, PieceType.Rook);
            yield return new PawnPromotion(from, to, PieceType.Bishop);
            yield return new PawnPromotion(from, to, PieceType.Knight);
        }
        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            Position oneMovePos = from + forward;
            if (CanMoveTo(oneMovePos, board))
            {
                if (oneMovePos.Row ==0|| oneMovePos.Row == 7)
                {
                    foreach (Move proMove in PromotionMoves(from, oneMovePos))
                    {
                        yield return proMove;
                    }
                }
                else
                {
                    yield return new NormalMove(from, oneMovePos);
                }


               
                Position twoMovePos = oneMovePos + forward;
                if (!HasMoved && CanMoveTo(twoMovePos, board))
                {
                    yield return new DoublePawn(from, twoMovePos);
                }
            }
        }
        private IEnumerable<Move> DiagonalMoves(Position from, Board board)// ăn chéo
        {
            Direction[] diagonalDirs = Color == Player.White
                ? new Direction[] { Direction.NorthEast, Direction.NorthWest }
                : new Direction[] { Direction.SouthEast, Direction.SouthWest };

            foreach (Direction dir in diagonalDirs)
            {
                Position to = from + dir;

                if(to ==board.GetPawnSkipPosition(Color.Opponent()))// kiểm tra ăn tốt địch
                {
                    yield return new EnPassant(from, to);
                }
                

                else if (CanCaptureAt(to, board))// check xem quân đối phương ở vị trí ăn được k
                {
                    if (to.Row == 0 || to.Row == 7)
                    {
                        foreach (Move proMove in PromotionMoves(from, to))
                        {
                            yield return proMove;
                        }
                    }
                    else
                    {
                        yield return new NormalMove(from, to);
                    }
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return ForwardMoves(from, board).Concat(DiagonalMoves(from, board));
        }
        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            return DiagonalMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.King ;
            });
        }
    }
}
