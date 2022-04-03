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

namespace CrazyEightsGroupProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PlayClick(object sender, RoutedEventArgs e)
        {
            PlayerChoice playerChoice = new PlayerChoice();
            playerChoice.Show();
            this.Close();
        }

        private void StatisticsClick(object sender, RoutedEventArgs e)
        {
            Statistics objStatistics = new Statistics();
            this.Visibility = Visibility.Hidden;
            objStatistics.Show();

        }

        private void InstructionsClick(object sender, RoutedEventArgs e)
        {
            Instructions objInstructions = new Instructions();
            this.Visibility = Visibility.Hidden;
            objInstructions.Show();
        }
    }
}
