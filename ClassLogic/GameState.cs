using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLogic
{
    public class GameState
    {
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }
        public GameState( Player player, Board board)
        {
            CurrentPlayer = player;
            Board = board;
        }
        public IEnumerable<Move> LegalMovesForPiece( Position pos) // danh sách nước đi hợp lệ cho quân cờ
        {
            if (Board.IsEmpty(pos) || Board[pos].Color!= CurrentPlayer)
            {
                return Enumerable.Empty<Move>();
            }
            Piece piece = Board[pos];
            IEnumerable<Move> moveCandidates = piece.GetMoves(pos, Board);
            return moveCandidates.Where(move => move.IsLegal(Board));
        }
        public void MakeMove(Move move) 
        { 
            move.Execute(Board);
            CurrentPlayer = CurrentPlayer.Opponent();
        }
    }
}
