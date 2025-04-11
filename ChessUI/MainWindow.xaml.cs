using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLogic;

namespace ChessUI
{
   
    public partial class MainWindow : Window
    {
        private readonly Image[,] piecesImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();
        private GameState gameState;
        private Position selectedPos = null;
        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();
            gameState = new GameState(Player.White, Board.Initial());
            DrawBoard(gameState.Board);
            SetCursor(gameState.CurrentPlayer);
        }
        private void InitializeBoard() { 
        for (int i=0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Image image = new Image();
                    piecesImages[i, j] = image;
                    PieceGrid.Children.Add(image);// Thêm ảnh quân cờ vào lưới hiển thị.


                    Rectangle highlight = new Rectangle();
                    highlights[i,j] = highlight;
                    HighlightGrid.Children.Add(highlight);//TThêm lớp highlight trong lưới phủ

                }
            }
        }
        private void DrawBoard(Board board) {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece piece = board[i, j];
                    piecesImages[i, j].Source = Images.GetImage(piece);//Gán hình ảnh tương ứng

                }
            }
        }
        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(BoardGrid);
            Position pos = ToSquarePosition(point);
            if(selectedPos == null)
            {
               OnFromPositionSelected(pos);//Lần đầu chọn quân cờ.
            }
            else
            {
                OnToPositionSelected(pos);//Lần thứ hai, chọn vị trí cần đi đến.


            }
        }

        private void OnFromPositionSelected(Position pos)
        {
            IEnumerable<Move> moves = gameState.LegalMovesForPiece(pos);
            if (moves.Any())
            {
                selectedPos = pos;
                CacheMoves(moves);
                ShowHighLights();
            }
        }
        private void OnToPositionSelected(Position pos)
        {
            
                selectedPos = null;
                HideHighLights();
                if (moveCache.TryGetValue(pos, out Move move))// kt xem ô có nằm trong ds nước đi k
                {
                    HandleMove(move);
                }
            
        }

        private void HandleMove(Move move)
        {
            gameState.MakeMove(move);// Cập nhật trạng thái game
            DrawBoard(gameState.Board);// Vẽ lại bàn cờ
            HideHighLights();
            SetCursor(gameState.CurrentPlayer);// Đặt lại con trỏ chuột
        }
        private Position ToSquarePosition(Point point)//Chuyển tọa độ chuột thành vị trí ô
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = (int)(point.Y / squareSize);
            int column = (int)(point.X / squareSize);
            return new Position(row, column);
        }
        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();
            foreach (Move move in moves)
            {
                moveCache[move.ToPos] = move; // Gán mỗi nước đi theo vị trí đích
            }
        }
        private void ShowHighLights()
        {
            Color color = Color.FromArgb(150,130,255,130);
            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = new SolidColorBrush(color);
            }
        }
        private void HideHighLights()
        {
            foreach (Position to in moveCache.Keys)
            {
                highlights[to.Row, to.Column].Fill = Brushes.Transparent;
            }
        }
        private void SetCursor(Player player)
        {
            if (player == Player.White)
            {
                Cursor = ChessCursors.WhiteCursor;
            }
            else
            {
                Cursor = ChessCursors.BlackCursor;
            }
        }
    }
}