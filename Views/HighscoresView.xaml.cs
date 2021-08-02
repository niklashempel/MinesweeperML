using System.Windows.Controls;
using MinesweeperML.ViewModels;

namespace MinesweeperML.Views
{
    /// <summary>
    /// Interaction logic for HighscoresView.xaml.
    /// </summary>
    public partial class HighscoresView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HighscoresView" /> class.
        /// </summary>
        public HighscoresView()
        {
            InitializeComponent();
        }

        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            (this.DataContext as HighscoresViewModel).SortingCommand.Execute(e);
        }
    }
}