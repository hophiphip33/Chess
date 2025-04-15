using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// <summary>
    /// Interaction logic for GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public event EventHandler<Option> OptionSelected;
        public GameOverMenu(GameState gameState)
        {
            InitializeComponent();
            Result result = gameState.result;
            WinnerText.Text = GetWinnerText(result.Winner);
            ReasonText.Text = GetEndReasonText(result.EndReason, gameState.CurrentPlayer);
        }

        private static string GetWinnerText(Player winner)
        {
            return winner switch
            {
                Player.White => "Trắng thắng!",
                Player.Black => "Đen thắng!",
                Player.None => "Hòa!"
            };
            
        }
        private static string PlayerString(Player player)
        {
            return player switch
            {
                Player.White => "Trắng",
                Player.Black => "Đen",
                Player.None => ""
            };
        }
        private static string GetEndReasonText(EndReason reason,Player currentPlayer)
        {
            return reason switch
            {
                EndReason.Stalemate => $"Hòa - {PlayerString(currentPlayer) } không thể di chuyển",
                EndReason.Checkmate => $"Chiếu hết -{PlayerString(currentPlayer)} không thể di chuyển",
                EndReason.FiftyMoveRule => " Hòa - Luật 50 bước",
                EndReason.InsufficientMaterial => " Hòa - Không đủ quân",
                EndReason.ThreefoldRepetition => "Hòa - Lặp lại 3 bước",
                _ => ""
            };
        }
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(this, Option.Restart);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(this, Option.Exit);
        }
    }
}
