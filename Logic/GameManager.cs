using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Logic
{
    public class GameManager
    {
        public enum eGameType
        {
            PlayerVsPlayer,
            PlayerVsComputer
        }
        private readonly eGameType m_GameType;
        private readonly User m_BlackPlayer;
        private readonly User m_WhitePlayer;
        private List<Point> m_AvailableMoves = new List<Point>();
        private Board m_Board;
        private eCoinColor m_ColorPlaying = eCoinColor.Black;
        private bool m_IsGameOver = false;
        private int m_CountOfSwapTurnWithOutMoves = 0;
        private int m_CountOfRounds = 0;
        private User m_LastWinner = null;
        private User m_LastLoser = null;

        public List<Point> AvailableMoves
        {
            get { return m_AvailableMoves; }
            set { m_AvailableMoves = value; }
        }

        public eCoinColor ColorPlaying
        {
            get { return m_ColorPlaying; }
            set { m_ColorPlaying = value; }
        }

        public Board Board
        {
            get { return m_Board; }
            set { m_Board = value; }
        }

        public bool IsGameOver { get => m_IsGameOver; }

        public int CountOfRounds { get => m_CountOfRounds; }

        public User LastWinner { get => m_LastWinner; }

        public User LastLoser { get => m_LastLoser; }

        public User WhitePlayer => m_WhitePlayer;

        public User BlackPlayer => m_BlackPlayer;

        public GameManager(int i_BoardSize, eGameType i_GameType, string i_BlackPlayerName, string i_WhitePlayerName)
        {
            User.UserOption whitePlayerType;
            m_GameType = i_GameType;
            m_Board = new Board(i_BoardSize);

            whitePlayerType = i_GameType == eGameType.PlayerVsComputer ? User.UserOption.Computer : User.UserOption.Human;

            m_BlackPlayer = new User(User.UserOption.Human, eCoinColor.Black, i_BlackPlayerName);
            m_WhitePlayer = new User(whitePlayerType, eCoinColor.White, i_WhitePlayerName);
        }

        public GameManager(eGameType i_GameType, User i_BlackPlayer, User i_WhitePlayer)
        {
            m_GameType = i_GameType;
            m_BlackPlayer = i_BlackPlayer;
            m_WhitePlayer = i_WhitePlayer;
        }

        /*Area's Function*/
        public void MakeMove(Point i_PointToPlay)
        {
            m_Board.MakeMove(i_PointToPlay, m_ColorPlaying);
            swapCurrentColorPlaying();
        }

        public GameManager Clone()
        {
            GameManager clonedGame = new GameManager(m_GameType, BlackPlayer, WhitePlayer);
            clonedGame.m_ColorPlaying = m_ColorPlaying;
            clonedGame.Board = Board.Clone();
            m_AvailableMoves = CloneAvailableMoves();

            return clonedGame;
        }

        public List<Point> CloneAvailableMoves()
        {
            List<Point> cloneAvailableMoves = new List<Point>();
            foreach (Point pointToMove in m_AvailableMoves)
            {
                cloneAvailableMoves.Add(pointToMove);
            }

            return cloneAvailableMoves;
        }

        private void swapCurrentColorPlaying()
        {
            m_ColorPlaying = Board.SwapColor(m_ColorPlaying);
            UpdateAvailableMoves();
            if (m_CountOfSwapTurnWithOutMoves <= 2)
            {
                if (!IsThereAvailableMove())
                {
                    ++m_CountOfSwapTurnWithOutMoves;
                    swapCurrentColorPlaying();
                }
                else
                {
                    if (m_GameType == eGameType.PlayerVsComputer && m_ColorPlaying == eCoinColor.White)
                    {
                        Point cell = AI.FindMaxMinPoint(this);
                        m_Board.MakeMove(cell, m_ColorPlaying);
                        swapCurrentColorPlaying();
                    }

                    m_CountOfSwapTurnWithOutMoves = 0;
                }
            }
            else
            {
                endOfGame();
            }
        }

        private void endOfGame()
        {
            UpdateScoreOfUsers();
            m_IsGameOver = true;
        }

        public void startNewRound()
        {
            m_CountOfSwapTurnWithOutMoves = 0;
            Board.InitBoard();
            m_ColorPlaying = eCoinColor.Black;
            m_IsGameOver = false;
        }

        public void GetScoreOfUsers(out int o_SumOfBlackPoints, out int o_SumOfWhitePoints)
        {
            Board.UpdateScoreOfUser(out o_SumOfBlackPoints, out o_SumOfWhitePoints);
        }

        public void UpdateScoreOfUsers()
        {
            m_CountOfRounds++;
            User winner = null;
            User loser = null;
            Board.UpdateScoreOfUser(out int sumOfBlackPoints, out int sumOfWhitePoints);
            BlackPlayer.PointsInLastGame = sumOfBlackPoints;
            WhitePlayer.PointsInLastGame = sumOfWhitePoints;

            if (sumOfBlackPoints > sumOfWhitePoints)
            {
                BlackPlayer.CountOfWins++;
                winner = BlackPlayer;
                loser = WhitePlayer;
            }
            else if (sumOfWhitePoints > sumOfBlackPoints)
            {
                WhitePlayer.CountOfWins++;
                winner = WhitePlayer;
                loser = BlackPlayer;
            }

            m_LastWinner = winner;
            m_LastLoser = loser;
        }

        public void UpdateAvailableMoves()
        {
            m_AvailableMoves.Clear();
            Point cell = new Point(0, 0);
            for (cell.X = 0; cell.X < Board.SizeOfBoard; ++cell.X)
            {
                for (cell.Y = 0; cell.Y < Board.SizeOfBoard; ++cell.Y)
                {
                    if (IsPointIsAvailableMove(cell))
                    {
                        m_AvailableMoves.Add(cell);
                    }
                }
            }
        }

        public bool IsThereAvailableMove()
        {
            return m_AvailableMoves.Count != 0;
        }

        public bool IsPointInAvailableMoveList(Point i_PointToCheck)
        {
            return m_AvailableMoves.Contains(i_PointToCheck);
        }

        public bool IsPointIsAvailableMove(Point i_PointToCheck)
        {
            bool isFoundAvailableMove = false;
            if (Board.IsEmptyCell(i_PointToCheck))
            {
                for (int x = -1; x < 2 && !isFoundAvailableMove; ++x)
                {
                    for (int y = -1; y < 2 && !isFoundAvailableMove; ++y)
                    {
                        isFoundAvailableMove = Board.IsLegalDirection(m_ColorPlaying, i_PointToCheck, x, y);
                    }
                }
            }

            return isFoundAvailableMove;
        }

    }
}
