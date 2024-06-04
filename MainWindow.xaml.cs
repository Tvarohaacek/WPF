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
using System.Xml;
using System.Linq;

namespace Wpf_2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameOverWindow _gameOverWindow;
        public int[,] Tiles = new int[4, 4];
        public Random Rnd = new Random();
        public int Score = 0;


        public MainWindow()
        {
            InitializeComponent();
            SetupTiles();
            VisualizeLabels(); 

        }


        public void SetupTiles()
        {
            int fRand = Rnd.Next(4);
            int sRand = Rnd.Next(4);

            Tiles[fRand, sRand] = GetTileNumber();

            fRand = Rnd.Next(4);
            sRand = Rnd.Next(4);

            if (Tiles[fRand, sRand] == 2)
            {
                if (sRand == 3)
                {
                    sRand--;
                }
                else
                {
                    sRand++;
                }
            }

            Tiles[fRand, sRand] = GetTileNumber();
        }

        public void VisualizeLabels()
        {


            for (int i = 0; i < 16; i++)
            {
                string labelName = "Label" + (i + 1);
                Label label = FindName(labelName) as Label;
                if (label != null)
                {
                    int row = i / 4;
                    int col = i % 4;
                    label.Content = Tiles[row, col].ToString();
                    if (label.Content.ToString() == "0")
                    {
                        label.Content = "";
                        label.Background = Brushes.White;
                    }
                    else
                    {
                        SolidColorBrush brush = new SolidColorBrush();
                        SolidColorBrush brushF = new SolidColorBrush();
                        int number;

                        if (int.TryParse(label.Content.ToString(), out number))
                        {
                            if (number > 31)
                            {
                                brushF.Color = Colors.White;
                            }
                            else
                            {
                                brushF.Color = Colors.Black;
                            }

                            if (number > 99)
                            {
                                label.FontSize = 30;
                            }
                            else
                            {
                                label.FontSize = 40;
                            }
                        }

                        label.Foreground = brushF;

                        switch (label.Content)
                        {
                            case "2":
                                brush.Color = Colors.Red;
                                break;
                            case "4":
                                brush.Color = Colors.Orange;
                                break;
                            case "8":
                                brush.Color = Colors.Yellow;
                                break;
                            case "16":
                                brush.Color = Colors.LimeGreen;
                                break;
                            case "32":
                                brush.Color = Colors.DarkGreen;
                                break;
                            case "64":
                                brush.Color = Colors.DarkCyan;
                                break;
                            case "128":
                                brush.Color = Colors.Cyan;
                                break;
                            case "256":
                                brush.Color = Colors.Blue;
                                break;
                            case "512":
                                brush.Color = Colors.DarkBlue;
                                break;
                            case "1024":
                                brush.Color = Colors.Purple;
                                break;
                            default:
                                brush.Color = Colors.Black;
                                break;
                        }
                        label.Background = brush;

                    }
                }
            }

            TextBlock ScoreBlock = FindName("ScoreBlock") as TextBlock;
            ScoreBlock.Text = Score.ToString();
        }



        public void CreateNewTile(int[,] board)
        {

            var zeroSpots = new List<(int, int)>();
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] == 0)
                        zeroSpots.Add((x, y));
                }
            }

            if (zeroSpots.Count == 0)
                return;
            var randomIndex = Rnd.Next(zeroSpots.Count);
            var randomSpot = zeroSpots[randomIndex];
            board[randomSpot.Item1, randomSpot.Item2] = GetTileNumber();
            Tiles = board;
        }

        public int GetTileNumber()
        {
            int n = Rnd.Next(9);
            if (n == 8)
                return 4;
            else
                return 2;
        }



        public void CheckEnd()
        {
            if (ArraysAreEqual(TestMoveDown((int[,])Tiles.Clone()), Tiles) && ArraysAreEqual(TestMoveUp((int[,])Tiles.Clone()), Tiles) &&
                ArraysAreEqual(TestMoveLeft((int[,])Tiles.Clone()), Tiles) && ArraysAreEqual(TestMoveRight((int[,])Tiles.Clone()), Tiles))
            {
                End();
            }
            else
            {
               
            }

        }

        public static bool ArraysAreEqual(int[,] array1, int[,] array2)
        {
            if (array1.GetLength(0) != array2.GetLength(0) || array1.GetLength(1) != array2.GetLength(1))
                return false;
            for (int i = 0; i < array1.GetLength(0); i++)
            {
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    if (array1[i, j] != array2[i, j])
                        return false;
                }
            }

            
            return true;
        }

        public void End()
        {
            //var gameOverWindow = new GameOverWindow();
            //gameOverWindow.ShowDialog();
            _gameOverWindow = new GameOverWindow(this, Score);
            _gameOverWindow.ShowDialog();
        }
         

        public void RestartGame()
        {
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();

            // Close all other windows except the new main window
            foreach (Window window in Application.Current.Windows)
            {
                if (window != Application.Current.MainWindow)
                {
                    window.Close();
                }
            }

            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    Tiles[i, j] = 0;
                }
            }

            SetupTiles();
            VisualizeLabels();
            Score = 0;
            Rnd = null;
            Rnd = new Random();
        }



        public void MoveRight(int[,] board)
        {
            int size = board.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                int previous = 0;
                int mergeIndex = size - 1;

                for (int j = size - 1; j >= 0; j--)
                {
                    if (board[i, j] != 0)
                    {
                        if (board[i, j] == previous)
                        {
                            board[i, mergeIndex + 1] *= 2; // Double the number at the mergeIndex+1
                            Score += board[i, mergeIndex + 1];
                            board[i, j] = 0; // Clear the current position
                            previous = 0; // Reset the previous number
                        }
                        else
                        {
                            previous = board[i, j];
                            if (mergeIndex != j)
                            {
                                board[i, mergeIndex] = board[i, j];
                                board[i, j] = 0;
                            }
                        }
                        mergeIndex--;
                    }
                }
            }
            Tiles = board;
            VisualizeLabels();
        }


        public void MoveLeft(int[,] board)
        {
            int size = board.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                int previous = 0;
                int mergeIndex = 0;

                for (int j = 0; j < size; j++)
                {
                    if (board[i, j] != 0)
                    {
                        if (board[i, j] == previous)
                        {
                            board[i, mergeIndex - 1] *= 2;
                            Score += board[i, mergeIndex - 1];// Double the number at the mergeIndex-1
                            board[i, j] = 0; // Clear the current position
                            previous = 0; // Reset the previous number
                        }
                        else
                        {
                            previous = board[i, j];
                            if (mergeIndex != j)
                            {
                                board[i, mergeIndex] = board[i, j];
                                board[i, j] = 0;
                            }
                        }
                        mergeIndex++;
                    }
                }
            }

            
            Tiles = board;
            VisualizeLabels();


        }


        public void MoveUp(int[,] board)
        {
            int size = board.GetLength(0);

            for (int j = 0; j < size; j++)
            {
                int previous = 0;
                int mergeIndex = 0;

                for (int i = 0; i < size; i++)
                {
                    if (board[i, j] != 0)
                    {
                        if (board[i, j] == previous)
                        {
                            board[mergeIndex - 1, j] *= 2; // Double the number at the mergeIndex-1
                            Score += board[mergeIndex - 1, j];
                            board[i, j] = 0; // Clear the current position
                            previous = 0; // Reset the previous number
                        }
                        else
                        {
                            previous = board[i, j];
                            if (mergeIndex != i)
                            {
                                board[mergeIndex, j] = board[i, j];
                                board[i, j] = 0;
                            }
                        }
                        mergeIndex++;
                    }
                }
            }
            Tiles = board;

            VisualizeLabels();
        }

        public void MoveDown(int[,] board)
        {
            int size = board.GetLength(0);

            for (int j = 0; j < size; j++)
            {
                int previous = 0;
                int mergeIndex = size - 1;

                for (int i = size - 1; i >= 0; i--)
                {
                    if (board[i, j] != 0)
                    {
                        if (board[i, j] == previous)
                        {
                            board[mergeIndex + 1, j] *= 2; // Double the number at the mergeIndex+1
                            Score += board[mergeIndex + 1, j];
                            board[i, j] = 0; // Clear the current position
                            previous = 0; // Reset the previous number
                        }
                        else
                        {
                            previous = board[i, j];
                            if (mergeIndex != i)
                            {
                                board[mergeIndex, j] = board[i, j];
                                board[i, j] = 0;
                            }
                        }
                        mergeIndex--;
                    }
                }
            }
            Tiles = board;
            VisualizeLabels();
        }


        private void A_click(object sender, RoutedEventArgs e)
        {
            if (ArraysAreEqual(TestMoveLeft((int[,])Tiles.Clone()), Tiles))
            {

            }
            else
            {
                MoveLeft(Tiles);

                CreateNewTile(Tiles);
                VisualizeLabels();
                CheckEnd();
            }
        }

        private void D_click(object sender, RoutedEventArgs e)
        {
            if (ArraysAreEqual(TestMoveRight((int[,])Tiles.Clone()), Tiles))
            {

            }
            else
            {
                MoveRight(Tiles);

                CreateNewTile(Tiles);
                VisualizeLabels();
                CheckEnd();
            }
        }

        private void W_click(object sender, RoutedEventArgs e)
        {
            if (ArraysAreEqual(TestMoveUp((int[,])Tiles.Clone()), Tiles))
            {

            }
            else
            {
                MoveUp(Tiles);

                CreateNewTile(Tiles);
                VisualizeLabels();
                CheckEnd();
            }
        }

        private void S_click(object sender, RoutedEventArgs e)
        {
            if (ArraysAreEqual(TestMoveDown((int[,])Tiles.Clone()), Tiles))
            {

            }
            else
            {
                MoveDown(Tiles);

                CreateNewTile(Tiles);
                VisualizeLabels();
                CheckEnd();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.Left)
            {
                if (ArraysAreEqual(TestMoveLeft((int[,])Tiles.Clone()), Tiles))
                {

                }
                else
                {
                    MoveLeft(Tiles);

                    CreateNewTile(Tiles);
                    VisualizeLabels();
                    CheckEnd();
                }
                return;
            }
            else if (e.Key == Key.D || e.Key == Key.Right)
            {
                if (ArraysAreEqual(TestMoveRight((int[,])Tiles.Clone()), Tiles))
                {

                }
                else
                {
                    MoveRight(Tiles);

                    CreateNewTile(Tiles);
                    VisualizeLabels();
                    CheckEnd();
                }
                return;
            }
            else if (e.Key == Key.W || e.Key == Key.Up)
            {
                if (ArraysAreEqual(TestMoveUp((int[,])Tiles.Clone()), Tiles))
                {

                }
                else
                {
                    MoveUp(Tiles);

                    CreateNewTile(Tiles);
                    VisualizeLabels();
                    CheckEnd();
                }
                return;
            }
            else if (e.Key == Key.S || e.Key == Key.Down)
            {
                if (ArraysAreEqual(TestMoveDown((int[,])Tiles.Clone()), Tiles))
                {

                }
                else
                {
                    MoveDown(Tiles);

                    CreateNewTile(Tiles);
                    VisualizeLabels();
                    CheckEnd();
                }
                return;
            }
            else
            {
                return;
            }


        }



        public int[,] TestMoveLeft(int[,] board)
        {
            int size = board.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                int previous = 0;
                int mergeIndex = 0;

                for (int j = 0; j < size; j++)
                {
                    if (board[i, j] != 0)
                    {
                        if (board[i, j] == previous)
                        {
                            board[i, mergeIndex - 1] *= 2; // Double the number at the mergeIndex-1
                            board[i, j] = 0; // Clear the current position
                            previous = 0; // Reset the previous number
                        }
                        else
                        {
                            previous = board[i, j];
                            if (mergeIndex != j)
                            {
                                board[i, mergeIndex] = board[i, j];
                                board[i, j] = 0;
                            }
                        }
                        mergeIndex++;
                    }
                }
                
            }
            return board;
        }

        public int[,] TestMoveDown(int[,] board)
        {
            int size = board.GetLength(0);

            for (int j = 0; j < size; j++)
            {
                int previous = 0;
                int mergeIndex = size - 1;

                for (int i = size - 1; i >= 0; i--)
                {
                    if (board[i, j] != 0)
                    {
                        if (board[i, j] == previous)
                        {
                            board[mergeIndex + 1, j] *= 2; // Double the number at the mergeIndex+1
                            board[i, j] = 0; // Clear the current position
                            previous = 0; // Reset the previous number
                        }
                        else
                        {
                            previous = board[i, j];
                            if (mergeIndex != i)
                            {
                                board[mergeIndex, j] = board[i, j];
                                board[i, j] = 0;
                            }
                        }
                        mergeIndex--;
                    }
                }
            }
            return board;
        }

        public int[,] TestMoveUp(int[,] board)
        {
            int size = board.GetLength(0);

            for (int j = 0; j < size; j++)
            {
                int previous = 0;
                int mergeIndex = 0;

                for (int i = 0; i < size; i++)
                {
                    if (board[i, j] != 0)
                    {
                        if (board[i, j] == previous)
                        {
                            board[mergeIndex - 1, j] *= 2; // Double the number at the mergeIndex-1
                            board[i, j] = 0; // Clear the current position
                            previous = 0; // Reset the previous number
                        }
                        else
                        {
                            previous = board[i, j];
                            if (mergeIndex != i)
                            {
                                board[mergeIndex, j] = board[i, j];
                                board[i, j] = 0;
                            }
                        }
                        mergeIndex++;
                    }
                }
            }
            return board;
        }

        public int[,] TestMoveRight(int[,] board)
        {
            int size = board.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                int previous = 0;
                int mergeIndex = size - 1;

                for (int j = size - 1; j >= 0; j--)
                {
                    if (board[i, j] != 0)
                    {
                        if (board[i, j] == previous)
                        {
                            board[i, mergeIndex + 1] *= 2; // Double the number at the mergeIndex+1
                            board[i, j] = 0; // Clear the current position
                            previous = 0; // Reset the previous number
                        }
                        else
                        {
                            previous = board[i, j];
                            if (mergeIndex != j)
                            {
                                board[i, mergeIndex] = board[i, j];
                                board[i, j] = 0;
                            }
                        }
                        mergeIndex--;
                    }
                }
            }
            return board;
        }

    }
}