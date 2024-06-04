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
using System.Windows.Shapes;
using static System.Formats.Asn1.AsnWriter;

namespace Wpf_2048
{
    /// <summary>
    /// Interaction logic for GameOverWindow.xaml
    /// </summary>
    public partial class GameOverWindow : Window    
    {

        private MainWindow _mainWindow;
        public GameOverWindow(MainWindow mainWindow, int score)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            TextBlock ScoreBlock = FindName("ScoreTextBlock") as TextBlock;
            ScoreBlock.Text = score.ToString();
        }
        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            //restartuje hru
            _mainWindow.RestartGame();
            Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
