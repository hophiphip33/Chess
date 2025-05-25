using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ClassLogic
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];
        private readonly Dictionary<Player, Position> pawnSkipPositions = new Dictionary<Player, Position>
        {//lưu vị trí mà tốt vừa đi qua 2 ô, giúp kiểm tra xem có được phép bắt qua đường hay không.
            {Player.White,null },
            {Player.Black,null }
        };
        public Piece this[int row, int column]
        {
            get { return pieces[row, column]; }
            set { pieces[row, column] = value; }
        }
        public Piece this[Position position]
        {
            get { return pieces[position.Row, position.Column]; }
            set { pieces[position.Row, position.Column] = value; }
        }

        public Position GetPawnSkipPosition(Player player)//ô mà tốt của player vừa nhảy qua 2 ô
        {
            return pawnSkipPositions[player];
        }
        public void SetPawnSkipPosition(Player player, Position position)//Cập nhật ô skip khi tốt vừa đi 2 bước.
        {
            pawnSkipPositions[player] = position;
        }
        public static Board Initial() {
            Board board = new Board();
            board.AddStartPieces();
            return board;

        }
        private void AddStartPieces()
        {
            for (int i = 0; i < 8; i++)
            {
                pieces[6, i] = new Pawn(Player.White);
                pieces[1, i] = new Pawn(Player.Black);
            }
            pieces[0, 0] = new Rook(Player.Black);
            pieces[0, 1] = new Knight(Player.Black);
            pieces[0, 2] = new Bishop(Player.Black);
            pieces[0, 3] = new Queen(Player.Black);
            pieces[0, 4] = new King(Player.Black);
            pieces[0, 5] = new Bishop(Player.Black);
            pieces[0, 6] = new Knight(Player.Black);
            pieces[0, 7] = new Rook(Player.Black);

            pieces[7, 0] = new Rook(Player.White);
            pieces[7, 1] = new Knight(Player.White);
            pieces[7, 2] = new Bishop(Player.White);
            pieces[7, 3] = new Queen(Player.White);
            pieces[7, 4] = new King(Player.White);
            pieces[7, 5] = new Bishop(Player.White);
            pieces[7, 6] = new Knight(Player.White);
            pieces[7, 7] = new Rook(Player.White);

        }
        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;

        }
        public  bool IsEmpty(Position pos)
        {
            return this[pos] == null ;
        }
        public IEnumerable<Position> PiecePositions()//ds vi tri cac quan tren ban
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    Position pos = new Position(row, column);
                    if (!IsEmpty(pos))
                    {
                        yield return pos;
                    }
                   
                }
            }
        }
        public IEnumerable<Position> PiecePositionsFor(Player player)// loc ds vi tri quan theo player
        {
            return PiecePositions()
                .Where(pos => this[pos].Color == player);
        }
        public bool IsInCheck(Player player)
        {
            return PiecePositionsFor(player.Opponent()) // doi thu dang chieu
                .Any(pos =>
                {
                    Piece piece = this[pos];
                    return piece.CanCaptureOpponentKing(pos, this);
                });
        }
        public Board Copy()// tao ban sao co  de kiem tra  gia dinh k thay doi ban goc
        {
            Board copy = new Board();
            foreach (Position pos in PiecePositions())
            {
                copy[pos] = this[pos].Copy();
            }
            return copy;
        }
        public Counting CountPieces()
        {
            Counting counting = new Counting();
            foreach (Position pos in PiecePositions())
            {
                Piece piece = this[pos];
                counting.Increment(piece.Color, piece.Type);
            }
            return counting;
        }
        public bool InsufficientMaterial() {// hoa k du quan

            Counting counting = CountPieces();
            return IsKingVKing(counting) ||
                   IsKingBishopVKing(counting) ||
                   IsKingKnightVKing(counting) ||
                   IsKingBishopVKingBishop(counting);

        }
        private static bool IsKingVKing(Counting counting)
        {
            return counting.TotalCount == 2;
        }
        private static bool IsKingBishopVKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Bishop) == 1 || counting.Black(PieceType.Bishop) == 1);
        }
        private static bool IsKingKnightVKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Knight) == 1 || counting.Black(PieceType.Knight) == 1);
        }
        private  bool IsKingBishopVKingBishop(Counting counting)
        {
            if (counting.TotalCount != 4)
            {
                return false;
            }
            if(counting.White(PieceType.Bishop) != 1 || counting.Black(PieceType.Bishop) != 1)
            {
                return false;
            }
            //------------------------------------------
            
            Position wBishopPos = FindPiece(Player.White, PieceType.Bishop); // Use the instance to call FindPiece
            Position bBishopPos = FindPiece(Player.Black, PieceType.Bishop);
            return wBishopPos.SquareColor() == bBishopPos.SquareColor();

        }
        private Position FindPiece(Player color, PieceType type)
        {
            return PiecePositionsFor(color)
                .First(pos => this[pos].Type == type);
        }

        private bool IsUnMoveKingandRook(Position kingpos, Position rookpos)// kiểm tra vua và xe chưa di chuyển
        {
            if (IsEmpty(kingpos)|| IsEmpty(rookpos))
            {
                return false;
            }
            Piece king = this[kingpos];
            Piece rook = this[rookpos];
            return king.Type == PieceType.King && rook.Type == PieceType.Rook && !king.HasMoved && !rook.HasMoved;
        }
        public bool CastleRightKS (Player player)
        {
            return player switch
            {
                Player.White => IsUnMoveKingandRook(new Position(7, 4), new Position(7, 7)),
                Player.Black => IsUnMoveKingandRook(new Position(0, 4), new Position(0, 7)),
                _ => false
            };
        }
        public bool CastleLeftQS(Player player)
        {
            return player switch
            {
                Player.White => IsUnMoveKingandRook(new Position(7, 4), new Position(7, 0)),
                Player.Black => IsUnMoveKingandRook(new Position(0, 4), new Position(0, 0)),
                _ => false
            };
        }

        private bool HasPawnInPosition(Player player, Position[] pawnPositions, Position skipPos)//Kiểm tra có quân tốt của player đứng tại các vị trí bắt qua đường không
        {
            foreach (Position pos in pawnPositions.Where(IsInside)) {

                Piece piece = this[pos];
                if (piece == null || piece.Color != player || piece.Type != PieceType.Pawn)
                {
                    continue;//ô đó không có quân, không phải quân của bạn, hoặc không phải là tốt => bỏ qua
                }
                EnPassant move = new EnPassant(pos, skipPos);//Tạo một nước đi "giả định" En Passant từ vị trí hiện tại đến skipPos
                if (move.IsLegal(this))
                {
                    return true;
                }
            }
            return false;
        }


        public bool CanCaptureEnPassant(Player player)
        {
            Position skipPos = GetPawnSkipPosition(player.Opponent());
            if (skipPos == null)
            {
                return false;
            }
            Position[] pawnPositions = player switch
            {
                Player.White => new Position[] { skipPos + Direction.SouthWest, skipPos + Direction.SouthEast },
                Player.Black => new Position[] { skipPos + Direction.NorthWest, skipPos + Direction.NorthEast },
                _ => Array.Empty<Position>()

            };
            return HasPawnInPosition(player, pawnPositions, skipPos);
        }
    }
}
