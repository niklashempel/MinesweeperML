using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using MinesweeperML.Business.Commands;
using MinesweeperML.Business.Database.DbContexts;
using MinesweeperML.Enumerations;
using System;

namespace MinesweeperML.ViewsModel
{
    /// <summary>
    /// Main menu view model.
    /// </summary>
    /// <seealso cref="BaseViewModel" />
    public class MainMenuViewModel : BaseViewModel
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        private readonly CustomGameViewModel customGameViewModel;

        private readonly HighscoresViewModel highscoresViewModel;

        private readonly MinesweeperViewModel minesweeperViewModel;
        private bool _toogleDarkmode;

        /// <summary>
        /// Gets the show highscores command.
        /// </summary>
        /// <value>The show highscores command.</value>
        public RelayCommand ShowHighscoresCommand { get; private set; }

        /// <summary>
        /// Gets the start customer game command.
        /// </summary>
        /// <value>The start customer game command.</value>
        public RelayCommand StartCustomGameCommand { get; private set; }

        /// <summary>
        /// Gets the start easy game command.
        /// </summary>
        /// <value>The start easy game command.</value>
        public RelayCommand StartEasyGameCommand { get; private set; }

        /// <summary>
        /// Gets the start hard game command.
        /// </summary>
        /// <value>The start hard game command.</value>
        public RelayCommand StartHardGameCommand { get; private set; }

        /// <summary>
        /// Gets the start medium game command.
        /// </summary>
        /// <value>The start medium game command.</value>
        public RelayCommand StartMediumGameCommand { get; private set; }

        /// <summary>
        /// Gets or sets the start window view model.
        /// </summary>
        /// <value>The start window view model.</value>
        public StartWindowViewModel StartWindowViewModel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [toogle darkmode].
        /// </summary>
        /// <value><c>true</c> if [toogle darkmode]; otherwise, <c>false</c>.</value>
        public bool ToogleDarkmode
        {
            get => _toogleDarkmode;
            set
            {
                if (value != _toogleDarkmode)
                {
                    _toogleDarkmode = value;
                    NotifyPropertyChanged(nameof(_toogleDarkmode));

                    ITheme theme = _paletteHelper.GetTheme();
                    IBaseTheme baseTheme = _toogleDarkmode ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
                    theme.SetBaseTheme(baseTheme);
                    _paletteHelper.SetTheme(theme);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenuViewModel" /> class.
        /// </summary>
        /// <param name="highscoresViewModel">The highscores view model.</param>
        /// <param name="customGameViewModel">The custom game view model.</param>
        /// <param name="minesweeperViewModel">The minesweeper view model.</param>
        public MainMenuViewModel(HighscoresViewModel highscoresViewModel, CustomGameViewModel customGameViewModel, MinesweeperViewModel minesweeperViewModel)
        {
            StartEasyGameCommand = new RelayCommand(param => this.StartEasyGame());
            StartMediumGameCommand = new RelayCommand(param => this.StartMediumGame());
            StartHardGameCommand = new RelayCommand(param => this.StartHardGame());
            StartCustomGameCommand = new RelayCommand(param => this.StartCustomGame());
            ShowHighscoresCommand = new RelayCommand(param => this.ShowHighscores());
            this.highscoresViewModel = highscoresViewModel;
            this.customGameViewModel = customGameViewModel;
            this.minesweeperViewModel = minesweeperViewModel;
            customGameViewModel.MinesweeperViewModel = minesweeperViewModel;
        }

        private void ShowHighscores()
        {
            StartWindowViewModel.SelectedViewModel = this.highscoresViewModel;
            this.highscoresViewModel.MainWindowViewModel = this;
            this.highscoresViewModel.SkipToFirstPageCommand.Execute(null);
        }

        private void StartCustomGame()
        {
            this.customGameViewModel.MainMenuViewModel = this;
            StartWindowViewModel.SelectedViewModel = this.customGameViewModel;
        }

        private void StartEasyGame()
        {
            StartGame(10, 8, 7, Difficulty.Easy);
        }

        private void StartGame(int columns, int rows, int numberOfBombs, Difficulty difficulty)
        {
            this.minesweeperViewModel.MainMenuViewModel = this;
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