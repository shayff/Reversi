using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public class User
    {
        public enum UserOption
        {
            Human,
            Computer,
        }

        private readonly UserOption m_UserType;
        private eCoinColor m_CoinColor;
        private string m_Name;
        private int m_CountOfWins = 0;
        private int m_PointsInLastGame = 0;

        /*get set*/
        public eCoinColor CoinColor
        {
            get { return m_CoinColor; }
            set { m_CoinColor = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public UserOption UserType
        {
            get { return m_UserType; }
        }

        public int CountOfWins { get => m_CountOfWins; set => m_CountOfWins = value; }

        public int PointsInLastGame { get => m_PointsInLastGame; set => m_PointsInLastGame = value; }

        /*ctor*/
        public User(UserOption i_UserType, eCoinColor i_CoinColor, string i_Name)
        {
            m_Name = i_Name;
            CoinColor = i_CoinColor;
            m_UserType = i_UserType;
        }
    }
}
