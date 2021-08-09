using Microsoft.EntityFrameworkCore;
using MinesweeperML.Business.Commands;
using MinesweeperML.Business.Database.DbContexts;
using MinesweeperML.ViewModels;

namespace MinesweeperML.ViewModels
{
    /// <summary>
    /// Custom game view model.
    /// </summary>
    /// <seealso cref="BaseViewModel" />
    public class CustomGameViewModel : BaseViewModel
    {
        private int bombs = 1;
        private int columns = 2;
        private RelayCommand goBackCommand;
        private int rows = 2;

        private RelayCommand startGameCommand;

        /// <summary>
        /// Gets or sets the bombs.
        /// </summary>
        /// <value>The bombs.</value>
        public int Bombs
        {
            get
            {
                return bombs;
            }
            set
            {
                if (value != bombs)
                {
                    bombs = value;
                    NotifyPropertyChanged(nameof(Bombs));
                }
            }
        }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public int Columns
        {
            get
            {
                return columns;
            }
            set
            {
                if (value != columns)
                {
                    columns = value;
                    NotifyPropertyChanged(nameof(Columns));
                    NotifyPropertyChanged(nameof(MaxBombs));
                    if (Bombs > MaxBombs)
                    {
                        Bombs = MaxBombs;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the go back command.
        /// </summary>
        /// <value>The go back command.</value>
        public RelayCommand GoBackCommand => goBackCommand ??= new RelayCommand(param => this.GoBack());

        /// <summary>
        /// Gets or sets the main menu view model.
        /// </summary>
        /// <value>The main menu view model.</value>
        public MainMenuViewModel MainMenuViewModel { get; set; }

        /// <summary>
        /// Gets the maximum bombs.
        /// </summary>
        /// <value>The maximum bombs.</value>
        public int MaxBombs => (Columns * Rows) - 1;

        /// <summary>
        /// Gets or sets the minesweeper view model.
        /// </summary>
        /// <value>The minesweeper view model.</value>
        public MinesweeperViewModel MinesweeperViewModel { get; set; }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>The rows.</value>
        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                if (value != rows)
                {
                    rows = value;
                    NotifyPropertyChanged(nameof(Rows));
                    NotifyPropertyChanged(nameof(MaxBombs));
                    if (Bombs > MaxBombs)
                    {
                        Bombs = MaxBombs;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the select new game view model.
        /// </summary>
        /// <value>The select new game view model.</value>
        public SelectNewGameViewModel SelectNewGameViewModel { get; set; }

        /// <summary>
        /// Gets the start game command.
        /// </summary>
        /// <value>The start game command.</value>
        public RelayCommand StartGameCommand => startGameCommand ??= new RelayCommand(param => this.StartGame());

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomGameViewModel" /> class.
        /// </summary>
        public CustomGameViewModel()
        {
        }

        private void GoBack()
        {
            MainMenuViewModel.StartWindowViewModel.SelectedViewModel = this.SelectNewGameViewModel;
        }

        private void StartGame()
        {
            this.MinesweeperViewModel.MainMenuViewModel = this.MainMenuViewModel;
            this.MinesweeperViewModel.SetGameSettings(columns, rows, bombs, Enumerations.Difficulty.Custom);
            this.MinesweeperViewModel.StartGame();
            MainMenuViewModel.StartWindowViewModel.SelectedViewModel = this.MinesweeperViewModel;
        }
    }
}