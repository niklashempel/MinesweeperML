using MinesweeperML.Business.Commands;
using MinesweeperML.Enumerations;
using MinesweeperML.ViewModels;

namespace MinesweeperML.ViewModels
{
    /// <summary>
    /// View model to select a new game.
    /// </summary>
    /// <seealso cref="MinesweeperML.ViewModels.BaseViewModel" />
    public class SelectNewGameViewModel : BaseViewModel
    {
        private readonly CustomGameViewModel customGameViewModel;
        private readonly MinesweeperViewModel minesweeperViewModel;
        private RelayCommand goBackCommand;
        private MainMenuViewModel mainMenuViewModel;
        private RelayCommand startCustomGameCommand;
        private RelayCommand startEasyGameCommand;

        private RelayCommand startHardGameCommand;

        private RelayCommand startMediumGameCommand;

        /// <summary>
        /// Gets the go back command.
        /// </summary>
        /// <value>The go back command.</value>
        public RelayCommand GoBackCommand
        {
            get
            {
                return goBackCommand ??= new RelayCommand(param => this.GoBack());
            }
        }

        /// <summary>
        /// Gets the start custom game command.
        /// </summary>
        /// <value>The start custom game command.</value>
        public RelayCommand StartCustomGameCommand
        {
            get
            {
                return startCustomGameCommand ??= new RelayCommand(param => this.StartCustomGame());
            }
        }

        /// <summary>
        /// Gets the start easy game command.
        /// </summary>
        /// <value>The start easy game command.</value>
        public RelayCommand StartEasyGameCommand
        {
            get
            {
                return startEasyGameCommand ??= new RelayCommand(param => this.StartEasyGame());
            }
        }

        /// <summary>
        /// Gets the start hard game command.
        /// </summary>
        /// <value>The start hard game command.</value>
        public RelayCommand StartHardGameCommand
        {
            get
            {
                return startHardGameCommand ??= new RelayCommand(param => this.StartHardGame());
            }
        }

        /// <summary>
        /// Gets the start medium game command.
        /// </summary>
        /// <value>The start medium game command.</value>
        public RelayCommand StartMediumGameCommand
        {
            get
            {
                return startMediumGameCommand ??= new RelayCommand(param => this.StartMediumGame());
            }
        }

        /// <summary>
        /// Gets or sets the start window view model.
        /// </summary>
        /// <value>The start window view model.</value>
        public StartWindowViewModel StartWindowViewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectNewGameViewModel" /> class.
        /// </summary>
        /// <param name="customGameViewModel">The custom game view model.</param>
        /// <param name="minesweeperViewModel">The minesweeper view model.</param>
        public SelectNewGameViewModel(CustomGameViewModel customGameViewModel, MinesweeperViewModel minesweeperViewModel)
        {
            this.customGameViewModel = customGameViewModel;
            this.minesweeperViewModel = minesweeperViewModel;
            customGameViewModel.MinesweeperViewModel = minesweeperViewModel;
        }

        /// <summary>
        /// Sets the navigation view models.
        /// </summary>
        /// <param name="mainMenuViewModel">The main menu view model.</param>
        /// <param name="startWindowViewModel">The start window view model.</param>
        public void SetNavigationViewModels(MainMenuViewModel mainMenuViewModel, StartWindowViewModel startWindowViewModel)
        {
            this.mainMenuViewModel = mainMenuViewModel;
            this.StartWindowViewModel = startWindowViewModel;
        }

        private void GoBack()
        {
            mainMenuViewModel.StartWindowViewModel.SelectedViewModel = this.mainMenuViewModel;
        }

        private void StartCustomGame()
        {
            this.customGameViewModel.MainMenuViewModel = this.mainMenuViewModel;
            this.customGameViewModel.SelectNewGameViewModel = this;
            StartWindowViewModel.SelectedViewModel = this.customGameViewModel;
        }

        private void StartEasyGame()
        {
            StartGame(10, 8, 7, Difficulty.Easy);
        }

        private void StartGame(int columns, int rows, int numberOfBombs, Difficulty difficulty)
        {
            this.minesweeperViewModel.MainMenuViewModel = this.mainMenuViewModel;
            this.minesweeperViewModel.StartGame(columns, rows, numberOfBombs, difficulty);
            StartWindowViewModel.SelectedViewModel = minesweeperViewModel;
        }

        private void StartHardGame()
        {
            StartGame(20, 15, 70, Difficulty.Hard);
        }

        private void StartMediumGame()
        {
            StartGame(15, 10, 30, Difficulty.Medium);
        }
    }
}