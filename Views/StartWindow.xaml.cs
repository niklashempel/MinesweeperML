using System.Windows;
using Microsoft.EntityFrameworkCore;
using MinesweeperML.Business.Database.DbContexts;
using MinesweeperML.ViewModels;

namespace MinesweeperML.Views
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml.
    /// </summary>
    public partial class StartWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartWindow" /> class.
        /// </summary>
        public StartWindow()
        {
            InitializeComponent();
        }
    }
}