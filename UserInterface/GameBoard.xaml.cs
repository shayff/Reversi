using Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Window
    {
        private int m_SumOfWhitePoints = 2;
        private int m_SumOfBlackPoints = 2;
        private readonly Button[,] r_BoardMatrix;
        private readonly GameManager r_Game;
        private readonly Dictionary<eCoinColor, string> r_eCoinToText = new Dictionary<eCoinColor, string>()
        {
            {eCoinColor.Black, "BlackButton" },
            {eCoinColor.White, "WhiteButton" },
            {eCoinColor.Empty, "BoardButton" }
        };

        public GameBoard(int i_BoardSize, GameManager.eGameType i_GameType)
        {
            InitializeComponent();
            this.Closing += GameBoard_Closing;
            createRowsAndCols(i_BoardSize);
            r_BoardMatrix = new Button[i_BoardSize, i_BoardSize];
            r_Game = new GameManager(i_BoardSize, i_GameType, "Player1", "Player2");

            //Creating a board buttons
            for (int y = 0; y < i_BoardSize; y++)
            {
                for (int x = 0; x < i_BoardSize; x++)
                {
                    createButton(x, y);
                }
            }
            PlayTurns();
        }

        private void createButton(int i_X, int i_Y)
        {
            Button button = new Button();
            button.Tag = new System.Drawing.Point(i_X, i_Y);
            Grid.SetColumn(button, i_X);
            Grid.SetRow(button, i_Y);
            this.ButtonsGrid.Children.Add(button);
            button.Click += Button_Click;
            r_BoardMatrix[i_X, i_Y] = button;
        }

        private void createRowsAndCols(int i_Size)
        {
            for (int i = 0; i < i_Size; i++)
            {
                this.ButtonsGrid.RowDefinitions.Add(new RowDefinition());
                this.ButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void PlayTurns()
        {
            //This method run after we make the move
            updateBoard();
            updateScore();
            updateCurrentPlaying();
            updateAvailableMoves();

            if (r_Game.IsGameOver)
            {
                MessageBoxResult result = MessageBox.Show("Game over, Do you want to Play again??",
                                          "Confirmation",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    r_Game.startNewRound();
                    PlayTurns();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void updateAvailableMoves()
        {
            r_Game.UpdateAvailableMoves();
            foreach (System.Drawing.Point moveCell in r_Game.AvailableMoves)
            {
                r_BoardMatrix[moveCell.X, moveCell.Y].IsEnabled = true;
                r_BoardMatrix[moveCell.X, moveCell.Y].Content = "";
            }
        }

        private void updateCurrentPlaying()
        {
            if (r_Game.ColorPlaying == eCoinColor.Black)
            {
                NowPlayingLabel.Text = "Black";
                BlackScoreCoin.Style = Resources["PlayingCoinBorderStyle"] as Style;
                WhiteScoreCoin.Style = Resources["CoinBorderStyle"] as Style;
            }
            else
            {
                NowPlayingLabel.Text = "White";
                WhiteScoreCoin.Style = Resources["PlayingCoinBorderStyle"] as Style;
                BlackScoreCoin.Style = Resources["CoinBorderStyle"] as Style;
            }
        }

        private void updateScore()
        {
            r_Game.GetScoreOfUsers(out m_SumOfBlackPoints, out m_SumOfWhitePoints);
            WhiteScore.Text = m_SumOfWhitePoints.ToString();
            BlackScore.Text = m_SumOfBlackPoints.ToString();
        }

        private void updateBoard()
        {
            for (int x = 0; x < r_Game.Board.SizeOfBoard; x++)
            {
                for (int y = 0; y < r_Game.Board.SizeOfBoard; y++)
                {
                    updateBoardButton(r_Game, x, y);
                }
            }
        }

        private void updateBoardButton(GameManager i_Game, int i_X, int i_Y)
        {
            r_BoardMatrix[i_X, i_Y].Content = r_eCoinToText[i_Game.Board.BoardMatrix[i_X, i_Y]];
            r_BoardMatrix[i_X, i_Y].IsEnabled = false;
            r_BoardMatrix[i_X, i_Y].Style = Resources[r_eCoinToText[i_Game.Board.BoardMatrix[i_X, i_Y]]] as Style;
        }

        #region Events
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            r_Game.MakeMove((System.Drawing.Point)clickedButton.Tag);
            PlayTurns();
        }
        private void GameBoard_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion
    }
}
