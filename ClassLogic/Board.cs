using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLogic
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];
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
    }
}
