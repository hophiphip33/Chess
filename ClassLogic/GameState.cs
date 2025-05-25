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

        private int noCaptureOrPawnMove = 0; // số nước đi không bắt quân hoặc không di chuyển tốt
        private string stateString;
        private readonly Dictionary<string, int> stateHistory = new Dictionary<string, int>(); // lưu lịch sử các nước đi đã thực hiện
        public GameState( Player player, Board board)
        {
            CurrentPlayer = player;
            Board = board;

            stateString = new StateString(CurrentPlayer, Board).ToString();
            stateHistory[stateString] = 1; // thêm nước đi đầu tiên vào lịch sử
        }
        public IEnumerable<Move> LegalMovesForPiece( Position pos) // danh sách nước đi hợp lệ cho quân cờ
        {
            if (Board.IsEmpty(pos) || Board[pos].Color!= CurrentPlayer)
            {
                return Enumerable.Empty<Move>();
            }
            Piece piece = Board[pos];
            IEnumerable<Move> moveCandidates = piece.GetMoves(pos, Board);
            return moveCandidates.Where(move => move.IsLegal(Board));// loc nuoc di
        }
        public void MakeMove(Move move) //Thực hiện nước đi
        { 
            Board.SetPawnSkipPosition(CurrentPlayer, null);//Xóa En Passant vì nước đi mới bắt đầu
            bool captureOrPawn =move.Execute(Board);

            if (captureOrPawn)// bat quan hoac di chuyen tot
            {
                noCaptureOrPawnMove = 0;
                stateHistory.Clear();
            }
            else
            {
                noCaptureOrPawnMove++;
            }
            CurrentPlayer = CurrentPlayer.Opponent();
            UpdateStateString();
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
            else if (Board.InsufficientMaterial())
            {
                result = Result.Draw(EndReason.InsufficientMaterial);
            }
            else if (FiftyMoveRule())
            {
                result = Result.Draw(EndReason.FiftyMoveRule);
            }
            else if (ThreefoldRepetition())
            {
                result = Result.Draw(EndReason.ThreefoldRepetition);
            }


        }
        public bool IsGameOver()
        {
            return result != null;
        }
        private bool FiftyMoveRule()
        {
            int fullMoves = noCaptureOrPawnMove / 2;
            return fullMoves == 50;// nếu không có nước đi nào bắt quân hoặc di chuyển tốt trong 50 nước đi thì hòa
        }
        private void UpdateStateString()
        {
            stateString = new StateString(CurrentPlayer, Board).ToString();
            if (!stateHistory.ContainsKey(stateString))
            {
                stateHistory[stateString] = 1;
            }
            else
            {
                stateHistory[stateString]++;
            }
        }
        private bool ThreefoldRepetition()
        {
            return stateHistory[stateString]==3;
        }
    }
}

