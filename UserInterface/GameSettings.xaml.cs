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
using Logic;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for GameSettings.xaml
    /// </summary>
    public partial class GameSettings : Window
    {
        int[] m_BoardSizes = { 6, 8, 10 };
        GameBoard r_GameBoardWindow;
        GameManager.eGameType m_GameType = GameManager.eGameType.PlayerVsPlayer;

        public GameSettings()
        {
            InitializeComponent();
            BoardSizes_ComboBox.ItemsSource = m_BoardSizes;
            BoardSizes_ComboBox.SelectedIndex = 0;
        }

        #region Events
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            r_GameBoardWindow = new GameBoard((int)this.BoardSizes_ComboBox.SelectedValue, m_GameType);
            r_GameBoardWindow.Show();
            this.Hide();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {    
            m_GameType = GameTypePlayerRadioBtn.IsChecked == true ? GameManager.eGameType.PlayerVsPlayer : GameManager.eGameType.PlayerVsComputer;
        }
        #endregion
    }
}
