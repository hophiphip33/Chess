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
        public Result result { get; private set; } = null;
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
        public void MakeMove(Move move) //Thực hiện nước đi
        { 
            move.Execute(Board);
            CurrentPlayer = CurrentPlayer.Opponent();
            CheckForGameOver();
        }
        public IEnumerable<Move> AllLegalMovesFor(Player player) // danh sách nước đi hợp lệ cho player
        {
            IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(player)
                .SelectMany(pos => {
                    Piece piece = Board[pos];
                    return piece.GetMoves(pos, Board);
                });
            return moveCandidates.Where(move => move.IsLegal(Board));
        }
        private void CheckForGameOver() { 
        if (!AllLegalMovesFor(CurrentPlayer).Any())
            {
                if (Board.IsInCheck(CurrentPlayer))
                {
                    result = Result.Win(CurrentPlayer.Opponent());
                }
                else
                {
                    result = Result.Draw(EndReason.Stalemate);
                }
            }
        }
        public bool IsGameOver()
        {
            return result != null;
        }
    }
}

