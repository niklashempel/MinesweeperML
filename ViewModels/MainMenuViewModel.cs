using System;
using Microsoft.EntityFrameworkCore;
using MinesweeperML.Business.Commands;
using MinesweeperML.Business.Database.DbContexts;
using MinesweeperML.Enumerations;
using MinesweeperML.ViewModels;

namespace MinesweeperML.ViewModels
{
    /// <summary>
    /// Main menu view model.
    /// </summary>
    /// <seealso cref="BaseViewModel" />
    public class MainMenuViewModel : BaseViewModel
    {
        private readonly HighscoresViewModel highscoresViewModel;
        private readonly SelectNewGameViewModel selectNewGameViewModel;

        private RelayCommand startNewGameCommand;

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
        /// Gets the start new game command.
        /// </summary>
        /// <value>The start new game command.</value>
        public RelayCommand StartNewGameCommand
        {
            get
            {
                return startNewGameCommand ??= new RelayCommand(param => this.StartNewGame());
            }
        }

        /// <summary>
        /// Gets or sets the start window view model.
        /// </summary>
        /// <value>The start window view model.</value>
        public StartWindowViewModel StartWindowViewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenuViewModel" /> class.
        /// </summary>
        /// <param name="highscoresViewModel">The highscores view model.</param>
        /// <param name="selectNewGameViewModel">The select new game view model.</param>
        public MainMenuViewModel(HighscoresViewModel highscoresViewModel, SelectNewGameViewModel selectNewGameViewModel)
        {
            ShowHighscoresCommand = new RelayCommand(param => this.ShowHighscores());
            this.highscoresViewModel = highscoresViewModel;
            this.selectNewGameViewModel = selectNewGameViewModel;
        }

        private void ShowHighscores()
        {
            StartWindowViewModel.SelectedViewModel = this.highscoresViewModel;
            this.highscoresViewModel.MainWindowViewModel = this;
            this.highscoresViewModel.SkipToFirstPageCommand.Execute(null);
        }

        private void StartNewGame()
        {
            this.selectNewGameViewModel.SetNavigationViewModels(this, StartWindowViewModel);
            StartWindowViewModel.SelectedViewModel = this.selectNewGameViewModel;
        }
    }
}