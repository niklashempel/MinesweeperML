using System.Windows;

namespace MinesweeperML.Views
{
    /// <summary>
    /// Interaction logic for GameWonDialog.xaml.
    /// </summary>
    public partial class GameWonDialog : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameWonDialog" /> class.
        /// </summary>
        public GameWonDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}