using System;
using System.Text;


namespace ClassLogic
{
    public class StateString
    {
        private readonly StringBuilder sb = new StringBuilder();

        public StateString( Player currentPlayer, Board board)
        {
            // them piece placement data
            AddPiecePlacementData(board);
            sb.Append(' ');
            // them current player
            AddCurrentPlayerData(currentPlayer);
            sb.Append(' ');
            // them quyen nhap thanh
            AddCastlingRights(board);
            sb.Append(' ');
            // them en passant data
            AddEnPassantData(board, currentPlayer);

        }
        public override string ToString()
        {
            return sb.ToString();
        }
        private static char PieceChar(Piece piece)
        {
            char c = piece.Type switch
            {
                PieceType.King => 'k',
                PieceType.Queen => 'q',
                PieceType.Rook => 'r',
                PieceType.Bishop => 'b',
                PieceType.Knight => 'n',
                PieceType.Pawn => 'p',
                _ => ' '
            };
            if (piece.Color == Player.White)// trang viet hoa
            {
                c = char.ToUpper(c);
            }
            return c;
        }
            private void AddRowData(Board board,int row)
            {
            int empty = 0;
            for (int c = 0; c < 8; c++)
            {
               if (board[row, c] == null)
                {
                    empty++;
                    continue;
                }
              
                
                if (empty > 0)
                {
                        sb.Append(empty);
                        empty = 0;
                 }
                sb.Append(PieceChar(board[row, c]));
                
            }
            if (empty > 0)
            {
                sb.Append(empty);
            }
        }
        private void AddPiecePlacementData(Board board)
        {
            for (int r = 0; r < 8; r++)
            {
                
                if (r > 0)
                {
                    sb.Append("/");
                }
                AddRowData(board, r);
            }
        }
        private void AddCurrentPlayerData(Player currentPlayer)
        {
            sb.Append(currentPlayer == Player.White ? "w" : "b");
        }
        private void AddCastlingRights(Board board)
        {
            bool castleWKS= board.CastleRightKS(Player.White);
            bool castleWQS = board.CastleLeftQS(Player.White);
            bool castleBKS = board.CastleRightKS(Player.Black);
            bool castleBQS = board.CastleLeftQS(Player.Black);

            if(!(castleWKS || castleWQS || castleBKS || castleBQS))
            {
                sb.Append('-');
                return;
            }
            if (castleWKS)
            {
                sb.Append('K');
            }
            if (castleWQS)
            {
                sb.Append('Q');
            }
            if (castleBKS)
            {
                sb.Append('k');
            }
            if (castleBQS)
            {
                sb.Append('q');
            }
        }
        private void AddEnPassantData(Board board,Player currentPlayer)
        {
            if (!board.CanCaptureEnPassant(currentPlayer))
            {
                sb.Append('-');
                return;
            }
            Position pos = board.GetPawnSkipPosition(currentPlayer.Opponent());
            char file = (char)('a' + pos.Column);
            int rank = 8 - pos.Row;
            sb.Append(file);
            sb.Append(rank);
        }
    }
}
