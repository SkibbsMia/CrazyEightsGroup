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

namespace CrazyEightsGroupProject
{
    /// <summary>
    /// Interaction logic for PlayerChoice.xaml
    /// </summary>
    public partial class PlayerChoice : Window
    {
        public PlayerChoice()
        {
            InitializeComponent();
        }

        private void ContinueClick(object sender, RoutedEventArgs e)
        {
            int numberOfPlayers = 4;

            if (twoPlayer.IsChecked == true)
            {
                numberOfPlayers = 2;
            }
            else if (threePlayer.IsChecked == true)
            {
                numberOfPlayers = 3;
            }
            else if (fourPlayer.IsChecked == true)
            {
                numberOfPlayers = 4;
            }

            PlayScreen playerScreen = new PlayScreen(numberOfPlayers);
            playerScreen.Show();
            this.Close();
        }
    }
}
